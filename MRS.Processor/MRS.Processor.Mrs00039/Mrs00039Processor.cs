using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisEkipUser;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPtttMethod;
using MOS.MANAGER.HisExecuteRole;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisExpMestMedicine;
using FlexCel.Report;
using MOS.MANAGER.HisExpMest;

namespace MRS.Processor.Mrs00039
{
    public class Mrs00039Processor : AbstractProcessor
    {
        Mrs00039Filter castFilter = null;
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<Mrs00039RDO> ListRdo = new List<Mrs00039RDO>();
        List<Mrs00039RDO> ListRdoPPVC = new List<Mrs00039RDO>();
        List<Mrs00039RDO> ListRdoVC = new List<Mrs00039RDO>();
        List<Mrs00039RDO> ListRdoDT = new List<Mrs00039RDO>();
        List<Mrs00039RDO> ListRdoPTTTType = new List<Mrs00039RDO>();
        List<Mrs00039RDO> ListRdoPPVC1 = new List<Mrs00039RDO>();
        List<Mrs00039RDO> ListRdoPPPT = new List<Mrs00039RDO>();
        List<Mrs00039RDO> ListDepa = new List<Mrs00039RDO>();
        List<HIS_EXP_MEST> ListExpMest = new List<HIS_EXP_MEST>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();
        List<HIS_SERE_SERV_PTTT> listHisSereServPttt = new List<HIS_SERE_SERV_PTTT>();
        List<HIS_EKIP_USER> listHisEkipUser = new List<HIS_EKIP_USER>();
        List<HIS_EXECUTE_ROLE> listHisExecuteRole = new List<HIS_EXECUTE_ROLE>();
        List<HIS_PTTT_METHOD> listHisPtttMethod = new List<HIS_PTTT_METHOD>();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        //List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        //List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERE_SERV_EXT> listExt = new List<HIS_SERE_SERV_EXT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceRetyCat = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();

        Dictionary<long, List<HIS_SERVICE_MACHINE>> dicServiceMachine = new Dictionary<long, List<HIS_SERVICE_MACHINE>>();
        List<HIS_MACHINE> ListMachine = new List<HIS_MACHINE>();
        CommonParam paramGet = new CommonParam();

