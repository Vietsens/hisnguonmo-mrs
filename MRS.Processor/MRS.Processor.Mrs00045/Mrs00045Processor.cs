using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisDeathWithin;
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
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00045
{
    public class Mrs00045Processor : AbstractProcessor
    {
        Mrs00045Filter castFilter = null;
        List<Mrs00045RDO> ListRdo = new List<Mrs00045RDO>();
        Dictionary<string, HIS_ICD> dicIcd = new Dictionary<string, HIS_ICD>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        List<HIS_DEATH_CERT_BOOK> ListDeathBook = new List<HIS_DEATH_CERT_BOOK>();
        List<HIS_DEATH_CAUSE> ListDeathCause = new List<HIS_DEATH_CAUSE>();
        List<HIS_DEATH_WITHIN> ListDeathWithin = new List<HIS_DEATH_WITHIN>();
        public object department_name;

        public Mrs00045Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00045Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00045Filter)this.reportFilter);
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00045: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                string query = "select * from his_death_cert_book";
                ListDeathBook = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_DEATH_CERT_BOOK>(query);

                string query1 = "select * from his_death_cause";
                ListDeathCause= new MOS.DAO.Sql.SqlDAO().GetSql<HIS_DEATH_CAUSE>(query1);

                string query2 = "select * from his_death_within";
                ListDeathWithin = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_DEATH_WITHIN>(query2);
                LoadDataToRam();
                dicIcd = LoadDicHisIcd();
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

                string query = @"select mr.creator,trea.* 
