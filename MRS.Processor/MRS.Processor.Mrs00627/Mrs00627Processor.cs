using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisPtttMethod;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTreatmentResult;

namespace MRS.Processor.Mrs00627
{
    public class Mrs00627Processor : AbstractProcessor
    {
        Mrs00627Filter filter = null;
        List<HIS_TREATMENT_END_TYPE> listHisTreatmentEndType = new List<HIS_TREATMENT_END_TYPE>();
        List<HIS_TREATMENT_RESULT> listHisTreatmentResult = new List<HIS_TREATMENT_RESULT>();
        //List<D_HIS_SERE_SERV_PTTT> listHisSereServPttt = new List<D_HIS_SERE_SERV_PTTT>();
        List<D_HIS_SERE_SERV> listHisSereServ = new List<D_HIS_SERE_SERV>();
        //List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();

        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();

        List<Mrs00627RDO> listRdo = new List<Mrs00627RDO>();
        List<DepartmentInOut> DepartmentInOuts = new List<DepartmentInOut>();
        List<Client> Clients = new List<Client>();
        List<KskAmount> KskAmounts = new List<KskAmount>();
        List<AppointmentAmount> AppointmentAmounts = new List<AppointmentAmount>();
        List<PatientTypeAlter> PatientTypeAlters = new List<PatientTypeAlter>();


        //List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        string thisReportTypeCode = "";

