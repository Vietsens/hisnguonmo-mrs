using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisDeathWithin;
using MOS.MANAGER.HisDepartment;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00063
{
    public class Mrs00063Processor : AbstractProcessor
    {
        Mrs00063Filter castFilter = null;
        List<Mrs00063RDO> ListRdo = new List<Mrs00063RDO>();
        string DEPARTMENT_NAME = "";
        List<V_HIS_TREATMENT> ListTreatment;
        List<V_HIS_PATIENT> ListPatient = new List<V_HIS_PATIENT>();

        CommonParam paramGet = new CommonParam();

        public Mrs00063Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00063Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00063Filter)this.reportFilter);
                //if (castFilter.DEPARTMENT_ID > 0)
                //{
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                treatmentFilter.OUT_TIME_FROM = castFilter.OUT_TIME_FROM;
                treatmentFilter.OUT_TIME_TO = castFilter.OUT_TIME_TO;
                treatmentFilter.IS_PAUSE = true;
                ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);

                if (IsNotNullOrEmpty(ListTreatment))
                {
                    if (castFilter.DEPARTMENT_IDs!=null)
                    {
                        ListTreatment = ListTreatment.Where(o => castFilter.DEPARTMENT_IDs.Contains(o.END_DEPARTMENT_ID??0)).ToList();
                    }
                    var patientIds = ListTreatment.Select(s => s.PATIENT_ID).Distinct().ToList();
                    int skip = 0;
                    while (patientIds.Count - skip > 0)
                    {
                        var listId = patientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisPatientViewFilterQuery patientFilter = new HisPatientViewFilterQuery();
                        patientFilter.IDs = listId;
                        var patient = new MOS.MANAGER.HisPatient.HisPatientManager(paramGet).GetView(patientFilter);
                        if (IsNotNullOrEmpty(patient))
                        {
                            ListPatient.AddRange(patient);
                        }
                    }
                }

                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Error("Co exception xay ra tai DAOGET trong qua trinh lay du lieuj V_HIS_TREATMENT MRS00062." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatmentFilter), treatmentFilter));
                    throw new DataMisalignedException();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    ListTreatment = ListTreatment.Where(o => o.TREATMENT_END_TYPE_ID != null && o.IS_PAUSE == 1).ToList();
                    ProcessListTreatment(paramGet, ListTreatment);
                    GetDepartmentById();
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

        private void ProcessListTreatment(CommonParam paramGet, List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    //ListTreatment = ListTreatment.Where(o => o.IS_ACTIVE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList(); 
                    if (castFilter.DEPARTMENT_ID.HasValue)
                    {
                        ListTreatment = ListTreatment.Where(o => o.END_DEPARTMENT_ID == castFilter.DEPARTMENT_ID.Value).ToList();
                    }
                    int start = 0;
                    int count = ListTreatment.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_TREATMENT> hisTreatments = ListTreatment.Skip(start).Take(limit).ToList();
                        HisPatientTypeAlterViewFilterQuery alterFilter = new HisPatientTypeAlterViewFilterQuery();
                        alterFilter.TREATMENT_IDs = hisTreatments.Select(s => s.ID).ToList();
                        List<V_HIS_PATIENT_TYPE_ALTER> hisPatientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(alterFilter);
                        ProcessDetailListTreatment(paramGet, hisTreatments, hisPatientTypeAlters);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessDetailListTreatment(CommonParam paramGet, List<V_HIS_TREATMENT> hisTreatments, List<V_HIS_PATIENT_TYPE_ALTER> hisPatientTypeAlters)
        {
            try
            {
                if (IsNotNullOrEmpty(hisPatientTypeAlters))
                {
                    Dictionary<long, HIS_ICD> dicIcd = GetIcd();
                    foreach (var treatment in hisTreatments)
                    {
                        Mrs00063RDO rdo = new Mrs00063RDO();
                        rdo.FEE_LOCK_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.FEE_LOCK_TIME ?? 0);
                        rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                        rdo.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        rdo.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        CalcuatorAge(rdo, treatment);
                        if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)// Config.IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
                        {
                            rdo.IS_CURED = "X";
                        }
                        else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)// Config.IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
                        {
                            rdo.IS_ABATEMENT = "X";
                        }
                        else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)// Config.IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                        {
                            rdo.IS_UNCHANGED = "X";
                        }
                        else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)// Config.IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                        {
                            rdo.IS_AGGRAVATION = "X";
                        }

                        var listPatientTypeAlter = hisPatientTypeAlters.Where(o => o.TREATMENT_ID == treatment.ID).ToList();

                        if (IsNotNullOrEmpty(listPatientTypeAlter))
                        {
                            if (castFilter.TREATMENT_TYPE_IDs != null)
                            {
                                if (!castFilter.TREATMENT_TYPE_IDs.Contains(listPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ThenByDescending(p => p.ID).ToList()[0].TREATMENT_TYPE_ID))
                                {
                                    continue;
                                }
                            }
                            if (listPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList()[0].PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.IS_BHYT = "X";
                                rdo.GIOITHIEU = treatment.MEDI_ORG_NAME;
                                rdo.TRANSFER_IN_MEDI_ORG_NAME = treatment.TRANSFER_IN_MEDI_ORG_NAME;
                                rdo.HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                            }
                        }

                        string outTimeStr = "";
                        if (treatment.OUT_TIME.HasValue)
                        {
                            outTimeStr = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME.Value);
                            rdo.TOTAL_DATE_TREATMENT = HIS.Treatment.DateTime.Calculation.DayOfTreatment(treatment.IN_TIME, treatment.OUT_TIME.Value);
                        }
                        if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET)// Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH)
                        {
                            rdo.DATE_DEAD_STR = outTimeStr;

                            if (treatment.DEATH_WITHIN_ID == MRS.MANAGER.Config.HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS)
                            {
                                rdo.IS_DEAD_IN_24H = "X";
                            }
                        }
                        else
                        {
                            rdo.DATE_OUT_STR = outTimeStr;
                        }
                        rdo.DIAGNOSE = treatment.ICD_NAME;
                        rdo.ADVISE = treatment.ADVISE;
                        rdo.TREATMENT_RESULT_CODE = treatment.TREATMENT_RESULT_CODE;
                        rdo.TREATMENT_RESULT_NAME = treatment.TREATMENT_RESULT_NAME;
                        rdo.STORE_CODE = treatment.STORE_CODE;
                        rdo.GENDER_NAME = treatment.TDL_PATIENT_GENDER_NAME;
                        rdo.APPOINTMENT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.APPOINTMENT_TIME ?? 0);
                        rdo.TREATMENT_END_TYPE_CODE = treatment.TREATMENT_END_TYPE_CODE;
                        rdo.TREATMENT_END_TYPE_NAME = treatment.TREATMENT_END_TYPE_NAME;
                        rdo.END_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME;
                        rdo.END_ROOM_NAME = treatment.END_ROOM_NAME;
                        rdo.DOCTOR_USERNAME = treatment.DOCTOR_USERNAME;
                        rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.IN_TIME);
                        rdo.IN_TIME = treatment.IN_TIME;
                        rdo.OUT_TIME = treatment.OUT_TIME;
                        rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0);
                        rdo.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);

                        var patient = ListPatient.FirstOrDefault(o => o.ID == treatment.PATIENT_ID);
                        if (patient != null)
                        {
                            rdo.CCCD_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patient.CCCD_DATE ?? 0);
                            rdo.CCCD_NUMBER = patient.CCCD_NUMBER;
                            rdo.CCCD_PLACE = patient.CCCD_PLACE;
                            rdo.CMND_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patient.CMND_DATE ?? 0);
                            rdo.CMND_NUMBER = patient.CMND_NUMBER;
                            rdo.CMND_PLACE = patient.CMND_PLACE;
                            rdo.CAREER_NAME = patient.CAREER_NAME;
                            rdo.EMAIL = patient.EMAIL;
                            rdo.ETHNIC_NAME = patient.ETHNIC_NAME;
                            rdo.FATHER_NAME = patient.FATHER_NAME;
                            rdo.MILITARY_RANK_NAME = patient.MILITARY_RANK_NAME;
                            rdo.MOTHER_NAME = patient.MOTHER_NAME;
                            rdo.NATIONAL_NAME = patient.NATIONAL_NAME;
                            rdo.MOBILE = patient.MOBILE;
                            rdo.PHONE = patient.PHONE;
                            rdo.RELATIVE_ADDRESS = patient.RELATIVE_ADDRESS;
                            rdo.RELATIVE_CMND_NUMBER = patient.RELATIVE_CMND_NUMBER;
                            rdo.RELATIVE_MOBILE = patient.RELATIVE_MOBILE;
                            rdo.RELATIVE_NAME = patient.RELATIVE_NAME;
                            rdo.RELATIVE_PHONE = patient.RELATIVE_PHONE;
                            rdo.RELATIVE_TYPE = patient.RELATIVE_TYPE;
                            rdo.RELIGION_NAME = patient.RELIGION_NAME;
                        }

                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private Dictionary<long, HIS_ICD> GetIcd()
        {
            Dictionary<long, HIS_ICD> result = new Dictionary<long, HIS_ICD>();
            try
            {
                CommonParam param = new CommonParam();
                HisIcdFilterQuery filter = new HisIcdFilterQuery();
                var listIcd = new MOS.MANAGER.HisIcd.HisIcdManager(param).Get(filter);
                foreach (var item in listIcd)
                {
                    if (!result.ContainsKey(item.ID)) result[item.ID] = item;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        private void GetDepartmentById()
        {
            try
            {
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    var department = new MOS.MANAGER.HisDepartment.HisDepartmentManager().GetById(castFilter.DEPARTMENT_ID.Value);
                    if (IsNotNull(department))
                    {
                        DEPARTMENT_NAME = department.DEPARTMENT_NAME.ToUpper();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void CalcuatorAge(Mrs00063RDO rdo, V_HIS_TREATMENT treatment)
        {
            try
            {
                int? tuoi = RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)// IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
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
                    if (tuoi >= 1 && tuoi <= 6)
                    {
                        rdo.IS_1DEN6TUOI = "X";
                    }
                    rdo.IS_1DEN15TUOI = "X";
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {

                dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? castFilter.OUT_TIME_FROM ?? 0));

                dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? castFilter.OUT_TIME_TO ?? 0));

                dicSingleTag.Add("DEPARTMENT_NAME", DEPARTMENT_NAME);

                ListRdo = ListRdo.OrderBy(o => o.TREATMENT_CODE).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
