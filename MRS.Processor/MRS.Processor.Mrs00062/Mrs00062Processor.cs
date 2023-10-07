using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Core.MrsReport.RDO; 
using MOS.MANAGER.HisDepartmentTran; 
using MOS.MANAGER.HisPatientTypeAlter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisReportTypeCat; 

namespace MRS.Processor.Mrs00062
{
    public class Mrs00062Processor : AbstractProcessor
    {
        Mrs00062Filter castFilter = null; 
        List<Mrs00062RDO> ListRdo = new List<Mrs00062RDO>(); 
        string DEPARTMENT_NAME; 
        Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_TREATMENT> ListTreatment;
        List<CATEGORY> listCategory = new List<CATEGORY>();
        List<EXAM_IN_TREAT> listExamInTreat = new List<EXAM_IN_TREAT>();
        List<HIS_DEPARTMENT_TRAN> listDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();

        public Mrs00062Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00062Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                castFilter = ((Mrs00062Filter)this.reportFilter); 
                CommonParam paramGet = new CommonParam(); 
                //if (castFilter.DEPARTMENT_ID > 0)
                //{
                Inventec.Common.Logging.LogSystem.Debug("Lay danh sach V_HIS_TREATMENT, MRS00062, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                HisTreatmentViewFilterQuery treatFilter = new HisTreatmentViewFilterQuery(); 
                treatFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                treatFilter.OUT_TIME_TO = castFilter.TIME_TO;
                treatFilter.IN_TIME_FROM = castFilter.IN_TIME_FROM;
                treatFilter.IN_TIME_TO = castFilter.IN_TIME_TO;
                treatFilter.IN_TIME_FROM = castFilter.DATE_OF_WEEK_FROM;
                treatFilter.IN_TIME_TO = castFilter.DATE_OF_WEEK_TO;
                if (castFilter.CHOOSE_RESULT == 0)
                {
                    treatFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                }
                if (castFilter.CHOOSE_RESULT == 1)
                {
                    treatFilter.IS_PAUSE = true;
                }
                ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatFilter);
                listPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).GetByTreatmentIds(ListTreatment.Select(o => o.ID).Distinct().ToList()).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).ToList();
                dicPatientTypeAlter = listPatientTypeAlter.GroupBy(q => q.TREATMENT_ID).ToDictionary(r => r.Key, r => r.Last());

                //Lay du lieu kham de kiem tra co xu tri nhap vien khong
                //if (castFilter.CHECK_EXAM_INTREAT == true)
                {
                    listExamInTreat = new ManagerSql().GetExamInTreat(castFilter);
                }
                //Lay du lieu chuyen khoa
                var listTreatmentId = ListTreatment.Select(p => p.ID).ToList();
                var skip = 0;
                while (listTreatmentId.Count - skip > 0)
                {
                    var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisDepartmentTranFilterQuery filterDpt = new HisDepartmentTranFilterQuery();
                    filterDpt.TREATMENT_IDs = listIDs;
                    var listDepartmentTranSub = new HisDepartmentTranManager(paramGet).Get(filterDpt);
                    if (IsNotNullOrEmpty(listDepartmentTranSub))
                    {
                        listDepartmentTran.AddRange(listDepartmentTranSub);
                    }
                }
                //Danh sach lam dich vu theo nhom dich vu
                listCategory = new ManagerSql().GetCategory(castFilter);
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieuj V_HIS_TREATMENT MRS00062."); 
                }
                else
                {

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
            bool result = true; 
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    ProcessListTreatment(ListTreatment); 
                    if (castFilter.DEPARTMENT_ID.HasValue)
                    {
                        GetDepartmentById(); 
                    }
                    if (castFilter.FIRST_DEPARTMENT_ID.HasValue || castFilter.MY_DEPARTMENT_ID.HasValue)
                    {
                        GetDepartmentById(); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessListTreatment(List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    if (castFilter.DEPARTMENT_ID.HasValue)
                    {
                        ListTreatment = ListTreatment.Where(o => o.LAST_DEPARTMENT_ID == castFilter.DEPARTMENT_ID.Value).ToList();
                    }
                    if (castFilter.FIRST_DEPARTMENT_ID.HasValue)
                    {
                        List<V_HIS_ROOM> listRoom = HisRoomCFG.HisRooms.Where(o => o.DEPARTMENT_ID == castFilter.FIRST_DEPARTMENT_ID).ToList();
                        ListTreatment = ListTreatment.Where(o => listRoom.Exists(p => p.ID == o.TDL_FIRST_EXAM_ROOM_ID)).ToList();
                    }
                    if (castFilter.MY_DEPARTMENT_ID.HasValue)
                    {
                       ListTreatment = ListTreatment.Where(o => o.LAST_DEPARTMENT_ID  == castFilter.MY_DEPARTMENT_ID).ToList();
                    }
                    if (castFilter.EXAM_ROOM_IDs !=null)
                    {
                        ListTreatment = ListTreatment.Where(o => castFilter.EXAM_ROOM_IDs.Exists(p => p == o.TDL_FIRST_EXAM_ROOM_ID)).ToList();
                    }
                    if (castFilter.TREATMENT_TYPE_IDs!=null)
                    {
                        ListTreatment = ListTreatment.Where(o => dicPatientTypeAlter.ContainsKey(o.ID) && castFilter.TREATMENT_TYPE_IDs.Contains(dicPatientTypeAlter[o.ID].TREATMENT_TYPE_ID)).ToList();
                    }
                    foreach (var treatment in ListTreatment)
                    {
                        Mrs00062RDO rdo = new Mrs00062RDO(); 
                        rdo.TREATMENT_CODE = treatment.TREATMENT_CODE; 
                        rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                        rdo.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        rdo.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        rdo.TDL_HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;

                        rdo.IS_EXAM_IN_TREAT = listExamInTreat != null && listExamInTreat.Exists(o => o.TREATMENT_ID == treatment.ID) ? (short)IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE : (short)IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE; 
                        rdo.TDL_TREATMENT_TYPE_ID = treatment.TDL_TREATMENT_TYPE_ID; 
                        rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                        rdo.DATE_CLINICAL_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.CLINICAL_IN_TIME ?? 0);

                        //xu ly lay khoa tiep nhan benh nhan nhap vien dieu tri
                        if (rdo.IS_EXAM_IN_TREAT == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                        {
                            var paty = listPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == treatment.ID && o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
                            if (paty != null)
                            {
                                var dpt = listDepartmentTran.FirstOrDefault(o => o.TREATMENT_ID == treatment.ID && o.ID == paty.DEPARTMENT_TRAN_ID);
                                if (dpt != null)
                                {
                                    rdo.CLINICAL_IN_DEPARTMENT_ID = dpt.DEPARTMENT_ID;
                                }
                            }
                            if (rdo.CLINICAL_IN_DEPARTMENT_ID == null)
                            {
                                var dpt = listDepartmentTran.FirstOrDefault(o => o.TREATMENT_ID == treatment.ID && o.DEPARTMENT_IN_TIME == null);
                                if (dpt != null)
                                {
                                    rdo.CLINICAL_IN_DEPARTMENT_ID = dpt.DEPARTMENT_ID;
                                }
                            }
                            var departmentCI = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.CLINICAL_IN_DEPARTMENT_ID);
                            if (departmentCI != null)
                            {
                                rdo.CLINICAL_IN_DEPARTMENT_CODE = departmentCI.DEPARTMENT_CODE;
                                rdo.CLINICAL_IN_DEPARTMENT_NAME = departmentCI.DEPARTMENT_NAME;
                            }
                        }

                        var department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.IN_DEPARTMENT_ID);
                        if (department != null)
                        {
                            rdo.IN_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                            rdo.IN_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                        }

                        var room = MANAGER.Config.HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatment.IN_ROOM_ID);
                        if (room != null)
                        {
                            rdo.IN_ROOM_CODE = room.ROOM_CODE;
                            rdo.IN_ROOM_NAME = room.ROOM_NAME;
                        }
                        ProcessIsBhyt(paramGet, treatment, rdo);

                        if (treatment.TRANSFER_IN_ICD_NAME != null && treatment.TRANSFER_IN_ICD_NAME != "")
                        {
                            rdo.DIAGNOSE_TUYENDUOI = treatment.TRANSFER_IN_ICD_NAME;
                        }
                        else
                        {
                            rdo.DIAGNOSE_TUYENDUOI = treatment.TRANSFER_IN_ICD_NAME;
                        }

                        rdo.ICD_CODE_TUYENDUOI = treatment.TRANSFER_IN_ICD_CODE;


                        rdo.GIOITHIEU = treatment.MEDI_ORG_NAME;


                        rdo.TRANSFER_IN_MEDI_ORG_NAME = treatment.TRANSFER_IN_MEDI_ORG_NAME;

                        rdo.ICD_NAME = treatment.ICD_NAME;

                        if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)// Config.IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                        {
                            rdo.DIAGNOSE_KKB = treatment.ICD_NAME;
                        }
                        else
                        {
                            rdo.DIAGNOSE_KDT = treatment.ICD_NAME;
                        }
                        rdo.TDL_PATIENT_DOB = treatment.TDL_PATIENT_DOB;
                        rdo.TDL_PATIENT_NATIONAL_NAME = treatment.TDL_PATIENT_NATIONAL_NAME;
                        CalcuatorAge(rdo, treatment); 
                        ListRdo.Add(rdo); 
                    }
                }
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00062."); 
                }
            }
            catch (Exception ex)
            {
                ListRdo.Clear(); 
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessIsBhyt(CommonParam paramGet, V_HIS_TREATMENT treatment, Mrs00062RDO rdo)
        {
            try
            {
                var currentPatientTypeAlter = dicPatientTypeAlter.ContainsKey(treatment.ID) ? dicPatientTypeAlter[treatment.ID] : new HIS_PATIENT_TYPE_ALTER(); 
                if (IsNotNull(currentPatientTypeAlter))
                {
                    if (currentPatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        rdo.IS_BHYT = "X"; 
                        ProcessHeinCardNumber(paramGet, rdo, currentPatientTypeAlter);

                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessHeinCardNumber(CommonParam paramGet, Mrs00062RDO rdo, HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            try
            {

                if (PatientTypeAlter != null)
                {
                    rdo.HEIN_CARD_NUMBER = PatientTypeAlter.HEIN_CARD_NUMBER; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void GetDepartmentById()
        {
            try
            {
                var department = new MOS.MANAGER.HisDepartment.HisDepartmentManager().GetById(castFilter.DEPARTMENT_ID ?? castFilter.FIRST_DEPARTMENT_ID ?? castFilter.MY_DEPARTMENT_ID ?? 0); 
                if (IsNotNull(department))
                {
                    DEPARTMENT_NAME = department.DEPARTMENT_NAME; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void CalcuatorAge(Mrs00062RDO rdo, V_HIS_TREATMENT treatment)
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

                dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0) ?? "" + (Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IN_TIME_FROM ?? 0) ?? "") + (Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.DATE_OF_WEEK_FROM ?? 0) ?? ""));

                dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0) ?? "" + (Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IN_TIME_TO ?? 0) ?? "") + (Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.DATE_OF_WEEK_TO ?? 0) ?? ""));

                dicSingleTag.Add("DEPARTMENT_NAME", DEPARTMENT_NAME);
                dicSingleTag.Add("DIC_CATEGORY_AMOUNT", listCategory.GroupBy(g => g.CATEGORY_CODE).ToDictionary(g => g.Key, p => p.Sum(s => s.AMOUNT)));
                HisReportTypeCatFilterQuery HisReportTypeCatfilter = new HisReportTypeCatFilterQuery();
                HisReportTypeCatfilter.REPORT_TYPE_CODE__EXACT = this.ReportTypeCode;
                var listHisReportTypeCat = new HisReportTypeCatManager().Get(HisReportTypeCatfilter)??new List<HIS_REPORT_TYPE_CAT>();
                dicSingleTag.Add("DIC_CATEGORY_NAME", listHisReportTypeCat.GroupBy(g => g.CATEGORY_CODE).ToDictionary(g => g.Key, p => p.First().CATEGORY_NAME));

                ListRdo = ListRdo.OrderBy(o => o.TREATMENT_CODE).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "Department", ListRdo.Where(q=>q.CLINICAL_IN_DEPARTMENT_ID.HasValue).GroupBy(o=>o.CLINICAL_IN_DEPARTMENT_ID).Select(p=>p.First()).ToList()); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
