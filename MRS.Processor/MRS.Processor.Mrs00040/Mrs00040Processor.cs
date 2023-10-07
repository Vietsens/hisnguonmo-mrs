using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisEkip;
using MOS.MANAGER.HisExecuteRole;
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
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisEkipUser;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisIcd;
using System.Reflection;
using MOS.MANAGER.HisPtttMethod;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSereServExt;
using System.IO;
using MOS.MANAGER.HisServiceRetyCat;

namespace MRS.Processor.Mrs00040
{
    public class Mrs00040Processor : AbstractProcessor
    {
        Mrs00040Filter castFilter = null;
        private List<Mrs00040RDO> ListRdo = new List<Mrs00040RDO>();
        private List<Mrs00040RDO> ListRdoTreatmentService = new List<Mrs00040RDO>();
        List<Mrs00040RDO> ListRdoDT = new List<Mrs00040RDO>();
        List<Mrs00040RDO> ListRdoPTTTGroup = new List<Mrs00040RDO>();
        List<Mrs00040RDO> ListRdoPPVC1 = new List<Mrs00040RDO>();
        List<Mrs00040RDO> ListRdoPPPT = new List<Mrs00040RDO>();
        List<HIS_EKIP_USER> listHisEkipUser = new List<HIS_EKIP_USER>();
        List<HIS_EXECUTE_ROLE> listHisExecuteRole = new List<HIS_EXECUTE_ROLE>();
        List<HIS_PTTT_METHOD> listHisPtttMethod = new List<HIS_PTTT_METHOD>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        //List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceRetyCat = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, List<SESE_PTTT_METHOD>> dicSesePtttMethod = new Dictionary<long, List<SESE_PTTT_METHOD>>();
        List<HIS_EKIP_USER> listHisEkipUserReal = new List<HIS_EKIP_USER>();
        List<HIS_EXECUTE_ROLE> listHisExecuteRoleReal = new List<HIS_EXECUTE_ROLE>();
        List<HIS_PTTT_METHOD> listHisPtttMethodReal = new List<HIS_PTTT_METHOD>();

        Dictionary<long, List<HIS_SERE_SERV_EXT>> dicSereServExt = new Dictionary<long, List<HIS_SERE_SERV_EXT>>();
        Dictionary<long, List<HIS_SERE_SERV_PTTT>> dicSereServPttt = new Dictionary<long, List<HIS_SERE_SERV_PTTT>>();
        Dictionary<long, List<HIS_SERE_SERV_BILL>> dicSereServBill = new Dictionary<long, List<HIS_SERE_SERV_BILL>>();
        Dictionary<long, HIS_TRANSACTION> dicTransaction = new Dictionary<long, HIS_TRANSACTION>();
        Dictionary<long, List<HIS_SERVICE_MACHINE>> dicServiceMachine = new Dictionary<long, List<HIS_SERVICE_MACHINE>>();
        List<HIS_MACHINE> ListMachine = new List<HIS_MACHINE>();