        public Mrs00627Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00627Filter);
        }
        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00627Filter)this.reportFilter;
            try
            {
                HisServiceRetyCatViewFilterQuery HisServiceRetyCatfilter = new HisServiceRetyCatViewFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = "MRS00627"
                };
                listHisServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatfilter);
                HisTreatmentEndTypeFilterQuery HisTreatmentEndTypefilter = new HisTreatmentEndTypeFilterQuery()
                {
                    IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                };
                listHisTreatmentEndType = new HisTreatmentEndTypeManager().Get(HisTreatmentEndTypefilter);
                HisTreatmentResultFilterQuery HisTreatmentResultfilter = new HisTreatmentResultFilterQuery()
                {
                    IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                };
                listHisTreatmentResult = new HisTreatmentResultManager().Get(HisTreatmentResultfilter);

                listHisSereServ = new ManagerSql().GetSS(filter) ?? new List<D_HIS_SERE_SERV>();

                DepartmentInOuts = new ManagerSql().GetDepartmentTran(filter);
                Clients = new ManagerSql().GetTreatment(filter);
                KskAmounts = new ManagerSql().GetKsk(filter);
                AppointmentAmounts = new ManagerSql().GetAppointment(filter);
                PatientTypeAlters = new ManagerSql().GetPatientTypeAlter(filter);

                listHisService = new HisServiceManager().Get(new HisServiceFilterQuery() { SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT });
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }


        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                DateTime Itime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.TIME_FROM ?? 0);
                while (Itime < (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.TIME_TO ?? 0))
                {
                    DateTime IDate = new System.DateTime(Itime.Year, Itime.Month, Itime.Day);
                    Mrs00627RDO rdo = new Mrs00627RDO();
                    rdo.DATE = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(IDate) ?? 0;

                    var sereServSub = listHisSereServ.Where(o => o.TDL_INTRUCTION_DATE == rdo.DATE).ToList();

                    rdo.DIC_CATEGORY = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, o.TDL_REQUEST_DEPARTMENT_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DIC_CATEGORY_COUNT = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, o.TDL_REQUEST_DEPARTMENT_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_EMEGENCY = sereServSub.Where(r => r.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && r.IS_EMERGENCY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).GroupBy(o => DepartmentCode(o.TDL_REQUEST_DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DIC_UNEMEGENCY = sereServSub.Where(r => r.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && r.IS_EMERGENCY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).GroupBy(o => DepartmentCode(o.TDL_REQUEST_DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DIC_EMOTIONLESS_METHOD = sereServSub.Where(r => r.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).GroupBy(o => DepartmentEmotionlessMethodCode(o.TDL_REQUEST_DEPARTMENT_ID, o.EMOTIONLESS_METHOD_ID ?? 0)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DIC_EXAM_WAIT = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL).GroupBy(o => ExecuteRoomCode(o.TDL_EXECUTE_ROOM_ID)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DIC_EXAM_DOING = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL).GroupBy(o => ExecuteRoomCode(o.TDL_EXECUTE_ROOM_ID)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DIC_SERVICE_TYPE_WAIT = sereServSub.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL).GroupBy(o => ServiceTypeCode(o.TDL_SERVICE_TYPE_ID, o.TDL_REQUEST_DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DIC_SERVICE = sereServSub.GroupBy(o => ServiceCode(o.TDL_SERVICE_CODE, o.TDL_REQUEST_DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DIC_CATEGORY_CLS = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DIC_SERVICE_ALL = sereServSub.GroupBy(o => ServiceCode(o.TDL_SERVICE_CODE)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DIC_CATEGORY_TREATMENT_TYPE_CLS = sereServSub.GroupBy(o => CategoryTreatmentTypeCode(o.SERVICE_ID,o.TDL_TREATMENT_TYPE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DIC_SERVICE_TREATMENT_TYPE_ALL = sereServSub.GroupBy(o => ServiceTreatmentTypeCode(o.TDL_SERVICE_CODE, o.TDL_TREATMENT_TYPE_ID)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DIC_SURG_GROUP = sereServSub.Where(r => r.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).GroupBy(o => DepartmentPtttGroupCode(o.TDL_REQUEST_DEPARTMENT_ID, o.SERVICE_ID)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DIC_EXP_DIE = DepartmentInOuts.Where(o => o.OUT_DATE ==rdo.DATE && o.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET && o.NEXT_ID == null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_EXP = DepartmentInOuts.Where(o => o.OUT_DATE == rdo.DATE && o.NEXT_ID == null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_EXP_END_TYPE = DepartmentInOuts.Where(o => o.OUT_DATE == rdo.DATE && o.NEXT_ID == null).GroupBy(r => DepartmentEndTypeCode(r.DEPARTMENT_ID, (r.TREATMENT_END_TYPE_ID ?? 0))).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_EXP_RESULT_END_TYPE = DepartmentInOuts.Where(o => o.OUT_DATE == rdo.DATE && o.NEXT_ID == null).GroupBy(r => DepartmentTreatmentResultCodeTreatmentEndTypeCode(r.DEPARTMENT_ID, (r.TREATMENT_RESULT_ID ?? 0), (r.TREATMENT_END_TYPE_ID ?? 0))).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_EXP_OUT = DepartmentInOuts.Where(o => o.OUT_DATE == rdo.DATE && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN && o.NEXT_ID == null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_EXP_TRAN = DepartmentInOuts.Where(o => o.OUT_DATE == rdo.DATE && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.NEXT_ID == null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_EXP_EXAM_TRAN = DepartmentInOuts.Where(o =>o.TDL_TREATMENT_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM&& o.OUT_DATE == rdo.DATE && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.NEXT_ID == null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    //Inventec.Common.Logging.LogSystem.Info("rdo.DIC_EXP_TRAN" + string.Join(",", rdo.DIC_EXP_TRAN.Keys));
                    rdo.DIC_IMP = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_CHMS = DepartmentInOuts.Where(o => (long)(o.NEXT_DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000 && o.NEXT_ID != null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_CLINICAL_IN = DepartmentInOuts.Where(o => (long)(o.NEXT_DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000 && o.NEXT_ID != null && o.CLINICAL_IN_TIME != null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.COUNT_EXAM_CLINICAL_IN = DepartmentInOuts.Where(o => IsFromExam(o,PatientTypeAlters) && o.DEPARTMENT_IN_TIME < rdo.DATE + 235959 && (o.NEXT_DEPARTMENT_IN_TIME >= rdo.DATE || (o.NEXT_ID == null && (o.IS_PAUSE == null || (o.IS_PAUSE == 1 && o.OUT_TIME >= rdo.DATE))))).Select(s => s.TREATMENT_ID).Distinct().Count();


                    rdo.DIC_BEGIN = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 < (long)rdo.DATE / 1000000 && (o.NEXT_DEPARTMENT_IN_TIME >= rdo.DATE || (o.NEXT_DEPARTMENT_IN_TIME == null && (o.OUT_TIME >= rdo.DATE || o.OUT_TIME == null)))).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_BEGIN_LESS15 = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 < (long)rdo.DATE / 1000000 && (o.NEXT_DEPARTMENT_IN_TIME >= rdo.DATE || (o.NEXT_DEPARTMENT_IN_TIME == null && (o.OUT_TIME >= rdo.DATE || o.OUT_TIME == null))) && Age(o.DEPARTMENT_IN_TIME ?? 0, o.TDL_PATIENT_DOB) <= 15).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_BEGIN_MORE15 = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 < (long)rdo.DATE / 1000000 && (o.NEXT_DEPARTMENT_IN_TIME >= rdo.DATE || (o.NEXT_DEPARTMENT_IN_TIME == null && (o.OUT_TIME >= rdo.DATE || o.OUT_TIME == null))) && Age(o.DEPARTMENT_IN_TIME ?? 0, o.TDL_PATIENT_DOB) > 15).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());
                    rdo.DIC_IMP_LESS15 = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000 && Age(o.DEPARTMENT_IN_TIME ?? 0, o.TDL_PATIENT_DOB) <= 15).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_IMP_MORE15 = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000 && Age(o.DEPARTMENT_IN_TIME ?? 0, o.TDL_PATIENT_DOB) > 15).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    Inventec.Common.Logging.LogSystem.Info("rdo.DIC_BEGIN" + string.Join(",", rdo.DIC_BEGIN.Keys));
                    rdo.DIC_ICD = DepartmentInOuts.Where(o => o.DEPARTMENT_IN_TIME < rdo.DATE + 235959 && (o.NEXT_DEPARTMENT_IN_TIME >= rdo.DATE || (o.NEXT_ID == null && (o.IS_PAUSE == null || (o.IS_PAUSE == 1 && o.OUT_TIME >= rdo.DATE))))).GroupBy(r => IcdCode(r.TREATMENT_ICD_CODE, r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_TREATMENT_ICD = DepartmentInOuts.Where(o => o.DEPARTMENT_IN_TIME < rdo.DATE + 235959 && (o.NEXT_DEPARTMENT_IN_TIME >= rdo.DATE || (o.NEXT_ID == null && (o.IS_PAUSE == null || (o.IS_PAUSE == 1 && o.OUT_TIME >= rdo.DATE))))).GroupBy(r => IcdCode(r.TREATMENT_ICD_CODE)).ToDictionary(p => p.Key, q => q.Select(s => s.TREATMENT_ID).Distinct().Count());

                    rdo.DIC_TREATMENT_EXAM_ICD = DepartmentInOuts.Where(o => o.TDL_TREATMENT_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM&&o.DEPARTMENT_IN_TIME < rdo.DATE + 235959 && (o.NEXT_DEPARTMENT_IN_TIME >= rdo.DATE || (o.NEXT_ID == null && (o.IS_PAUSE == null || (o.IS_PAUSE == 1 && o.OUT_TIME >= rdo.DATE))))).GroupBy(r => IcdCode(r.TREATMENT_ICD_CODE)).ToDictionary(p => p.Key, q => q.Select(s => s.TREATMENT_ID).Distinct().Count());

                    rdo.OLD_PATIENT = Clients.Where(o => o.IN_DATE == rdo.DATE && o.IS_USED_TO_EXAM == 1).Count();
                    rdo.NEW_PATIENT = Clients.Where(o => o.IN_DATE == rdo.DATE && o.IS_USED_TO_EXAM == null).Count();
                    rdo.SUBCLINICAL_EXTRA = Clients.Where(o => o.IN_DATE == rdo.DATE && o.IS_SUBCLINICAL_EXTRA == 1).Count();
                    rdo.DIC_CLIENT_ICD = Clients.Where(o => o.IN_DATE == rdo.DATE).GroupBy(r => IcdCode(r.ICD_CODE)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_PATIENT_TYPE = Clients.Where(o => o.IN_DATE == rdo.DATE).GroupBy(r => PatientTypeCode(r.TDL_PATIENT_TYPE_ID ?? 0)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_TREATMENT_PATIENT_TYPE = Clients.Where(o => o.IN_DATE == rdo.DATE).GroupBy(r => TreatmentPatientTypeCode((r.TDL_TREATMENT_TYPE_ID ?? 0), (r.TDL_PATIENT_TYPE_ID ?? 0))).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_KSK = KskAmounts.Where(o => o.IN_DATE == rdo.DATE).GroupBy(r => r.CODE??"").ToDictionary(p => p.Key, q => q.Sum(s=>s.COUNT));

                    rdo.COUNT_APPOINTMENT_TRUE = (AppointmentAmounts.FirstOrDefault(o => o.IN_DATE == rdo.DATE)??new AppointmentAmount()).COUNT;

                    listRdo.Add(rdo);
                    Itime = Itime.AddDays(1);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool IsFromExam(DepartmentInOut departmentTran, List<PatientTypeAlter> PatientTypeAlters)
        {
            bool result = false;
            try
            {
                var curentPatientTypeAlter = PatientTypeAlters.OrderByDescending(p=>p.LOG_TIME).ThenByDescending(q=>q.ID).FirstOrDefault(o=>o.TREATMENT_ID==departmentTran.TREATMENT_ID&&(departmentTran.NEXT_DEPARTMENT_IN_TIME==null||o.LOG_TIME<departmentTran.NEXT_DEPARTMENT_IN_TIME));
                if(curentPatientTypeAlter!=null)
                {
                    var previousPatientTypeAlter = PatientTypeAlters.OrderByDescending(p => p.LOG_TIME).ThenByDescending(q => q.ID).FirstOrDefault(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID && o.LOG_TIME < curentPatientTypeAlter.LOG_TIME);
                    if (previousPatientTypeAlter != null)
                    {
                        result = curentPatientTypeAlter.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && previousPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM;
                    }
                }
              
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                result = false;
            }
            return result;
        }

        private string ExecuteRoomCode(long TdlExecuteRoomId)
        {
            string result = "";
            try
            {
                result = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == TdlExecuteRoomId) ?? new V_HIS_ROOM()).ROOM_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                result = "";
            }
            return result;
        }

        private string DepartmentTreatmentResultCodeTreatmentEndTypeCode(long DepartmentId, long TreatmentResultId, long TreatmentEndTypeId)
        {
            string result = "";
            try
            {
                result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == DepartmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                     + "_" + ((listHisTreatmentResult.FirstOrDefault(o => o.ID == TreatmentResultId) ?? new HIS_TREATMENT_RESULT()).TREATMENT_RESULT_CODE ?? "")
                     + "_" + ((listHisTreatmentEndType.FirstOrDefault(o => o.ID == TreatmentEndTypeId) ?? new HIS_TREATMENT_END_TYPE()).TREATMENT_END_TYPE_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }


        private string TreatmentPatientTypeCode(long TreatmentTypeId,long PatientTypeId)
        {
            string result = "";
            try
            {
                result = ((HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == TreatmentTypeId) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_CODE ?? "") + "_" + ((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == PatientTypeId) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }
        private string PatientTypeCode(long PatientTypeId)
        {
            string result = "";
            try
            {
                result = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == PatientTypeId) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }
        private string DepartmentCode(long departmentId)
        {
            string result = "";
            try
            {
                result = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string DepartmentEndTypeCode(long departmentId,long endTypeId)
        {
            string result = "";
            try
            {
                result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                     + "_" + ((listHisTreatmentEndType.FirstOrDefault(o=>o.ID==endTypeId)?? new HIS_TREATMENT_END_TYPE()).TREATMENT_END_TYPE_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }


        private string DepartmentEmotionlessMethodCode(long departmentId, long EMOTIONLESS_METHOD_ID)
        {
            string result = "";
            try
            {
                
                    result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                         + "_" + ((HisEmotionlessMethodCFG.EMOTIONLESS_METHODs.FirstOrDefault(o => o.ID == EMOTIONLESS_METHOD_ID) ?? new HIS_EMOTIONLESS_METHOD()).EMOTIONLESS_METHOD_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string DepartmentPtttGroupCode(long departmentId, long serviceId)
        {
            string result = "";
            try
            {
                var service = listHisService.FirstOrDefault(o => o.ID == serviceId) ?? new HIS_SERVICE();
                if (service != null)
                {
                    result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                         + "_" + ((HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == service.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_CODE ?? "");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string IcdCode(string IcdCode)
        {
            string result = "";
            try
            {
                result = (IcdCode ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }
        private string IcdCode(string IcdCode,long departmentId)
        {
            string result = "";
            try
            {
                result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE??"")
                     + "_" + (IcdCode??"");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }
        private string ServiceTypeCode(long serviceTypeId, long departmentId)
        {
            try
            {
                return ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                    + "_" + ((HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == serviceTypeId) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string CategoryCode(long serviceId, long departmentId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                    + "_" + ((listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string CategoryCode(long serviceId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return ((listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string ServiceCode(string serviceCode, long departmentId)
        {
            try
            {
                return ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                    + "_" + (serviceCode ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string CategoryTreatmentTypeCode(long serviceId, long treatmentTypeId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return ((HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == treatmentTypeId) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_CODE ?? "")
                    + "_" + ((listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string ServiceTreatmentTypeCode(string serviceCode, long treatmentTypeId)
        {
            try
            {
                return ((HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == treatmentTypeId) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_CODE ?? "")
                    + "_" + (serviceCode ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }
        private string ServiceCode(string serviceCode)
        {
            try
            {
                return (serviceCode ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO ?? 0));

            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.SetUserFunction(store, "SumKeys", new RDOSumKeys());
            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }

        public object HisTreatmentResultfilter { get; set; }
    }
}