        public Mrs00039Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00039Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00039Filter)reportFilter);
            var result = true;
            try
            {
                //dich vụ phau thuat
                ListRdo = new ManagerSql().GetSereServ(castFilter);

                //loc theo thu
                DateTime? timeFrom = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM ?? castFilter.FINISH_TIME_FROM ?? 0);
                DateTime? timeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_TO ?? castFilter.FINISH_TIME_TO ?? 0);
                if (castFilter.INPUT_DATA_IDs != null)
                {
                    List<DayOfWeek> dayOfWeeks = castFilter.INPUT_DATA_IDs.Select(o => this.MapToDayOfWeek(o)).Where(p => p.HasValue).Select(q => q.Value).ToList();
                    if (timeFrom.HasValue && timeTo.HasValue)
                    {
                        List<long> dateTimes = this.getFromDayOfWeek(timeFrom.Value, timeTo.Value, dayOfWeeks);
                        if (castFilter.TIME_FROM != null)
                        {
                            ListRdo = ListRdo.Where(o => dateTimes.Contains(o.TDL_INTRUCTION_DATE)).ToList();
                        }
                        if (castFilter.FINISH_TIME_FROM != null)
                        {
                            ListRdo = ListRdo.Where(o => dateTimes.Exists(p => p <= o.TDL_FINISH_TIME && o.TDL_FINISH_TIME < p + 1000000)).ToList();
                        }
                    }
                }
                if (castFilter.BRANCH_ID != null)
                {
                    ListRdo = ListRdo.Where(o => HisDepartmentCFG.DEPARTMENTs.Exists(p => p.ID == o.TDL_REQUEST_DEPARTMENT_ID && p.BRANCH_ID == castFilter.BRANCH_ID)).ToList();
                }

                if (IsNotNullOrEmpty(castFilter.BRANCH_IDs))
                {
                    ListRdo = ListRdo.Where(o => HisDepartmentCFG.DEPARTMENTs.Exists(p => p.ID == o.TDL_REQUEST_DEPARTMENT_ID && castFilter.BRANCH_IDs.Contains(p.BRANCH_ID))).ToList();
                }

                if (castFilter.IS_NT_NGT.HasValue && IsNotNullOrEmpty(ListRdo))
                {
                    List<HIS_DEPARTMENT> departments = new List<HIS_DEPARTMENT>();
                    if (castFilter.IS_NT_NGT.Value == 0)
                    {
                        departments = HisDepartmentCFG.DEPARTMENTs.Where(o => o.REQ_SURG_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                    }
                    else if (castFilter.IS_NT_NGT.Value == 1)
                    {
                        departments = HisDepartmentCFG.DEPARTMENTs.Where(o => o.REQ_SURG_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }

                    if (IsNotNullOrEmpty(departments))
                    {
                        ListRdo = ListRdo.Where(o => departments.Select(s => s.ID).Contains(o.TDL_REQUEST_DEPARTMENT_ID)).ToList();
                    }
                }
                //DV - thanh toan
                var skip = 0;
                if (IsNotNullOrEmpty(ListRdo))
                {
                    var sereServIds = ListRdo.Select(o => o.ID).ToList();
                    if (IsNotNullOrEmpty(sereServIds))
                    {
                        skip = 0;
                        while (sereServIds.Count - skip > 0)
                        {
                            var listIDs = sereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServPtttFilterQuery HisSereServPtttfilter = new HisSereServPtttFilterQuery();
                            HisSereServPtttfilter.SERE_SERV_IDs = listIDs;
                            var listHisSereServPtttSub = new HisSereServPtttManager(paramGet).Get(HisSereServPtttfilter);
                            if (listHisSereServPtttSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listHisSereServPtttSub" + listHisSereServPtttSub.Count);
                            else
                            {
                                listHisSereServPttt.AddRange(listHisSereServPtttSub);
                            }

                            if (castFilter.HAS_BILL == true || castFilter.HAS_BILL_OR_BHYT != null)
                            {
                                HisSereServBillFilterQuery filterSereServBill = new HisSereServBillFilterQuery();
                                filterSereServBill.SERE_SERV_IDs = listIDs;
                                filterSereServBill.IS_NOT_CANCEL = true;
                                var listSereServBillSub = new HisSereServBillManager(paramGet).Get(filterSereServBill);
                                if (IsNotNull(listSereServBillSub))
                                {
                                    ListSereServBill.AddRange(listSereServBillSub);
                                }
                            }
                            HisSereServExtFilterQuery sereServExt = new HisSereServExtFilterQuery();
                            sereServExt.SERE_SERV_IDs = listIDs;

                            if ((castFilter.IS_NOT_FEE.HasValue || castFilter.IS_NOT_GATHER_DATA.HasValue) && IsNotNullOrEmpty(ListRdo))
                            {
                                if (castFilter.IS_NOT_FEE.HasValue) sereServExt.IS_NOT_FEE = castFilter.IS_NOT_FEE == 1;
                                if (castFilter.IS_NOT_GATHER_DATA.HasValue) sereServExt.IS_NOT_GATHER_DATA = castFilter.IS_NOT_GATHER_DATA == 1;

                            }
                            var listSsExt = new HisSereServExtManager(paramGet).Get(sereServExt);
                            if (listSsExt != null)
                            {
                                listExt.AddRange(listSsExt);
                            }
                        }
                    }


                }
                if (castFilter.HAS_BILL ?? false)
                {
                    ListRdo = ListRdo.Where(o => ListSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).ToList();
                }

                if ((castFilter.IS_NOT_FEE.HasValue || castFilter.IS_NOT_GATHER_DATA.HasValue) && IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.Where(o => listExt.Select(s => s.SERE_SERV_ID).Contains(o.ID)).ToList();
                }

                if (castFilter.HAS_BILL_OR_BHYT != null && IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.Where(o => (ListSereServBill.Exists(p => p.SERE_SERV_ID == o.ID) || o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT) == castFilter.HAS_BILL_OR_BHYT).ToList();
                }



                var PtttMethodIds = listHisSereServPttt.Select(o => o.PTTT_METHOD_ID ?? 0).Distinct().ToList();
                PtttMethodIds = PtttMethodIds.Distinct().ToList();
                if (IsNotNullOrEmpty(PtttMethodIds))
                {
                    skip = 0;
                    while (PtttMethodIds.Count - skip > 0)
                    {
                        var listIDs = PtttMethodIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPtttMethodFilterQuery HisPtttMethodfilter = new HisPtttMethodFilterQuery();
                        HisPtttMethodfilter.IDs = listIDs;
                        var listHisPtttMethodSub = new HisPtttMethodManager(paramGet).Get(HisPtttMethodfilter);
                        if (listHisPtttMethodSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisPtttMethodSub" + listHisPtttMethodSub.Count);
                        else
                        {
                            listHisPtttMethod.AddRange(listHisPtttMethodSub);
                        }
                    }
                }

                if (IsNotNullOrEmpty(ListRdo))
                {
                    var ekipIds = ListRdo.Select(o => o.EKIP_ID ?? 0).Distinct().ToList();
                    if (IsNotNullOrEmpty(ekipIds))
                    {
                        skip = 0;
                        while (ekipIds.Count - skip > 0)
                        {
                            var listIDs = ekipIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisEkipUserFilterQuery HisEkipUserfilter = new HisEkipUserFilterQuery();
                            HisEkipUserfilter.EKIP_IDs = listIDs;
                            HisEkipUserfilter.ORDER_FIELD = "ID";
                            HisEkipUserfilter.ORDER_DIRECTION = "ASC";
                            var listHisEkipUserSub = new HisEkipUserManager(paramGet).Get(HisEkipUserfilter);
                            if (listHisEkipUserSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listHisEkipUserSub" + listHisEkipUserSub.Count);
                            else
                            {
                                listHisEkipUser.AddRange(listHisEkipUserSub);
                            }


                        }
                    }

                    var treatmentIds = ListRdo.Select(p => p.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                    if (IsNotNullOrEmpty(treatmentIds))
                    {
                        skip = 0;
                        while (treatmentIds.Count - skip > 0)
                        {
                            var listIDs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                            treatmentFilter.IDs = listIDs;

                            var listHisTreatmentSub = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                            if (listHisTreatmentSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listHisTreatmentSub" + listHisTreatmentSub.Count);
                            else
                            {
                                listTreatment.AddRange(listHisTreatmentSub);
                            }
                        }
                    }

                    var serviceReqIds = ListRdo.Select(p => p.SERVICE_REQ_ID ?? 0).Distinct().ToList();
                    //if (IsNotNullOrEmpty(serviceReqIds))
                    //{
                    //    skip = 0;
                    //    while (serviceReqIds.Count - skip > 0)
                    //    {
                    //        var listIDs = serviceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    //        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    //        HisExpMestMedicineViewFilterQuery expFilter = new HisExpMestMedicineViewFilterQuery();
                    //        expFilter.TDL_SERVICE_REQ_IDs = listIDs;

                    //        var listExpMedicineSub = new HisExpMestMedicineManager(paramGet).GetView(expFilter);
                    //        if (listExpMedicineSub == null)
                    //            Inventec.Common.Logging.LogSystem.Error("listExpMedicineSub" + listExpMedicineSub.Count);
                    //        else
                    //        {
                    //            listExpMestMedicine.AddRange(listExpMedicineSub);
                    //        }
                    //    }
                    //}
                    if (IsNotNullOrEmpty(treatmentIds))
                    {
                        skip = 0;
                        while (treatmentIds.Count - skip > 0)
                        {
                            var listIDs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisExpMestFilterQuery expFilter = new HisExpMestFilterQuery();
                            expFilter.TDL_TREATMENT_IDs = listIDs;

                            var listExpSub = new HisExpMestManager(paramGet).Get(expFilter);
                            if (listExpSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listExpSub" + listExpSub.Count);
                            else
                            {
                                ListExpMest.AddRange(listExpSub);
                            }
                        }
                    }
                    var expMestIds = ListExpMest.Select(x => x.ID).Distinct().ToList();
                    if (IsNotNullOrEmpty(expMestIds))
                    {
                        skip = 0;
                        while (expMestIds.Count - skip > 0)
                        {
                            var Ids = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisExpMestMedicineViewFilterQuery expFilter = new HisExpMestMedicineViewFilterQuery();
                            expFilter.EXP_MEST_IDs = Ids;

                            var listExpMedicineSub = new HisExpMestMedicineManager(paramGet).GetView(expFilter);
                            if (listExpMedicineSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listExpMedicineSub" + listExpMedicineSub.Count);
                            else
                            {
                                listExpMestMedicine.AddRange(listExpMedicineSub);
                            }
                        }
                    }


                    var executeRoleIds = listHisEkipUser.Select(o => o.EXECUTE_ROLE_ID).Distinct().ToList();
                    if (IsNotNullOrEmpty(executeRoleIds))
                    {
                        skip = 0;
                        while (executeRoleIds.Count - skip > 0)
                        {
                            var listIDs = executeRoleIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisExecuteRoleFilterQuery HisExecuteRolefilter = new HisExecuteRoleFilterQuery();
                            HisExecuteRolefilter.IDs = listIDs;
                            var listHisExecuteRoleSub = new HisExecuteRoleManager(paramGet).Get(HisExecuteRolefilter);
                            if (listHisExecuteRoleSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listHisExecuteRoleSub" + listHisExecuteRoleSub.Count);
                            else
                            {
                                listHisExecuteRole.AddRange(listHisExecuteRoleSub);
                            }
                        }
                    }
                }
                //cac loai phau thuat
                List<long> PtttGroupIds = (HisPtttGroupCFG.PTTT_GROUPs ?? new List<HIS_PTTT_GROUP>()).Select(o => o.ID).ToList();
                //dich vu

                HisServiceFilterQuery listServiceFilter = new HisServiceFilterQuery();
                if (castFilter.REPORT_TYPE_CODE_CATEGORY_CODE == null)
                    listServiceFilter.PTTT_GROUP_IDs = PtttGroupIds;

                listHisService = new HisServiceManager(paramGet).Get(listServiceFilter);

                //danh sach dich vu - nhom bao cao
                GetServiceRetyCat();

                //get dịch vụ máy
                GetServiceMachine();

                //get máy
                GetMachine();

                //lọc theo máy
                FilterByMachine();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterByMachine()
        {
            if (castFilter.MACHINE_IDs != null)
            {
                var serviceIds = dicServiceMachine.Values.SelectMany(p => p.ToList()).Where(o => castFilter.MACHINE_IDs.Contains(o.MACHINE_ID)).Select(q => q.SERVICE_ID).Distinct().ToList();
                ListRdo = ListRdo.Where(o => serviceIds.Contains(o.SERVICE_ID)).ToList();
                var sereServIds = ListRdo.Select(o => o.ID).ToList();
                listExt = listExt.Where(o => sereServIds.Contains(o.SERE_SERV_ID)).ToList();
            }
            if (castFilter.EXECUTE_MACHINE_IDs != null)
            {
                listExt = listExt.Where(o => castFilter.EXECUTE_MACHINE_IDs.Contains(o.MACHINE_ID ?? 0)).ToList();
                var sereServIds = listExt.Select(o => o.SERE_SERV_ID).ToList();
                ListRdo = ListRdo.Where(o => sereServIds.Contains(o.ID)).ToList();
            }
        }

        private void GetMachine()
        {
            string query = "select * from his_machine where is_delete=0";
            ListMachine = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MACHINE>(query) ?? new List<HIS_MACHINE>();
        }

        private void GetServiceMachine()
        {
            string query = "select * from his_service_machine where is_delete=0";
            var ListServiceMachine = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE_MACHINE>(query);
            if (ListServiceMachine != null && ListServiceMachine.Count > 0)
            {
                dicServiceMachine = ListServiceMachine.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => q.ToList());
            }
        }

        private void GetServiceRetyCat()
        {
            HisServiceRetyCatViewFilterQuery serviceRetyCatfilter = new HisServiceRetyCatViewFilterQuery();
            serviceRetyCatfilter.REPORT_TYPE_CODE__EXACT = "MRS00660";
            var serviceRetyCats = new HisServiceRetyCatManager().GetView(serviceRetyCatfilter);
            dicServiceRetyCat = serviceRetyCats.GroupBy(o => o.SERVICE_ID).ToDictionary(o => o.Key, q => q.First());
        }

        private DayOfWeek? MapToDayOfWeek(long dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1: return DayOfWeek.Monday;
                case 2: return DayOfWeek.Tuesday;
                case 3: return DayOfWeek.Wednesday;
                case 4: return DayOfWeek.Thursday;
                case 5: return DayOfWeek.Friday;
                case 6: return DayOfWeek.Saturday;
                case 7: return DayOfWeek.Sunday;
                default:
                    return null;
            }
        }
        public List<long> getFromDayOfWeek(DateTime startDate, DateTime endDate, List<DayOfWeek> DayOfWeeks)
        {
            List<long> result = new List<long>();
            try
            {
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (DayOfWeeks.Contains(date.DayOfWeek))
                    {
                        long? longDate = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(date.Year, date.Month, date.Day));
                        if (longDate.HasValue)
                        {
                            result.Add(longDate.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;

        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                if (ListRdo != null && ListRdo.Count > 0)
                {
                    ProcessListSereServPttt();

                    ListRdo = ListRdo.OrderBy(o => o.PATIENT_CODE).ToList();

                    ListRdoPPVC = ListRdo.Where(p => !string.IsNullOrEmpty(p.SURG_PPVC) && p.SURG_PPVC.ToLower().Contains("tê")).ToList();
                    //ListRdoPPVC = ListRdo.Where(x => x.EMOTIONLESS_METHOD_ID != null).ToList();
                    LogSystem.Info("countppvc:" + ListRdoPPVC.Count());
                    LogSystem.Info("serviceReqId:" + string.Join(",", ListRdoPPVC.Select(p => p.SERVICE_REQ_ID)));
                    LogSystem.Info("countmedicine:" + listExpMestMedicine.Count());
                    ProcessListPPVC(ListRdoPPVC, listExpMestMedicine);
                    ProcessGroupPT(ListRdo);
                    //if (ListRdoPPVC != null && listExpMestMedicine != null)
                    //{
                    //    ProcessListPPVC(ListRdoPPVC, listExpMestMedicine);
                    //}


                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessListSereServPttt()
        {
            try
            {
                long PtttPriorityEmergency = HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__CC;
                long PtttPriorityProgram = HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__P;
                foreach (var rdo in ListRdo)
                {
                    //Mrs00039RDO rdo = new Mrs00039RDO(sereServ);
                    rdo.SERVICE_REQ_ID = rdo.SERVICE_REQ_ID;
                    rdo.PATIENT_NAME = rdo.PATIENT_NAME;
                    rdo.PATIENT_CODE = rdo.PATIENT_CODE;
                    rdo.TDL_REQUEST_DEPARTMENT_ID = rdo.TDL_REQUEST_DEPARTMENT_ID;
                    var treatment = listTreatment.FirstOrDefault(p => p.ID == rdo.TDL_TREATMENT_ID);
                    if (treatment != null)
                    {
                        rdo.FEE_LOCK_TIME = treatment.FEE_LOCK_TIME;
                        rdo.FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToMonthString(treatment.FEE_LOCK_TIME ?? 0);
                        rdo.IN_TIME = treatment.IN_TIME;
                        rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.IN_TIME);
                        rdo.OUT_TIME = treatment.OUT_TIME;
                        rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0);
                        rdo.HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                    }
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    if (patientType.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        rdo.PATIENT_TYPE_NAME_01 = "BHYT";
                    else if (patientType.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        rdo.PATIENT_TYPE_NAME_01 = "VP";
                    else
                        rdo.PATIENT_TYPE_NAME_01 = "XHH";
                    //if (patientType.ID != HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                    //    rdo.HEIN_CARD_NUMBER = rdo.HEIN_CARD_NUMBER;
                    rdo.MEDI_ORG_CODE = rdo.MEDI_ORG_CODE;
                    rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                    rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;

                    //đối tượng bệnh nhân
                    var tdlPatientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    rdo.TDL_PATIENT_TYPE_NAME = tdlPatientType.PATIENT_TYPE_NAME;
                    rdo.TDL_PATIENT_TYPE_CODE = tdlPatientType.PATIENT_TYPE_CODE;

                    rdo.REQUEST_USERNAME = rdo.TDL_REQUEST_USERNAME;
                    rdo.SERVICE_NAME = rdo.TDL_SERVICE_NAME;
                    rdo.SERVICE_CODE = rdo.TDL_SERVICE_CODE;
                    rdo.TIME_SURG_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.START_TIME ?? 0);
                    rdo.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    rdo.finish_Time = rdo.TDL_FINISH_TIME ?? 0;

                    rdo.FINISH_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.TDL_FINISH_TIME ?? 0);
                    CalcuatorAge(rdo);
                    IsBhyt(rdo);

                    var sereServPttt = listHisSereServPttt.FirstOrDefault(o => o.SERE_SERV_ID == rdo.ID) ?? new HIS_SERE_SERV_PTTT();
                    rdo.PTTT_METHOD_ID = sereServPttt.PTTT_METHOD_ID;
                    rdo.SURG_PPPT = (listHisPtttMethod.FirstOrDefault(o => o.ID == sereServPttt.PTTT_METHOD_ID) ?? new HIS_PTTT_METHOD()).PTTT_METHOD_NAME;
                    rdo.EMOTIONLESS_METHOD_ID = sereServPttt.EMOTIONLESS_METHOD_ID;
                    rdo.SURG_PPVC = (HisEmotionlessMethodCFG.EMOTIONLESS_METHODs.FirstOrDefault(o => o.ID == sereServPttt.EMOTIONLESS_METHOD_ID) ?? new HIS_EMOTIONLESS_METHOD()).EMOTIONLESS_METHOD_NAME;
                    rdo.ICD_CODE = rdo.ICD_CODE;
                    rdo.ICD_NAME = rdo.ICD_NAME;
                    rdo.BEFORE_SURG = sereServPttt.BEFORE_PTTT_ICD_NAME;
                    rdo.AFTER_SURG = sereServPttt.AFTER_PTTT_ICD_NAME;
                    rdo.SURG_TYPE_NAME = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == sereServPttt.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;

                    var serviceMachine = dicServiceMachine.ContainsKey(rdo.SERVICE_ID) ? dicServiceMachine[rdo.SERVICE_ID] : null;
                    if (serviceMachine != null && serviceMachine.Count > 0)
                    {
                        var machine = ListMachine.Where(p => serviceMachine.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                        if (machine.Count > 0)
                        {
                            rdo.MACHINE_NAME = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            rdo.MACHINE_CODE = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                    }

                    var service = listHisService.FirstOrDefault(o => o.ID == rdo.SERVICE_ID) ?? new HIS_SERVICE();
                    rdo.DEFAULT_SURG_TYPE_NAME = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == service.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                    rdo.PTTT_GROUP_ID = service.PTTT_GROUP_ID;
                    rdo.PTTT_GROUP_DB = (service.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4) ? "x" : "";
                    rdo.PTTT_GROUP_1 = (service.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1) ? "x" : "";
                    rdo.PTTT_GROUP_2 = (service.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2) ? "x" : "";
                    rdo.PTTT_GROUP_3 = (service.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3) ? "x" : "";
                    rdo.EKIP_ID = rdo.EKIP_ID;
                    var ekipUser = listHisEkipUser.Where(o => o.EKIP_ID == (rdo.EKIP_ID ?? 0)).ToList();
                    rdo.AMOUNT = rdo.AMOUNT;
                    rdo.DICR_EXECUTE_USERNAME = new Dictionary<string, string>();
                    if (IsNotNullOrEmpty(ekipUser))
                    {
                        var executeRole = listHisExecuteRole.Where(p => ekipUser.Exists(q => q.EXECUTE_ROLE_ID == p.ID)).ToList();
                        if (executeRole != null) { rdo.EXECUTE_ROLE_NAME = string.Join(",", executeRole.Select(p => p.EXECUTE_ROLE_NAME)); }
                        rdo.DICR_EXECUTE_USERNAME = ekipUser.GroupBy(o => listHisExecuteRole.FirstOrDefault(p => p.ID == o.EXECUTE_ROLE_ID).EXECUTE_ROLE_CODE).ToDictionary(q => q.Key, q => string.Join("\r\n", q.Select(o => o.USERNAME).ToList()));
                    }
                    rdo.SERVICE_AMOUNT = rdo.AMOUNT;
                    rdo.PTTT_PRIORITY_NAME = (HisPtttPriorityCFG.PTTT_PRIORITYs.FirstOrDefault(o => o.ID == sereServPttt.PTTT_PRIORITY_ID) ?? new HIS_PTTT_PRIORITY()).PTTT_PRIORITY_NAME;

                    rdo.PTTT_PRIORITY_CC = (sereServPttt.PTTT_PRIORITY_ID == HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__CC) ? "x" : "";
                    rdo.PTTT_PRIORITY_CT = (sereServPttt.PTTT_PRIORITY_ID == HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__P) ? "x" : "";

                    rdo.PTTT_TABLE_NAME = (HisPtttTableCFG.PTTT_TABLEs.FirstOrDefault(o => o.ID == sereServPttt.PTTT_TABLE_ID) ?? new HIS_PTTT_TABLE()).PTTT_TABLE_NAME;

                    rdo.EMOTIONLESS_RESULT_NAME = (HisEmotionlessResultCFG.EMOTIONLESS_RESULTs.FirstOrDefault(o => o.ID == sereServPttt.EMOTIONLESS_RESULT_ID) ?? new HIS_EMOTIONLESS_RESULT()).EMOTIONLESS_RESULT_NAME;

                    rdo.SURG_PPVC_2 = (HisEmotionlessMethodCFG.EMOTIONLESS_METHODs.FirstOrDefault(o => o.ID == sereServPttt.EMOTIONLESS_METHOD_SECOND_ID) ?? new HIS_EMOTIONLESS_METHOD()).EMOTIONLESS_METHOD_NAME;

                    rdo.TDL_EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    rdo.TDL_REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;

                    HIS_SERE_SERV_EXT ext = listExt != null ? listExt.FirstOrDefault(o => o.SERE_SERV_ID == rdo.ID) : null;
                    if (ext != null)
                    {
                        rdo.BEGIN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ext.BEGIN_TIME ?? 0);
                        rdo.BEGIN_TIME_NUM = ext.BEGIN_TIME ?? 0;
                        rdo.DESCRIPTION_SURGERY = ext.DESCRIPTION;
                        rdo.NOTE = ext.NOTE;
                        rdo.INTRUCTION_NOTE = ext.INSTRUCTION_NOTE;
                        var machineExt = ListMachine.Where(p => ext.MACHINE_ID == p.ID).ToList();
                        if (machineExt.Count > 0)
                        {
                            rdo.EXECUTE_MACHINE_NAME = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            rdo.EXECUTE_MACHINE_CODE = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                    }

                    if (ext != null && ext.END_TIME.HasValue)
                    {
                        rdo.END_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ext.END_TIME ?? 0);
                        rdo.END_TIME_NUM = ext.END_TIME ?? 0;
                    }

                    if (dicServiceRetyCat.ContainsKey(rdo.SERVICE_ID))
                    {
                        rdo.CATEGORY_CODE = dicServiceRetyCat[rdo.SERVICE_ID].CATEGORY_CODE;
                        rdo.CATEGORY_NAME = dicServiceRetyCat[rdo.SERVICE_ID].CATEGORY_NAME;
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessListPPVC(List<Mrs00039RDO> listPPVC, List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine)
        {
            try
            {
                if (listExpMestMedicine != null)
                {
                    foreach (var item in listExpMestMedicine)
                    {
                        var ppvc = listPPVC.FirstOrDefault(p => p.TDL_TREATMENT_ID==item.TDL_TREATMENT_ID);
                        Mrs00039RDO rdo = new Mrs00039RDO();
                        if (ppvc != null)
                        {
                            rdo.SERVICE_ID = ppvc.SERVICE_ID;
                            rdo.HEIN_SERVICE_BHYT_CODE = ppvc.TDL_HEIN_SERVICE_BHYT_CODE;
                            rdo.SERVICE_NAME = ppvc.SERVICE_NAME;
                            rdo.MEDI_ORG_CODE = ppvc.MEDI_ORG_CODE;
                        }
                        rdo.MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.MEDICINE_AMOUNT = item.AMOUNT;
                        rdo.MEDICINE_PRICE = item.PRICE;
                        rdo.MEDICINE_VAT_RATIO = item.VAT_RATIO;
                        ListRdoVC.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessGroupPT(List<Mrs00039RDO> ListRdo)
        {
            try
            {
                if (ListRdo != null)
                {
                    //doi tuong
                    ListRdoDT = ListRdo.GroupBy(p => new { p.TDL_PATIENT_TYPE_ID }).Select(q => new Mrs00039RDO
                    {
                        TDL_PATIENT_TYPE_ID = q.First().TDL_PATIENT_TYPE_ID,

                        PATIENT_TYPE_NAME = q.First().PATIENT_TYPE_NAME,

                        DIC_REQ_DEPARTMENT_AMOUNT = q.Where(o => !string.IsNullOrWhiteSpace(o.TDL_REQUEST_DEPARTMENT_NAME)).GroupBy(g => g.TDL_REQUEST_DEPARTMENT_NAME).ToDictionary(p => p.Key, y => y.Sum(p => p.SERVICE_AMOUNT)),
                    }).ToList();

                    //nhom pt
                    ListRdoPTTTType = ListRdo.Where(P => P.PTTT_GROUP_ID != null).GroupBy(p => new { p.PTTT_GROUP_ID }).Select(q => new Mrs00039RDO
                    {
                        PTTT_GROUP_ID = q.First().PTTT_GROUP_ID,
                        DEFAULT_SURG_TYPE_NAME = q.First().DEFAULT_SURG_TYPE_NAME,

                        DIC_REQ_DEPARTMENT_AMOUNT = q.Where(o => !string.IsNullOrWhiteSpace(o.TDL_REQUEST_DEPARTMENT_NAME)).GroupBy(g => g.TDL_REQUEST_DEPARTMENT_NAME).ToDictionary(p => p.Key, y => y.Sum(p => p.SERVICE_AMOUNT)),

                    }).ToList();

                    //ppvc
                    ListRdoPPVC1 = ListRdo.Where(P => P.EMOTIONLESS_METHOD_ID != null).GroupBy(p => new { p.EMOTIONLESS_METHOD_ID }).Select(q => new Mrs00039RDO
                    {
                        EMOTIONLESS_METHOD_ID = q.First().EMOTIONLESS_METHOD_ID,
                        SURG_PPVC = q.First().SURG_PPVC,

                        DIC_REQ_DEPARTMENT_AMOUNT = q.Where(o => !string.IsNullOrWhiteSpace(o.TDL_REQUEST_DEPARTMENT_NAME)).GroupBy(g => g.TDL_REQUEST_DEPARTMENT_NAME).ToDictionary(p => p.Key, y => y.Sum(p => p.SERVICE_AMOUNT)),

                    }).ToList();

                    //pppt
                    ListRdoPPPT = ListRdo.Where(P => P.PTTT_METHOD_ID != null).GroupBy(p => new { p.PTTT_METHOD_ID }).Select(q => new Mrs00039RDO
                    {
                        PTTT_METHOD_ID = q.First().PTTT_METHOD_ID,
                        SURG_PPPT = q.First().SURG_PPPT,

                        DIC_REQ_DEPARTMENT_AMOUNT = q.Where(o => !string.IsNullOrWhiteSpace(o.TDL_REQUEST_DEPARTMENT_NAME)).GroupBy(g => g.TDL_REQUEST_DEPARTMENT_NAME).ToDictionary(p => p.Key, y => y.Sum(p => p.SERVICE_AMOUNT)),

                    }).ToList();


                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        //private void ProcessListPTGM(List<Mrs00039RDO> listPTGM)
        //{
        //    try
        //    {


        //        if (listPTGM != null)
        //        {
        //            foreach (var item in listPTGM)
        //            {

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //    }
        //}

        private void CalcuatorAge(Mrs00039RDO rdo)
        {
            try
            {
                int? tuoi = RDOCommon.CalculateAge(rdo.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (rdo.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.MALE_YEAR = ProcessYearDob(rdo.TDL_PATIENT_DOB);
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.FEMALE_YEAR = ProcessYearDob(rdo.TDL_PATIENT_DOB);
                    }
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

        private void IsBhyt(Mrs00039RDO rdo)
        {
            try
            {

                if (rdo.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {

                    rdo.IS_BHYT = "X";

                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00039Filter)this.reportFilter).TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00039Filter)this.reportFilter).FINISH_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00039Filter)this.reportFilter).TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00039Filter)this.reportFilter).FINISH_TIME_TO ?? 0));
            for (int i = 0; i < listHisExecuteRole.Count; i++)
            {
                dicSingleTag.Add(string.Format("EXECUTE_ROLE_NAME_{0}", i + 1), listHisExecuteRole[i].EXECUTE_ROLE_NAME);
            }

            if (((Mrs00039Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00039Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }

            if (((Mrs00039Filter)this.reportFilter).REQUEST_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00039Filter)this.reportFilter).REQUEST_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }

            if (castFilter.IS_NT_NGT.HasValue)
            {
                if (castFilter.IS_NT_NGT.Value == 0)
                {
                    dicSingleTag.Add("TITLE_NT_NGT", "NGOẠI TRÚ");
                }
                else if (castFilter.IS_NT_NGT.Value == 1)
                {
                    dicSingleTag.Add("TITLE_NT_NGT", "NỘI TRÚ");
                }
            }

            if (castFilter.IS_NT_NGT_0.HasValue)
            {
                if (castFilter.IS_NT_NGT_0.Value == 0)
                {
                    dicSingleTag.Add("TITLE_NT_NGT_0", "NGOẠI TRÚ");
                }
                else if (castFilter.IS_NT_NGT_0.Value == 1)
                {
                    dicSingleTag.Add("TITLE_NT_NGT_0", "NỘI TRÚ");
                }
            }
            var ListPPVC =  ListRdoVC.Where(p => !string.IsNullOrEmpty(p.SERVICE_NAME)).OrderBy(p => p.SERVICE_NAME).ThenBy(p => p.MEDICINE_TYPE_NAME).ToList();
            objectTag.AddObjectData(store, "ReportPPVC", ListPPVC);
            objectTag.AddObjectData(store, "ReportPTGM", ListRdo.Where(p => !string.IsNullOrEmpty(p.EXECUTE_ROLE_NAME) && p.EXECUTE_ROLE_NAME.Contains("mê")).OrderBy(p => p.IN_TIME).ToList());
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.TDL_INTRUCTION_TIME).ToList());
            objectTag.AddObjectData(store, "ExecuteDepartment", ListRdo.OrderBy(o => o.finish_Time).GroupBy(g => g.TDL_EXECUTE_DEPARTMENT_ID).Select(s => s.First()).ToList());
            objectTag.AddRelationship(store, "ExecuteDepartment", "Report", "TDL_EXECUTE_DEPARTMENT_ID", "TDL_EXECUTE_DEPARTMENT_ID");
            objectTag.AddObjectData(store, "Table", ListRdo.OrderBy(o => o.finish_Time).GroupBy(g => g.PTTT_TABLE_NAME).Select(s => s.First()).ToList());
            objectTag.AddRelationship(store, "Table", "Report", "PTTT_TABLE_NAME", "PTTT_TABLE_NAME");
            objectTag.SetUserFunction(store, "Element", new RDOElement());

            objectTag.AddObjectData(store, "DT", ListRdoDT.Where(p => p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT || p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__DV).ToList());

            objectTag.AddObjectData(store, "PTTTType", ListRdoPTTTType.Where(p => p.PTTT_GROUP_ID != null).ToList());
            objectTag.AddObjectData(store, "PPVC", ListRdoPPVC1.Where(p => p.EMOTIONLESS_METHOD_ID != null).ToList());
            objectTag.AddObjectData(store, "PPPT", ListRdoPPPT.Where(p => p.PTTT_METHOD_ID != null).ToList());

            long a = 1;
            var ListDepartment = ListRdo.Select(p => p.TDL_REQUEST_DEPARTMENT_NAME).Distinct().ToList();
            if (ListDepartment != null)
            {
                foreach (var item in ListDepartment)
                {
                    dicSingleTag.Add("DEPARTMENT_NAME__" + a, item);
                    a++;
                }
            }

            dicSingleTag.Add("TOTAL_AMOUNT", ListRdoDT.Where(p => p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT || p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__DV).Sum(p => p.SERVICE_AMOUNT));

            if (castFilter.EXECUTE_MACHINE_IDs != null)
            {
                var machine = this.ListMachine.Where(o => castFilter.EXECUTE_MACHINE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("EXECUTE_MACHINE_NAMEs", string.Join(";", machine.Select(o => o.MACHINE_NAME).ToList()));
            }
            if (castFilter.MACHINE_IDs != null)
            {
                var machine = this.ListMachine.Where(o => castFilter.MACHINE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("MACHINE_NAMEs", string.Join(";", machine.Select(o => o.MACHINE_NAME).ToList()));
            }
            if (castFilter.PATIENT_TYPE_IDs != null)
            {
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
            }
            if (castFilter.TDL_PATIENT_TYPE_IDs != null)
            {
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.TDL_PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("TDL_PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
            }
        }
    }

    class ServiceFuncMergeSameData : TFlexCelUserFunction
    {
        long ServiceId;

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            bool result = false;
            try
            {
                long ServiceId_ = Convert.ToInt64(parameters[0]);

                if (ServiceId_ != null)
                {
                    if (ServiceId_ == ServiceId)
                    {
                        return true;
                    }
                    else
                    {
                        ServiceId = ServiceId_;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }
            return result;
        }
    }
}