        CommonParam paramGet = new CommonParam();
        public Mrs00040Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00040Filter);
        }

        protected override bool GetData()
        {

            castFilter = ((Mrs00040Filter)reportFilter);
            Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00040: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

            var result = true;
            try
            {
                //danh sach phong thuc hien
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
                Inventec.Common.Logging.LogSystem.Error("ListServiceReq" + ListRdo.Count);
                if (castFilter.BRANCH_ID != null)
                {
                    ListRdo = ListRdo.Where(o => HisDepartmentCFG.DEPARTMENTs.Exists(p => p.ID == o.TDL_REQUEST_DEPARTMENT_ID && p.BRANCH_ID == castFilter.BRANCH_ID)).ToList();
                }

                if (IsNotNullOrEmpty(castFilter.BRANCH_IDs))
                {
                    ListRdo = ListRdo.Where(o => HisDepartmentCFG.DEPARTMENTs.Exists(p => p.ID == o.TDL_REQUEST_DEPARTMENT_ID && castFilter.BRANCH_IDs.Contains(p.BRANCH_ID))).ToList();
                }


                var lisSereServIds = ListRdo.Select(s => s.ID).Distinct().ToList();

                List<HIS_SERE_SERV_EXT> listExt = new List<HIS_SERE_SERV_EXT>();
                var skip = 0;
                if (IsNotNullOrEmpty(lisSereServIds))
                {
                    while (lisSereServIds.Count - skip > 0)
                    {
                        var listIDs = lisSereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisSereServExtFilterQuery sereServExt = new HisSereServExtFilterQuery();
                        sereServExt.SERE_SERV_IDs = listIDs;
                        if (castFilter.IS_NOT_FEE.HasValue) sereServExt.IS_NOT_FEE = castFilter.IS_NOT_FEE == 1;
                        if (castFilter.IS_NOT_GATHER_DATA.HasValue) sereServExt.IS_NOT_GATHER_DATA = castFilter.IS_NOT_GATHER_DATA == 1;

                        var listSsExt = new HisSereServExtManager(paramGet).Get(sereServExt);
                        if (listSsExt != null)
                        {
                            listExt.AddRange(listSsExt);
                        }
                    }

                    dicSereServExt = listExt.GroupBy(o => o.SERE_SERV_ID).ToDictionary(p => p.Key, q => q.ToList());
                    if (IsNotNullOrEmpty(ListRdo)  && (castFilter.IS_NOT_FEE.HasValue || castFilter.IS_NOT_GATHER_DATA.HasValue))
                    {
                        ListRdo = ListRdo.Where(o => dicSereServExt.ContainsKey(o.ID)).ToList();
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

                    if (castFilter.TREATMENT_TYPE_ID.HasValue && IsNotNullOrEmpty(ListRdo) && IsNotNullOrEmpty(lastHisPatientTypeAlter))
                    {
                        ListRdo = ListRdo.Where(o => lastHisPatientTypeAlter.Exists(
                            p => o.TDL_TREATMENT_ID == p.TREATMENT_ID
                                && p.TREATMENT_TYPE_ID == castFilter.TREATMENT_TYPE_ID)).ToList();
                    }
                }

                var sereServIds = ListRdo.Select(o => o.ID).ToList();
                List<HIS_SERE_SERV_PTTT> listHisSereServPttt = new List<HIS_SERE_SERV_PTTT>();
                List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
                List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();
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
                            Inventec.Common.Logging.LogSystem.Error("listHisSereServPtttSub is null");
                        else
                        {
                            listHisSereServPttt.AddRange(listHisSereServPtttSub);
                        }
                    }

                    skip = 0;
                    while (sereServIds.Count - skip > 0)
                    {
                        var listIDs = sereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillFilterQuery filterSereServBill = new HisSereServBillFilterQuery();
                        filterSereServBill.SERE_SERV_IDs = listIDs;
                        filterSereServBill.IS_NOT_CANCEL = true;
                        var listSereServBillSub = new HisSereServBillManager(paramGet).Get(filterSereServBill);
                        if (listSereServBillSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listSereServBillSub is null");
                        else
                        {
                            ListSereServBill.AddRange(listSereServBillSub);

                            HisTransactionFilterQuery filterTransaction = new HisTransactionFilterQuery();
                            filterTransaction.IDs = listSereServBillSub.Select(s => s.BILL_ID).Distinct().ToList();
                            filterTransaction.IS_CANCEL = false;
                            var listTransactionSub = new HisTransactionManager(paramGet).Get(filterTransaction);
                            if (listTransactionSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listTransactionSub is null");
                            else
                            {
                                ListTransaction.AddRange(listTransactionSub);
                            }
                        }
                    }
                    dicSereServPttt = listHisSereServPttt.GroupBy(o => o.SERE_SERV_ID).ToDictionary(p => p.Key, q => q.ToList());
                    dicSereServBill = ListSereServBill.GroupBy(o => o.SERE_SERV_ID).ToDictionary(p => p.Key, q => q.ToList());
                    dicTransaction = ListTransaction.GroupBy(o => o.ID).ToDictionary(p => p.Key, q => q.First());

                }

                if ((castFilter.HAS_BILL ?? false) && IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.Where(o => dicSereServBill.ContainsKey(o.ID)).ToList();
                }

                if (castFilter.HAS_BILL_OR_BHYT != null && IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.Where(o => (dicSereServBill.ContainsKey(o.ID) || o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT) == castFilter.HAS_BILL_OR_BHYT).ToList();
                }

                var PtttMethodIds = listHisSereServPttt.Select(o => o.PTTT_METHOD_ID ?? 0).Distinct().ToList();
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
                            Inventec.Common.Logging.LogSystem.Error("listHisPtttMethodSub is null");
                        else
                        {
                            listHisPtttMethod.AddRange(listHisPtttMethodSub);
                        }
                    }
                }

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
                            Inventec.Common.Logging.LogSystem.Error("listHisEkipUserSub is null");
                        else
                        {
                            listHisEkipUser.AddRange(listHisEkipUserSub);
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
                            Inventec.Common.Logging.LogSystem.Error("listHisExecuteRoleSub is null");
                        else
                        {
                            listHisExecuteRole.AddRange(listHisExecuteRoleSub);
                        }
                    }
                }
                //cac loai phau thuat
                List<long> PtttGroupIds = (HisPtttGroupCFG.PTTT_GROUPs ?? new List<HIS_PTTT_GROUP>()).Select(o => o.ID).ToList();
                listHisService = new HisServiceManager(paramGet).Get(new HisServiceFilterQuery() { PTTT_GROUP_IDs = PtttGroupIds });
                //ListSereServBill = ListSereServBill.Where(o => ListSereServ.Exists(p => o.SERE_SERV_ID == p.ID)).ToList();

                //danh sach dich vu - nhom bao cao
                GetServiceRetyCat();


                if (castFilter.ADD_REAL_METHOD_INFO == true)
                {
                    //thêm thông tin phẫu thuật thực tế
                    var PtttMethodRealIds = new List<long>();
                    var ekipRealIds = new List<long>();
                    if (IsNotNullOrEmpty(sereServIds))
                    {
                        skip = 0;
                        while (sereServIds.Count - skip > 0)
                        {
                            var listIDs = sereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            var listSesePtttMethodSub = new ManagerSql().GetSesePtttMethod(listIDs);
                            if (listSesePtttMethodSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listSesePtttMethodSub is null");
                            else
                            {
                                foreach (var item in listSesePtttMethodSub)
                                {
                                    if (!dicSesePtttMethod.ContainsKey(item.TDL_SERE_SERV_ID ?? 0))
                                    {
                                        dicSesePtttMethod[item.TDL_SERE_SERV_ID ?? 0] = new List<SESE_PTTT_METHOD>();
                                    }
                                    dicSesePtttMethod[item.TDL_SERE_SERV_ID ?? 0].Add(item);
                                    if (item.PTTT_METHOD_ID > 0)
                                    {
                                        PtttMethodRealIds.Add(item.PTTT_METHOD_ID ?? 0);
                                    }
                                    if (item.EKIP_ID > 0)
                                    {
                                        ekipRealIds.Add(item.EKIP_ID ?? 0);
                                    }
                                }
                            }
                        }
                    }
                    //phương pháp thực tế
                    PtttMethodRealIds = PtttMethodRealIds.Distinct().ToList();
                    if (IsNotNullOrEmpty(PtttMethodRealIds))
                    {
                        skip = 0;
                        while (PtttMethodRealIds.Count - skip > 0)
                        {
                            var listIDs = PtttMethodRealIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisPtttMethodFilterQuery HisPtttMethodfilter = new HisPtttMethodFilterQuery();
                            HisPtttMethodfilter.IDs = listIDs;
                            var listHisPtttMethodRealSub = new HisPtttMethodManager(paramGet).Get(HisPtttMethodfilter);
                            if (listHisPtttMethodRealSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listHisPtttMethodRealSub is null");
                            else
                            {
                                listHisPtttMethodReal.AddRange(listHisPtttMethodRealSub);
                            }
                        }
                    }

                    //thông tin kip làm thực tế
                    ekipRealIds = ekipRealIds.Distinct().ToList();
                    if (IsNotNullOrEmpty(ekipRealIds))
                    {
                        skip = 0;
                        while (ekipRealIds.Count - skip > 0)
                        {
                            var listIDs = ekipRealIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisEkipUserFilterQuery HisEkipUserfilter = new HisEkipUserFilterQuery();
                            HisEkipUserfilter.EKIP_IDs = listIDs;
                            HisEkipUserfilter.ORDER_FIELD = "ID";
                            HisEkipUserfilter.ORDER_DIRECTION = "ASC";
                            var listHisEkipUserRealSub = new HisEkipUserManager(paramGet).Get(HisEkipUserfilter);
                            if (listHisEkipUserRealSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listHisEkipUserRealSub is null");
                            else
                            {
                                listHisEkipUserReal.AddRange(listHisEkipUserRealSub);
                            }
                        }
                    }
                    //thông tin vai trò của kip làm thực tế
                    var executeRoleRealIds = listHisEkipUserReal.Select(o => o.EXECUTE_ROLE_ID).Distinct().ToList();
                    if (IsNotNullOrEmpty(executeRoleRealIds))
                    {
                        skip = 0;
                        while (executeRoleRealIds.Count - skip > 0)
                        {
                            var listIDs = executeRoleRealIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisExecuteRoleFilterQuery HisExecuteRolefilter = new HisExecuteRoleFilterQuery();
                            HisExecuteRolefilter.IDs = listIDs;
                            var listHisExecuteRoleRealSub = new HisExecuteRoleManager(paramGet).Get(HisExecuteRolefilter);
                            if (listHisExecuteRoleRealSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listHisExecuteRoleRealSub is null");
                            else
                            {
                                listHisExecuteRoleReal.AddRange(listHisExecuteRoleRealSub);
                            }
                        }
                    }
                }

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
                dicSereServExt = dicSereServExt.Where(o => sereServIds.Contains(o.Key)).ToDictionary(p=>p.Key,q=>q.Value);
            }
            if (castFilter.EXECUTE_MACHINE_IDs != null)
            {
                var sereServIds = dicSereServExt.SelectMany(o => o.Value).Where(q => castFilter.EXECUTE_MACHINE_IDs.Contains(q.MACHINE_ID??0)).Select(p => p.SERE_SERV_ID).ToList();
                dicSereServExt = dicSereServExt.Where(o => sereServIds.Contains(o.Key)).ToDictionary(p => p.Key, q => q.Value);
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
                    //bổ sung thêm mỗi thông tin phương pháp thực tế là 1 lượt phẫu thuật thủ thuật
                    AddRealPttt();
                    ProcessListSereServPttt();
                    ListRdo = ListRdo.OrderBy(o => o.PATIENT_CODE).ToList();
                    ProcessGroupTT(ListRdo);
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void AddRealPttt()
        {
            try
            {
                var ListRdoReal = ListRdo.Where(o => dicSesePtttMethod.Keys != null && dicSesePtttMethod.Keys.Contains(o.ID)).ToList();
                foreach (var item in ListRdoReal)
                {
                    if (dicSesePtttMethod.ContainsKey(item.ID) && dicSesePtttMethod[item.ID] != null)
                    {
                        foreach (var ssptt in dicSesePtttMethod[item.ID])
                        {
                            Mrs00040RDO rdo = new Mrs00040RDO();
                            System.Reflection.PropertyInfo[] pis = Inventec.Common.Repository.Properties.Get<Mrs00040RDO>();
                            foreach (var pi in pis)
                            {
                                pi.SetValue(rdo, pi.GetValue(item));
                            }

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
                            rdo.AMOUNT = ssptt.AMOUNT ?? 0;
                            //thông tin PTT thực tế
                            rdo.MISU_PPPT_REAL = (listHisPtttMethodReal.FirstOrDefault(o => o.ID == ssptt.PTTT_METHOD_ID) ?? new HIS_PTTT_METHOD()).PTTT_METHOD_NAME;
                            rdo.PTTT_GROUP_NAME_REAL = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == ssptt.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                            //ekip thực hiện PTTT thực tế
                            var ekipUserReal = listHisEkipUserReal.Where(o => ssptt.EKIP_ID == o.EKIP_ID).ToList();
                            if (rdo.DICR_EXECUTE_USERNAME == null)
                            {
                                rdo.DICR_EXECUTE_USERNAME = new Dictionary<string, string>();
                            }
                            if (rdo.DICR_EXECUTE_USERNAME_REAL == null)
                            {
                                rdo.DICR_EXECUTE_USERNAME_REAL = new Dictionary<string, string>();
                            }
                            if (IsNotNullOrEmpty(ekipUserReal) && rdo.DICR_EXECUTE_USERNAME.Keys.Count == 0)
                            {
                                rdo.DICR_EXECUTE_USERNAME_REAL = ekipUserReal.GroupBy(o => (listHisExecuteRoleReal.FirstOrDefault(p => p.ID == o.EXECUTE_ROLE_ID) ?? new HIS_EXECUTE_ROLE()).EXECUTE_ROLE_CODE).ToDictionary(q => q.Key, q => string.Join("\r\n", q.Select(o => o.USERNAME).ToList()));
                            }
                            ListRdo.Add(rdo);
                        }

                        item.HAS_REAL = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessListSereServPttt()
        {
            try
            {
                var GroupByTreatmentAndServcie = ListRdo.GroupBy(o => new { o.TDL_TREATMENT_ID, o.SERVICE_ID, o.PATIENT_TYPE_ID }).ToList();
                int ix = 0;
                int count = GroupByTreatmentAndServcie.Count;
                foreach (var item in GroupByTreatmentAndServcie)
                {
                    ix++;
                    if (ix == (int)(count / 10))
                    {
                        LogSystem.Info("10 %");
                    }
                    else if (ix == (int)(count / 5))
                    {
                        LogSystem.Info("20 %");
                    }
                    else if (ix == (int)(count / 2))
                    {
                        LogSystem.Info("50 %");
                    }
                    else if (ix == (int)(count * 3 / 4))
                    {
                        LogSystem.Info("75 %");
                    }
                    else if (ix == (int)(count))
                    {
                        LogSystem.Info("100 %");
                    }
                    List<Mrs00040RDO> listSub = item.ToList<Mrs00040RDO>();
                    Mrs00040RDO firstSereServ = item.First();
                    var firstSSB = dicSereServBill.ContainsKey(firstSereServ.ID) ? dicSereServBill[firstSereServ.ID].FirstOrDefault() : null;

                    var transaction = firstSSB != null && dicTransaction.ContainsKey(firstSSB.BILL_ID) ? dicTransaction[firstSSB.BILL_ID] : new HIS_TRANSACTION();
                    //var treat = listTreatment.FirstOrDefault(o => o.ID == item.First().TDL_TREATMENT_ID) ?? new HIS_TREATMENT();
                    Mrs00040RDO r = new Mrs00040RDO(firstSereServ, transaction);
                    r.PATIENT_NAME = item.First().PATIENT_NAME;
                    r.PATIENT_FIRST_NAME = item.First().TDL_PATIENT_FIRST_NAME;
                    r.TDL_HEIN_MEDI_ORG_CODE = item.First().TDL_HEIN_MEDI_ORG_CODE;

                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == r.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    var patientType1 = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == r.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    if (patientType.ID != HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        r.HEIN_CARD_NUMBER = item.First().HEIN_CARD_NUMBER;
                    if (patientType.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        r.PATIENT_TYPE_NAME_01 = "BHYT";
                    else if (patientType.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        r.PATIENT_TYPE_NAME_01 = "VP";
                    else
                        r.PATIENT_TYPE_NAME_01 = "XHH";
                    r.PATIENT_TYPE_NAME_02 = patientType1.PATIENT_TYPE_NAME;
                    r.START_TIME = firstSereServ.START_TIME;
                    r.END_TIME_STR = firstSereServ.END_TIME_STR;
                    r.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                    r.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                    r.PATIENT_CODE = item.First().PATIENT_CODE;
                    r.TREATMENT_CODE = item.First().TDL_TREATMENT_CODE;
                    r.SERVICE_NAME = firstSereServ.TDL_SERVICE_NAME;
                    r.TREA_ICD_NAME = firstSereServ.TREA_ICD_NAME;
                    r.TREA_ICD_CODE = firstSereServ.TREA_ICD_CODE;
                    r.TREA_SUB_NAME = firstSereServ.TREA_SUB_NAME;
                    r.TREA_SUB_CODE = firstSereServ.TREA_SUB_CODE;
        
                    if (dicServiceRetyCat.ContainsKey(firstSereServ.SERVICE_ID))
                    {
                        r.CATEGORY_CODE = dicServiceRetyCat[firstSereServ.SERVICE_ID].CATEGORY_CODE;
                        r.CATEGORY_NAME = dicServiceRetyCat[firstSereServ.SERVICE_ID].CATEGORY_NAME;
                    }

                    CalcuatorAge(r, item.First());
                    IsBhyt(item.First(), r);

                    var serviceMerge = listHisService.FirstOrDefault(o => o.ID == firstSereServ.SERVICE_ID) ?? new HIS_SERVICE();
                    r.DEFAULT_MISU_TYPE_NAME = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == serviceMerge.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                    r.AMOUNT = listSub.Sum(o => o.AMOUNT);

                    r.VIR_TOTAL_PRICE = listSub.Sum(o => o.VIR_TOTAL_PRICE);

                    ListRdoTreatmentService.Add(r);
                    foreach (var sereServ in listSub)
                    {
                        var fsSSB = dicSereServBill.ContainsKey(sereServ.ID) ? dicSereServBill[sereServ.ID].FirstOrDefault() : null;

                        var tran = fsSSB != null && dicTransaction.ContainsKey(fsSSB.BILL_ID) ? dicTransaction[fsSSB.BILL_ID] : new HIS_TRANSACTION();

                        sereServ.HIS_TRANSACTION = tran;
                        if (sereServ.DICR_EXECUTE_USERNAME == null)
                        {
                            sereServ.DICR_EXECUTE_USERNAME = new Dictionary<string, string>();
                        }
                        if (sereServ.DICR_EXECUTE_USERNAME_REAL == null)
                        {
                            sereServ.DICR_EXECUTE_USERNAME_REAL = new Dictionary<string, string>();
                        }
                        if (sereServ.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            sereServ.TDL_PATIENT_DOB_MALE_STR = r.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                        }
                        if (sereServ.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            sereServ.TDL_PATIENT_DOB_FEMALE_STR = r.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                        }
                        sereServ.SERVICE_AMOUNT = sereServ.AMOUNT;
                        sereServ.SERVICE_REQ_CODE = sereServ.TDL_SERVICE_REQ_CODE;
                        sereServ.PATIENT_FIRST_NAME = sereServ.TDL_PATIENT_FIRST_NAME;
                        sereServ.TREATMENT_CODE = sereServ.TDL_TREATMENT_CODE;
                        sereServ.REQUEST_USERNAME = sereServ.TDL_REQUEST_USERNAME;
                        sereServ.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServ.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        sereServ.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        sereServ.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                        sereServ.finish_Time = sereServ.TDL_FINISH_TIME ?? 0;
                        sereServ.FINISH_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.TDL_FINISH_TIME ?? 0);
                        //rdo.NOTE = sereServ.NOTE;
                        sereServ.TIME_MISU_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.START_TIME ?? 0);
                        CalcuatorAge(sereServ, sereServ);
                        if (patientType.ID != HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                            if (patientType.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                sereServ.PATIENT_TYPE_NAME_01 = "BHYT";
                            else if (patientType.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                                sereServ.PATIENT_TYPE_NAME_01 = "VP";
                            else
                                sereServ.PATIENT_TYPE_NAME_01 = "XHH";
                        sereServ.PATIENT_TYPE_NAME_02 = patientType1.PATIENT_TYPE_NAME;
                        sereServ.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                        sereServ.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                        //đối tượng bệnh nhân
                        sereServ.TDL_PATIENT_TYPE_NAME = patientType1.PATIENT_TYPE_NAME;
                        sereServ.TDL_PATIENT_TYPE_CODE = patientType1.PATIENT_TYPE_CODE;

                        IsBhyt(sereServ, sereServ);
                        //thông tin PTTT
                        var sereServPttt = dicSereServPttt.ContainsKey(sereServ.ID)?dicSereServPttt[sereServ.ID].FirstOrDefault() : new HIS_SERE_SERV_PTTT();

                        //thông tin PTT thực tế
                        //var sereServPtttReal = listSesePtttMethod.FirstOrDefault(o => o.TDL_SERE_SERV_ID == sereServ.ID) ?? new SESE_PTTT_METHOD();
                        sereServ.PTTT_METHOD_ID = sereServPttt.PTTT_METHOD_ID;
                        sereServ.MISU_PPPT = (listHisPtttMethod.FirstOrDefault(o => o.ID == sereServPttt.PTTT_METHOD_ID) ?? new HIS_PTTT_METHOD()).PTTT_METHOD_NAME;
                        //phương pháp PTTT thực tế:
                        //sereServ.MISU_PPPT_REAL = (listHisPtttMethodReal.FirstOrDefault(o => o.ID == sereServPtttReal.PTTT_METHOD_ID) ?? new HIS_PTTT_METHOD()).PTTT_METHOD_NAME;
                        sereServ.EMOTIONLESS_METHOD_ID = sereServPttt.EMOTIONLESS_METHOD_ID;
                        sereServ.MISU_PPVC = (HisEmotionlessMethodCFG.EMOTIONLESS_METHODs.FirstOrDefault(o => o.ID == sereServPttt.EMOTIONLESS_METHOD_ID) ?? new HIS_EMOTIONLESS_METHOD()).EMOTIONLESS_METHOD_NAME;
                        sereServ.BEFORE_MISU = sereServPttt.BEFORE_PTTT_ICD_NAME;
                        sereServ.BEFORE_MISU_CODE = sereServPttt.BEFORE_PTTT_ICD_CODE;
                        if (sereServPttt.BEFORE_PTTT_ICD_CODE != null || sereServPttt.BEFORE_PTTT_ICD_NAME != null)
                        {
                            sereServ.BEFORE_MISU_FULL = sereServ.BEFORE_MISU_CODE + " - " + sereServ.BEFORE_MISU + ";";
                        }
                        sereServ.AFTER_MISU = sereServPttt.AFTER_PTTT_ICD_NAME;
                        sereServ.AFTER_MISU_CODE = sereServPttt.AFTER_PTTT_ICD_CODE;
                        if (sereServ.AFTER_MISU_CODE != null || sereServ.AFTER_MISU != null)
                        {
                            sereServ.AFTER_MISU_FULL = sereServ.AFTER_MISU_CODE + " - " + sereServ.AFTER_MISU + ";";
                        }
                        if (string.IsNullOrWhiteSpace(sereServ.ICD_SUB_CODE))
                        {
                            sereServ.ICD_SUB_CODE = sereServPttt.ICD_SUB_CODE;
                            sereServ.ICD_SUB_NAME = sereServPttt.ICD_TEXT;
                        }


                        if (sereServ.ICD_SUB_NAME != null && sereServ.ICD_SUB_CODE != null)
                        {
                            string[] icdSubCode = sereServ.ICD_SUB_CODE.Split(';');
                            string[] icdSubName = sereServ.ICD_SUB_NAME.Split(';');
                            if (icdSubCode.Length > 0 && icdSubCode.Length == icdSubName.Length)
                            {
                                for (int i = 1; i < icdSubCode.Length; i++)
                                {
                                    sereServ.ICD_SUB_FULL += icdSubCode[i] + " - " + icdSubName[i] + ";";
                                }
                            }

                        }
                        sereServ.PTTT_GROUP_ID = serviceMerge.PTTT_GROUP_ID;
                        sereServ.PTTT_GROUP_NAME = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == serviceMerge.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                        //sereServ.PTTT_GROUP_NAME_REAL = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == sereServPtttReal.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                        sereServ.MISU_TYPE_NAME = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == sereServPttt.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                        var service = listHisService.FirstOrDefault(o => o.ID == sereServ.SERVICE_ID) ?? new HIS_SERVICE();
                        sereServ.DEFAULT_MISU_TYPE_NAME = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == service.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;

                        //ekip thực hiện PTTT
                        var ekipUser = listHisEkipUser.Where(o => o.EKIP_ID == (sereServ.EKIP_ID ?? 0)).ToList();

                        if (IsNotNullOrEmpty(ekipUser) && sereServ.DICR_EXECUTE_USERNAME_REAL.Keys.Count == 0)
                        {
                            sereServ.DICR_EXECUTE_USERNAME = ekipUser.GroupBy(o => (listHisExecuteRole.FirstOrDefault(p => p.ID == o.EXECUTE_ROLE_ID) ?? new HIS_EXECUTE_ROLE()).EXECUTE_ROLE_CODE).ToDictionary(q => q.Key, q => string.Join("\r\n", q.Select(o => o.USERNAME).ToList()));
                        }

                        ////ekip thực hiện PTTT thực tế
                        //var ekipUserReal = listHisEkipUserReal.Where(o => o.EKIP_ID == (sereServPtttReal.EKIP_ID ?? 0)).ToList();

                        //if (IsNotNullOrEmpty(ekipUserReal))
                        //{
                        //    sereServ.DICR_EXECUTE_USERNAME_REAL = ekipUserReal.GroupBy(o => (listHisExecuteRoleReal.FirstOrDefault(p => p.ID == o.EXECUTE_ROLE_ID) ?? new HIS_EXECUTE_ROLE()).EXECUTE_ROLE_CODE).ToDictionary(q => q.Key, q => string.Join("\r\n", q.Select(o => o.USERNAME).ToList()));
                        //}

                        sereServ.PTTT_PRIORITY_NAME = (HisPtttPriorityCFG.PTTT_PRIORITYs.FirstOrDefault(o => o.ID == sereServPttt.PTTT_PRIORITY_ID) ?? new HIS_PTTT_PRIORITY()).PTTT_PRIORITY_NAME;

                        sereServ.PTTT_TABLE_NAME = (HisPtttTableCFG.PTTT_TABLEs.FirstOrDefault(o => o.ID == sereServPttt.PTTT_TABLE_ID) ?? new HIS_PTTT_TABLE()).PTTT_TABLE_NAME;

                        sereServ.EMOTIONLESS_RESULT_NAME = (HisEmotionlessResultCFG.EMOTIONLESS_RESULTs.FirstOrDefault(o => o.ID == sereServPttt.EMOTIONLESS_RESULT_ID) ?? new HIS_EMOTIONLESS_RESULT()).EMOTIONLESS_RESULT_NAME;

                        sereServ.SURG_PPVC_2 = (HisEmotionlessMethodCFG.EMOTIONLESS_METHODs.FirstOrDefault(o => o.ID == sereServPttt.EMOTIONLESS_METHOD_SECOND_ID) ?? new HIS_EMOTIONLESS_METHOD()).EMOTIONLESS_METHOD_NAME;

                        sereServ.TDL_EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sereServ.TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        sereServ.TDL_REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;

                        HIS_SERE_SERV_EXT ext = dicSereServExt.ContainsKey(sereServ.ID)? dicSereServExt[sereServ.ID].FirstOrDefault() : null;
                        if (ext != null)
                        {
                            sereServ.BEGIN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ext.BEGIN_TIME ?? 0);
                            sereServ.BEGIN_TIME = ext.BEGIN_TIME ?? 0;
                            sereServ.DESCRIPTION_SURGERY = ext.DESCRIPTION;
                            sereServ.NOTE = ext.NOTE;
                            sereServ.INTRUCTION_NOTE = ext.INSTRUCTION_NOTE;
                            var machineExt = ListMachine.Where(p => ext.MACHINE_ID == p.ID).ToList();
                            if (machineExt.Count > 0)
                            {
                                sereServ.EXECUTE_MACHINE_NAME = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                                sereServ.EXECUTE_MACHINE_CODE = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                            }
                        }

                        if (ext != null && ext.END_TIME.HasValue)
                        {
                            sereServ.END_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ext.END_TIME ?? 0);
                            sereServ.END_TIME = ext.END_TIME ?? 0;
                        }


                        if (dicServiceRetyCat.ContainsKey(sereServ.SERVICE_ID))
                        {
                            sereServ.CATEGORY_CODE = dicServiceRetyCat[sereServ.SERVICE_ID].CATEGORY_CODE;
                            sereServ.CATEGORY_NAME = dicServiceRetyCat[sereServ.SERVICE_ID].CATEGORY_NAME;
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

        private void ProcessGroupTT(List<Mrs00040RDO> ListRdo)
        {
            try
            {
                if (ListRdo != null)
                {
                    //doi tuong
                    ListRdoDT = ListRdo.GroupBy(p => new { p.TDL_PATIENT_TYPE_ID }).Select(q => new Mrs00040RDO
                    {
                        TDL_PATIENT_TYPE_ID = q.First().TDL_PATIENT_TYPE_ID,

                        PATIENT_TYPE_NAME = q.First().PATIENT_TYPE_NAME,

                        DIC_REQ_DEPARTMENT_AMOUNT = q.Where(o => !string.IsNullOrWhiteSpace(o.TDL_REQUEST_DEPARTMENT_NAME)).GroupBy(g => g.TDL_REQUEST_DEPARTMENT_NAME).ToDictionary(p => p.Key, y => y.Sum(p => p.SERVICE_AMOUNT)),
                    }).ToList();

                    //nhom pt
                    ListRdoPTTTGroup = ListRdo.Where(P => P.PTTT_GROUP_ID != null).GroupBy(p => new { p.PTTT_GROUP_ID }).Select(q => new Mrs00040RDO
                    {
                        PTTT_GROUP_ID = q.First().PTTT_GROUP_ID,
                        DEFAULT_MISU_TYPE_NAME = q.First().DEFAULT_MISU_TYPE_NAME,

                        DIC_REQ_DEPARTMENT_AMOUNT = q.Where(o => !string.IsNullOrWhiteSpace(o.TDL_REQUEST_DEPARTMENT_NAME)).GroupBy(g => g.TDL_REQUEST_DEPARTMENT_NAME).ToDictionary(p => p.Key, y => y.Sum(p => p.SERVICE_AMOUNT)),

                    }).ToList();

                    //ppvc
                    ListRdoPPVC1 = ListRdo.Where(P => P.EMOTIONLESS_METHOD_ID != null).GroupBy(p => new { p.EMOTIONLESS_METHOD_ID }).Select(q => new Mrs00040RDO
                    {
                        EMOTIONLESS_METHOD_ID = q.First().EMOTIONLESS_METHOD_ID,
                        MISU_PPVC = q.First().MISU_PPVC,

                        DIC_REQ_DEPARTMENT_AMOUNT = q.Where(o => !string.IsNullOrWhiteSpace(o.TDL_REQUEST_DEPARTMENT_NAME)).GroupBy(g => g.TDL_REQUEST_DEPARTMENT_NAME).ToDictionary(p => p.Key, y => y.Sum(p => p.SERVICE_AMOUNT)),

                    }).ToList();

                    //pppt
                    ListRdoPPPT = ListRdo.Where(P => P.PTTT_METHOD_ID != null).GroupBy(p => new { p.PTTT_METHOD_ID }).Select(q => new Mrs00040RDO
                    {
                        PTTT_METHOD_ID = q.First().PTTT_METHOD_ID,
                        MISU_PPPT = q.First().MISU_PPPT,

                        DIC_REQ_DEPARTMENT_AMOUNT = q.Where(o => !string.IsNullOrWhiteSpace(o.TDL_REQUEST_DEPARTMENT_NAME)).GroupBy(g => g.TDL_REQUEST_DEPARTMENT_NAME).ToDictionary(p => p.Key, y => y.Sum(p => p.SERVICE_AMOUNT)),

                    }).ToList();


                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void CalcuatorAge(Mrs00040RDO rdo, Mrs00040RDO item)
        {
            try
            {
                rdo.VIR_ADDRESS = item.VIR_ADDRESS;
                int? tuoi = RDOCommon.CalculateAge(item.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.MALE_YEAR = ProcessYearDob(item.TDL_PATIENT_DOB);
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.FEMALE_YEAR = ProcessYearDob(item.TDL_PATIENT_DOB);
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

        private void IsBhyt(Mrs00040RDO item, Mrs00040RDO rdo)
        {
            try
            {
                if (item.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.HEIN_CARD_NUMBER_02 = item.TDL_HEIN_CARD_NUMBER;
                    rdo.IS_BHYT = "X";
                }
                //if (item.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                //rdo.PATIENT_TYPE_NAME = "Viện phí";



            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00040Filter)this.reportFilter).TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00040Filter)this.reportFilter).FINISH_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00040Filter)this.reportFilter).TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00040Filter)this.reportFilter).FINISH_TIME_TO ?? 0));

            //danh sách các vai trò thực hiện PTTT
            for (int i = 0; i < listHisExecuteRole.Count; i++)
            {
                dicSingleTag.Add(string.Format("EXECUTE_ROLE_NAME_{0}", i + 1), listHisExecuteRole[i].EXECUTE_ROLE_NAME);
            }

            //danh sách các vai trò thực tế thực hiện PTTT
            for (int i = 0; i < listHisExecuteRoleReal.Count; i++)
            {
                dicSingleTag.Add(string.Format("EXECUTE_ROLE_NAME__REAL_{0}", i + 1), listHisExecuteRoleReal[i].EXECUTE_ROLE_NAME);
            }

            if (((Mrs00040Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00040Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }

            if (((Mrs00040Filter)this.reportFilter).REQUEST_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00040Filter)this.reportFilter).REQUEST_DEPARTMENT_ID ?? 0);
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
            if (castFilter.IS_COOK_MEDICINE.HasValue && castFilter.IS_COOK_MEDICINE == 0)
            {
                ListRdo = ListRdo.Where(x => x.TDL_SERVICE_NAME.ToLower().Contains("sắc thuốc thang")).ToList();
            }
            else if (castFilter.IS_COOK_MEDICINE.HasValue && castFilter.IS_COOK_MEDICINE == 1)
            {
                ListRdo = ListRdo.Where(x => !x.TDL_SERVICE_NAME.ToLower().Contains("sắc thuốc thang")).ToList();
            }
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.TDL_INTRUCTION_TIME).ToList());
            objectTag.AddObjectData(store, "Treatment", ListRdo.OrderBy(o => o.TDL_INTRUCTION_TIME).GroupBy(p => p.TREATMENT_CODE).Select(q => q.First()).ToList());
            objectTag.AddObjectData(store, "TreatmentService", ListRdoTreatmentService.OrderBy(o => o.TDL_INTRUCTION_TIME).GroupBy(p => p.TREATMENT_CODE).Select(q => q.First()).ToList());
            objectTag.AddRelationship(store, "Treatment", "Report", "TREATMENT_CODE", "TREATMENT_CODE");
            objectTag.AddRelationship(store, "TreatmentService", "Report", "TREATMENT_CODE", "TREATMENT_CODE");
            objectTag.AddRelationship(store, "Treatment", "TreatmentService", "TREATMENT_CODE", "TREATMENT_CODE");
            objectTag.AddObjectData(store, "ExecuteDepartment", ListRdo.OrderBy(o => o.TDL_INTRUCTION_TIME).GroupBy(g => g.TDL_EXECUTE_DEPARTMENT_ID).Select(s => s.First()).ToList());
            objectTag.AddRelationship(store, "ExecuteDepartment", "Report", "TDL_EXECUTE_DEPARTMENT_ID", "TDL_EXECUTE_DEPARTMENT_ID");
            objectTag.SetUserFunction(store, "Element", new RDOElement());
            objectTag.SetUserFunction(store, "fSearch", new fSearch());

            objectTag.AddObjectData(store, "DT", ListRdoDT.Where(p => p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT || p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__DV).ToList());
            objectTag.AddObjectData(store, "PTTTType", ListRdoPTTTGroup.Where(p => p.PTTT_GROUP_ID != null).ToList());
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
}