from his_rs.his_treatment trea 
left join HIS_MEDI_RECORD mr on mr.id= trea.medi_record_id
where 1=1 and trea.is_pause=1 and trea.treatment_end_type_id=1
";
                //lọc theo loại thời gian
                if (castFilter.INPUT_DATA_ID_TIME_TYPE == 1)//loai thoi gian ra vien
                {
                    query += string.Format("and trea.out_time between {0} and {1}\n", castFilter.CREATE_TIME_FROM, castFilter.CREATE_TIME_TO);
                }
                else if (castFilter.INPUT_DATA_ID_TIME_TYPE == 2)//loai thoi gian luu tru
                {
                    query += string.Format("and mr.store_time between {0} and {1}\n", castFilter.CREATE_TIME_FROM, castFilter.CREATE_TIME_TO);
                }
                else
                {
                    query += string.Format("and trea.out_time between {0} and {1}\n", castFilter.CREATE_TIME_FROM, castFilter.CREATE_TIME_TO);
                }
                Inventec.Common.Logging.LogSystem.Info(query);
                ListTreatment = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);
                
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
                    //ListTreatment = ListTreatment.Where(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET && o.IS_PAUSE == 1).ToList();
                    if (castFilter.DEPARTMENT_ID.HasValue)
                    {
                        ListTreatment = ListTreatment.Where(o => o.END_DEPARTMENT_ID == castFilter.DEPARTMENT_ID.Value).ToList();
                        if (IsNotNullOrEmpty(ListTreatment))
                        {
                            department_name = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID==ListTreatment.First().END_DEPARTMENT_ID)??new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        }
                    }

                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListTreatment.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<HIS_TREATMENT> treatments = ListTreatment.Skip(start).Take(limit).ToList();

                        HisPatientTypeAlterFilterQuery PatientTypeAlterFilter = new HisPatientTypeAlterFilterQuery();
                        PatientTypeAlterFilter.TREATMENT_IDs = treatments.Select(s => s.ID).ToList();
                        var listPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).Get(PatientTypeAlterFilter)??new  List<HIS_PATIENT_TYPE_ALTER>();

                        ProcessCurrentListTreatment(paramGet, treatments, listPatientTypeAlter);

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

        private void ProcessCurrentListTreatment(CommonParam paramGet, List<HIS_TREATMENT> treatments, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            try
            {
                if (treatments != null)
                {
                    foreach (var treatment in treatments)
                    {
                        var deathCert = ListDeathBook.FirstOrDefault(o => o.ID == treatment.DEATH_CERT_BOOK_ID);
                        var deathCause = ListDeathCause.FirstOrDefault(o => o.ID == treatment.DEATH_CAUSE_ID);
                        var deathWithin = ListDeathWithin.FirstOrDefault(o => o.ID == treatment.DEATH_WITHIN_ID);
                        Mrs00045RDO rdo = new Mrs00045RDO(treatment);
                        rdo.STORE_CREATOR = treatment.CREATOR;
                        rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        rdo.PATIENT_DOB = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                        rdo.DEATH_TIMING = deathWithin != null && deathWithin.DEATH_WITHIN_NAME.Contains("giờ") ? deathWithin.DEATH_WITHIN_NAME : "";
                        rdo.DEATH_MAIN_CAUSE = deathWithin != null && !deathWithin.DEATH_WITHIN_NAME.Contains("giờ") ? deathWithin.DEATH_WITHIN_NAME : "";
                        rdo.DEATH_CERT_BOOK_NAME = deathCert != null ? deathCert.DEATH_CERT_BOOK_NAME : "";
                        rdo.DEATH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.DEATH_TIME ?? 0);
                        rdo.DEATH_CAUSE = deathCause != null ? deathCause.DEATH_CAUSE_NAME : "";
                        rdo.IS_EXAMINATION = treatment.IS_HAS_AUPOPSY == 1 ? "X" : "";

                        rdo.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        rdo.ICD_TEXT = treatment.ICD_TEXT;
                        rdo.ICD_NAME = treatment.ICD_NAME;
                        SetDateTreatMent(paramGet, rdo, treatment);
                        rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                        if (treatment.OUT_TIME.HasValue)
                        {
                            rdo.DATE_DEAD_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME.Value);
                        }
                        IsBhyt(listPatientTypeAlter, rdo, treatment);
                        CalcuatorAge(rdo, treatment);
                        SetJobForPatient(rdo, treatment);
                        // bổ sung các thông tin thời gian
                        rdo.IN_TIME = treatment.IN_TIME;
                        rdo.OUT_TIME = treatment.OUT_TIME;
                        rdo.DEATH_TIME = treatment.DEATH_TIME;
                        rdo.MAIN_CAUSE = treatment.MAIN_CAUSE;
                        rdo.END_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID==treatment.LAST_DEPARTMENT_ID)?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        ListRdo.Add(rdo);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void IsBhyt(List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter, Mrs00045RDO rdo, HIS_TREATMENT treatment)
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
                            if (treatment.TRANSFER_IN_ICD_NAME != null && treatment.TRANSFER_IN_ICD_NAME != "")
                            {
                                rdo.DIAGNOSE_TUYENDUOI = treatment.TRANSFER_IN_ICD_NAME;
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
                            rdo.ICD_CODE_KKB = treatment.ICD_CODE;
                        }
                        else
                        {
                            rdo.DIAGNOSE_KDT = treatment.ICD_NAME;
                            rdo.ICD_CODE_KDT = treatment.ICD_CODE;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetDateTreatMent(CommonParam paramGet, Mrs00045RDO rdo, HIS_TREATMENT treatment)
        {
            try
            {
                if (treatment.DEATH_WITHIN_ID == 1)
                {
                    rdo.IS_DEAD_IN_24H = "X";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool checkTreatmentInDepartment(CommonParam paramGet, HIS_TREATMENT treatment)
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

        private void CalcuatorAge(Mrs00045RDO rdo, HIS_TREATMENT treatment)
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

        private void SetJobForPatient(Mrs00045RDO rdo, HIS_TREATMENT treatment)
        {
            try
            {
                rdo.JOB = treatment.TDL_PATIENT_CAREER_NAME;
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

                objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(p => p.PATIENT_CODE).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
