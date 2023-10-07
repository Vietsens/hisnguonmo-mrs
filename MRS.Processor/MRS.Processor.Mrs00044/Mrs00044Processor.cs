using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisDepartment;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00044
{
    public class Mrs00044Processor : AbstractProcessor
    {
        Mrs00044Filter castFilter = null;
        List<Mrs00044RDO> ListRdo = new List<Mrs00044RDO>();
        Dictionary<string, HIS_ICD> dicIcd = new Dictionary<string, HIS_ICD>();
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();

        string department_name;

        public Mrs00044Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00044Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00044Filter)this.reportFilter);
                LoadDataToRam();
                dicIcd = LoadDicHisIcd();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void LoadDataToRam()
        {
            try
            {
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                treatmentFilter.CREATE_TIME_FROM = castFilter.CREATE_TIME_FROM;
                treatmentFilter.CREATE_TIME_TO = castFilter.CREATE_TIME_TO;
                ListTreatment = new HisTreatmentManager().GetView(treatmentFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListTreatment.Clear();
            }
        }

        private Dictionary<string, HIS_ICD> LoadDicHisIcd()
        {
            Dictionary<string, HIS_ICD> result = new Dictionary<string, HIS_ICD>();
            try
            {
                CommonParam param = new CommonParam();
                HisIcdFilterQuery filter = new HisIcdFilterQuery();
                var listIcd = new HisIcdManager(param).Get(filter);
                if (IsNotNullOrEmpty(listIcd))
                {
                    foreach (var item in listIcd)
                    {
                        if (String.IsNullOrEmpty(item.ICD_CODE)) continue;
                        if (!result.ContainsKey(item.ICD_CODE)) result[item.ICD_CODE] = item;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ProcessListTreatment();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListTreatment()
        {
            try
            {
                if (ListTreatment != null && ListTreatment.Count > 0)
                {
                    ListTreatment = ListTreatment.Where(o => o.TREATMENT_END_TYPE_ID != null && o.TREATMENT_END_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET && o.IS_PAUSE == 1).ToList();
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListTreatment.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_TREATMENT> treatments = ListTreatment.Skip(start).Take(limit).ToList();

                        HisPatientTypeAlterViewFilterQuery PatientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                        PatientTypeAlterFilter.TREATMENT_IDs = treatments.Select(s => s.ID).ToList();
                        var listPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).GetView(PatientTypeAlterFilter);

                        HisPatientViewFilterQuery patientFilter = new HisPatientViewFilterQuery();
                        patientFilter.IDs = treatments.Select(s => s.PATIENT_ID).ToList();
                        var listPatient = new HisPatientManager(paramGet).GetView(patientFilter);

                        ProcessCurrentListTreatment(paramGet, treatments, listPatient, listPatientTypeAlter);

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("co exception xa ra tai DAOGET trong qua trinh tong hop du lieu");
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessCurrentListTreatment(CommonParam paramGet, List<V_HIS_TREATMENT> treatments, List<V_HIS_PATIENT> listPatient, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            try
            {
                if (treatments != null)
                {
                    foreach (var treatment in treatments)
                    {
                        if (checkTreatmentInDepartment(paramGet, treatment))
                        {
                            Mrs00044RDO rdo = new Mrs00044RDO();
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                            rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                            rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            rdo.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                            //rdo.DIAGNOSE_KDT = treatment.ICD_NAME ?? treatment.ICD_TEXT;
                            SetDateTreatMent(paramGet, ref rdo, treatment);
                            IsBhyt(listPatientTypeAlter, rdo, treatment);
                            CalcuatorAge(rdo, treatment);
                            SetJobForPatient(listPatient, rdo, treatment.PATIENT_ID);
                            if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
                            {
                                rdo.IS_CURED = "X";
                            }
                            else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
                            {
                                rdo.IS_ABATEMENT = "X";
                            }
                            else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                            {
                                rdo.IS_UNCHANGED = "X";
                            }
                            else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                            {
                                rdo.IS_AGGRAVATION = "X";
                            }
                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void IsBhyt(List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter, Mrs00044RDO rdo, V_HIS_TREATMENT treatment)
        {
            try
            {
                if (listPatientTypeAlter != null)
                {
                    var PatientTypeAlter = listPatientTypeAlter.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                    if (PatientTypeAlter != null && PatientTypeAlter.Count > 0)
                    {
                        if (PatientTypeAlter.OrderBy(o => o.LOG_TIME).ToList()[0].PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.IS_BHYT = "X";
                            if (treatment.TRANSFER_IN_ICD_NAME != null && treatment.TRANSFER_IN_ICD_NAME != "")
                            {
                                rdo.DIAGNOSE_TUYENDUOI = treatment.TRANSFER_IN_ICD_NAME ;
                            }
                            else
                            {
                            rdo.DIAGNOSE_TUYENDUOI = (treatment.TRANSFER_IN_ICD_CODE != null && dicIcd.ContainsKey(treatment.TRANSFER_IN_ICD_CODE)) ? dicIcd[treatment.TRANSFER_IN_ICD_CODE].ICD_NAME : "";
                            }
                            
                            rdo.ICD_CODE_TUYENDUOI = treatment.TRANSFER_IN_ICD_CODE;
                            rdo.GIOITHIEU = treatment.MEDI_ORG_NAME;
                        }
                        if (PatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList()[0].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                        {
                            rdo.DIAGNOSE_KKB = treatment.ICD_NAME;
                        }
                        else
                        {
                            rdo.DIAGNOSE_KDT = treatment.ICD_NAME;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetDateTreatMent(CommonParam paramGet, ref Mrs00044RDO rdo, V_HIS_TREATMENT treatment)
        {
            try
            {
                if (treatment != null)
                {
                    if (treatment.CLINICAL_IN_TIME.HasValue)
                    {
                        rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.CLINICAL_IN_TIME ?? 0);
                    }
                    else
                    {
                        rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                    }

                    if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    {
                        rdo.DATE_TRIP_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                    }
                    else
                    {
                        rdo.DATE_OUT_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                    }
                }
                //var departmentTran = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranGet(paramGet).GetHospitalInOut(treatment.ID);
                //if (departmentTran != null && departmentTran.Count > 0)
                //{
                //    rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(departmentTran[0].DEPARTMENT_IN_TIME ?? 0);
                //    if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                //    {
                //        rdo.DATE_TRIP_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(departmentTran[1].DEPARTMENT_IN_TIME ?? 0);
                //    }
                //    else
                //    {
                //        rdo.DATE_OUT_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(departmentTran[1].DEPARTMENT_IN_TIME ?? 0);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool checkTreatmentInDepartment(CommonParam paramGet, V_HIS_TREATMENT treatment)
        {
            bool result = false;
            try
            {
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    HisDepartmentTranViewFilterQuery depaTranFilter = new HisDepartmentTranViewFilterQuery();
                    depaTranFilter.TREATMENT_ID = treatment.ID;
                    depaTranFilter.ORDER_DIRECTION = "DESC";
                    depaTranFilter.ORDER_FIELD = "LOG_TIME";
                    var hisDepartmentTrans = new HisDepartmentTranManager(paramGet).GetView(depaTranFilter);
                    if (IsNotNullOrEmpty(hisDepartmentTrans))
                    {
                        if (hisDepartmentTrans[0].DEPARTMENT_ID == castFilter.DEPARTMENT_ID.Value)
                        {
                            result = true;
                            if (!IsNotNull(department_name))
                                department_name = hisDepartmentTrans[0].DEPARTMENT_NAME;
                        }
                    }
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void CalcuatorAge(Mrs00044RDO rdo, V_HIS_TREATMENT treatment)
        {
            try
            {
                int? tuoi = CalculAge(treatment.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.MALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.FEMALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                }
                if (tuoi == 0)
                {
                    rdo.IS_DUOI_12THANG = "X";
                }
                else if (tuoi >= 1 && tuoi <= 15)
                {
                    rdo.IS_1DEN15TUOI = "X";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private int? CalculAge(long ageNumber)
        {
            int tuoi;
            try
            {
                DateTime dtNgSinh = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ageNumber) ?? DateTime.MinValue;
                TimeSpan diff = DateTime.Now - dtNgSinh;
                long tongsogiay = diff.Ticks;
                if (tongsogiay < 0)
                {
                    tuoi = 0;
                    return 0;
                }
                DateTime newDate = new DateTime(tongsogiay);

                int nam = newDate.Year - 1;
                int thang = newDate.Month - 1;
                int ngay = newDate.Day - 1;
                int gio = newDate.Hour;
                int phut = newDate.Minute;
                int giay = newDate.Second;

                if (nam > 0)
                {
                    tuoi = nam;
                }
                else
                {
                    tuoi = 0;
                    //if (thang > 0)
                    //{
                    //    tuoi = thang.ToString();
                    //    cboAge = "Tháng";
                    //}
                    //else
                    //{
                    //    if (ngay > 0)
                    //    {
                    //        tuoi = ngay.ToString();
                    //        cboAge = "ngày";
                    //    }
                    //    else
                    //    {
                    //        tuoi = "";
                    //        cboAge = "Giờ";
                    //    }
                    //}
                }
                return tuoi;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return null;
            }
        }

        private string ProcessYearDob(long dob)
        {
            try
            {
                if (dob > 0)
                {
                    return dob.ToString().Substring(0, 4);
                }
                return null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        private void SetJobForPatient(List<V_HIS_PATIENT> listPatient, Mrs00044RDO rdo, long patientId)
        {
            try
            {
                if (listPatient != null)
                {
                    var patient = listPatient.FirstOrDefault(o => o.ID == patientId);
                    if (patient != null)
                    {
                        rdo.JOB = patient.CAREER_NAME;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.CREATE_TIME_FROM > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.CREATE_TIME_FROM ?? 0));
                }
                if (castFilter.CREATE_TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.CREATE_TIME_TO ?? 0));
                }
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", department_name);
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
