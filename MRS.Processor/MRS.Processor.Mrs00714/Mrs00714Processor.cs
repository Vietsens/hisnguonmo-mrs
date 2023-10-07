using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisEkip;
using MOS.MANAGER.HisExecuteRole;
using MOS.MANAGER.HisEkipUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDeathWithin;
using MOS.MANAGER.HisPtttCatastrophe;
using MOS.MANAGER.HisPtttCondition;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisReportTypeCat;

namespace MRS.Processor.Mrs00714
{
    public class Mrs00714Processor : AbstractProcessor
    {
        private List<Mrs00714RDO> listRdo = new List<Mrs00714RDO>();
        //List<V_HIS_EKIP_USER> ListEkipUserAll = new List<V_HIS_EKIP_USER>();
        //List<EKIP_USER_CFG> ListEkipUserCfg = new List<EKIP_USER_CFG>();
        Mrs00714Filter filter;
        List<HIS_DEATH_WITHIN> listHisDeathWithin = new List<HIS_DEATH_WITHIN>();
        List<HIS_PTTT_CATASTROPHE> listHisPtttCatastrophe = new List<HIS_PTTT_CATASTROPHE>();
        List<HIS_PTTT_CONDITION> listHisPtttCondition = new List<HIS_PTTT_CONDITION>();

        List<SS_USER_REMUNERATION> ListSsUserRemuneration = new List<SS_USER_REMUNERATION>();

        List<HIS_REPORT_TYPE_CAT> listReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();

        List<USER_COUNT_PTTT> listUserPttt = new List<USER_COUNT_PTTT>();

        public Mrs00714Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00714Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00714Filter)reportFilter);
            var result = true;
            try
            {
                listRdo = new ManagerSql().GetRdo(filter);

                ListSsUserRemuneration = new ManagerSql().GetRemuneration(filter);

                HisDeathWithinFilterQuery deathWithinFilter = new HisDeathWithinFilterQuery();
                listHisDeathWithin = new HisDeathWithinManager().Get(deathWithinFilter);

                HisReportTypeCatFilterQuery HisReportTypeCatfilter = new HisReportTypeCatFilterQuery();
                HisReportTypeCatfilter.REPORT_TYPE_CODE__EXACT = this.reportType.REPORT_TYPE_CODE;
                listReportTypeCat = new HisReportTypeCatManager().Get(HisReportTypeCatfilter) ?? new List<HIS_REPORT_TYPE_CAT>();
                listReportTypeCat = listReportTypeCat.Where(o => o.REPORT_TYPE_CODE == "MRS00714").ToList();
                if (listReportTypeCat != null && listReportTypeCat.Count > 0)
                {
                    HisServiceRetyCatViewFilterQuery HisServiceRetyCatfilter = new HisServiceRetyCatViewFilterQuery();
                    HisServiceRetyCatfilter.REPORT_TYPE_CAT_IDs = listReportTypeCat.Select(o => o.ID).ToList();
                    listServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatfilter) ?? new List<V_HIS_SERVICE_RETY_CAT>();
                }
                listServiceRetyCat = listServiceRetyCat.Where(o => listReportTypeCat.Exists(p => p.ID == o.REPORT_TYPE_CAT_ID)).ToList();

                HisPtttConditionFilterQuery ConditionFilter = new HisPtttConditionFilterQuery();
                listHisPtttCondition = new HisPtttConditionManager().Get(ConditionFilter);
                HisPtttCatastropheFilterQuery CatastropheFilter = new HisPtttCatastropheFilterQuery();
                listHisPtttCatastrophe = new HisPtttCatastropheManager().Get(CatastropheFilter);
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
            var result = true;
            try
            {
                CountPtttByUser();
                if (IsNotNullOrEmpty(listRdo))
                {
                    Dictionary<long, HIS_DEPARTMENT> dicDepa = HisDepartmentCFG.DEPARTMENTs.ToDictionary(o => o.ID);
                    Dictionary<long, V_HIS_ROOM> dicRoom = HisRoomCFG.HisRooms.ToDictionary(o => o.ID);
                    Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceRetyCat = this.listServiceRetyCat.GroupBy(g => g.SERVICE_ID).ToDictionary(o => o.Key, p => p.First());
                    Dictionary<long, HIS_PATIENT_TYPE> dicPatientType = HisPatientTypeCFG.PATIENT_TYPEs.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_PTTT_METHOD> dicPtttMethod = HisPtttMethodCFG.PTTT_METHODs.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_PTTT_GROUP> dicPtttGroup = HisPtttGroupCFG.PTTT_GROUPs.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_DEATH_WITHIN> dicDeathWithin = this.listHisDeathWithin.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_EMOTIONLESS_METHOD> dicEmotionlessMethod = HisEmotionlessMethodCFG.EMOTIONLESS_METHODs.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_PTTT_CATASTROPHE> dicPtttCatastrophe = this.listHisPtttCatastrophe.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_PTTT_CONDITION> dicPtttCondition = this.listHisPtttCondition.ToDictionary(o => o.ID);
                    foreach (var rdo in listRdo)
                    {
                        if (dicDepa.ContainsKey(rdo.REQUEST_DEPARTMENT_ID ?? 0))
                        {
                            rdo.REQUEST_DEPARTMENT_NAME = dicDepa[rdo.REQUEST_DEPARTMENT_ID ?? 0].DEPARTMENT_NAME;
                        }
                        if (dicRoom.ContainsKey(rdo.REQUEST_ROOM_ID ?? 0))
                        {
                            rdo.REQUEST_ROOM_NAME = dicRoom[rdo.REQUEST_ROOM_ID ?? 0].ROOM_NAME;
                        }
                        if (dicDepa.ContainsKey(rdo.EXECUTE_DEPARTMENT_ID ?? 0))
                        {
                            rdo.EXECUTE_DEPARTMENT_NAME = dicDepa[rdo.EXECUTE_DEPARTMENT_ID ?? 0].DEPARTMENT_NAME;
                        }
                        if (dicRoom.ContainsKey(rdo.EXECUTE_ROOM_ID ?? 0))
                        {
                            rdo.EXECUTE_ROOM_NAME = dicRoom[rdo.EXECUTE_ROOM_ID ?? 0].ROOM_NAME;
                        }
                        if (dicServiceRetyCat.ContainsKey(rdo.SERVICE_ID))
                        {
                            rdo.CATEGORY_CODE = dicServiceRetyCat[rdo.SERVICE_ID].CATEGORY_CODE;
                            rdo.CATEGORY_NAME = dicServiceRetyCat[rdo.SERVICE_ID].CATEGORY_NAME;
                        }
                        if (dicPatientType.ContainsKey(rdo.PATIENT_TYPE_ID ?? 0))
                        {
                            rdo.PATIENT_TYPE_NAME = dicPatientType[rdo.PATIENT_TYPE_ID ?? 0].PATIENT_TYPE_NAME;
                        }

                        rdo.HAS_PARRENT = rdo.PARENT_ID != null ? "Phụ" : "Chính";

                        if (dicPtttMethod.ContainsKey(rdo.PTTT_METHOD_ID ?? 0))
                        {
                            rdo.PTTT_METHOL_NAME = dicPtttMethod[rdo.PTTT_METHOD_ID ?? 0].PTTT_METHOD_NAME;
                        }

                        if (dicPtttGroup.ContainsKey(rdo.PTTT_GROUP_ID ?? 0))
                        {
                            rdo.PTTT_GROUP_CODE = dicPtttGroup[rdo.PTTT_GROUP_ID ?? 0].PTTT_GROUP_CODE;
                            rdo.PTTT_GROUP_NAME = dicPtttGroup[rdo.PTTT_GROUP_ID ?? 0].PTTT_GROUP_NAME;
                        }

                        if (dicPtttGroup.ContainsKey(rdo.SV_PTTT_GROUP_ID ?? 0))
                        {
                            rdo.SV_PTTT_GROUP_CODE = dicPtttGroup[rdo.SV_PTTT_GROUP_ID ?? 0].PTTT_GROUP_CODE;
                            rdo.SV_PTTT_GROUP_NAME = dicPtttGroup[rdo.SV_PTTT_GROUP_ID ?? 0].PTTT_GROUP_NAME;
                        }

                        if (dicDeathWithin.ContainsKey(rdo.DEATH_WITHIN_ID ?? 0))
                        {
                            rdo.DEATH_WITHIN_NAME = dicDeathWithin[rdo.DEATH_WITHIN_ID ?? 0].DEATH_WITHIN_NAME;
                        }

                        if (dicEmotionlessMethod.ContainsKey(rdo.EMOTIONLESS_METHOD_ID ?? 0))
                        {
                            rdo.EMOTIONLESS_METHOD_NAME = dicEmotionlessMethod[rdo.EMOTIONLESS_METHOD_ID ?? 0].EMOTIONLESS_METHOD_NAME;
                        }

                        if (dicPtttCatastrophe.ContainsKey(rdo.PTTT_CATASTROPHE_ID ?? 0))
                        {
                            rdo.PTTT_CATASTROPHE_NAME = dicPtttCatastrophe[rdo.PTTT_CATASTROPHE_ID ?? 0].PTTT_CATASTROPHE_NAME;
                        }

                        if (dicPtttCondition.ContainsKey(rdo.PTTT_CONDITION_ID ?? 0))
                        {
                            rdo.PTTT_CONDITION_NAME = dicPtttCondition[rdo.PTTT_CONDITION_ID ?? 0].PTTT_CONDITION_NAME;
                        }

                        if (dicPtttMethod.ContainsKey(rdo.REAL_PTTT_METHOD_ID ?? 0))
                        {
                            rdo.REAL_PTTT_METHOD_NAME = dicPtttMethod[rdo.REAL_PTTT_METHOD_ID ?? 0].PTTT_METHOD_NAME;
                        }

                        rdo.START_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.L_START_TIME ?? 0);
                        rdo.PTTT_NUM_ORDER = rdo.PARENT_ID != null ? 2 : 1;
                        decimal tyle = 0;
                        if (rdo.ORIGINAL_PRICE > 0)
                        {
                            tyle = rdo.HEIN_LIMIT_PRICE.HasValue ? (rdo.HEIN_LIMIT_PRICE.Value / (rdo.ORIGINAL_PRICE * (1 + rdo.VAT_RATIO))) * 100 : (rdo.PRICE / rdo.ORIGINAL_PRICE) * 100;
                        }

                        rdo.BHYT_RATIO = tyle;
                        rdo.EKIP_ID = rdo.EKIP_ID ?? 0;
                        rdo.AGE = Inventec.Common.DateTime.Calculation.Age(rdo.DOB ?? 0);
                        rdo.BEGIN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.L_BEGIN_TIME ?? 0);
                        rdo.IN_CODE = rdo.IN_CODE;

                        if (dicPatientType.ContainsKey(rdo.TDL_PATIENT_TYPE_ID ?? 0))
                        {
                            rdo.TREATMENT_PATIENT_TYPE_NAME = dicPatientType[rdo.TDL_PATIENT_TYPE_ID ?? 0].PATIENT_TYPE_NAME;
                        }

                        rdo.BEGIN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.L_BEGIN_TIME ?? 0);
                        rdo.END_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.L_END_TIME ?? 0);
                    }

                    Inventec.Common.Logging.LogSystem.Info("ket thuc so lan pttt:" + listRdo.Count);

                }
                if (IsNotNullOrEmpty(ListSsUserRemuneration))
                {
                    Dictionary<long, HIS_DEPARTMENT> dicDepa = HisDepartmentCFG.DEPARTMENTs.ToDictionary(o => o.ID);
                    Dictionary<long, V_HIS_ROOM> dicRoom = HisRoomCFG.HisRooms.ToDictionary(o => o.ID);
                    Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceRetyCat = this.listServiceRetyCat.GroupBy(g => g.SERVICE_ID).ToDictionary(o => o.Key, p => p.First());
                    Dictionary<long, HIS_PATIENT_TYPE> dicPatientType = HisPatientTypeCFG.PATIENT_TYPEs.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_PTTT_METHOD> dicPtttMethod = HisPtttMethodCFG.PTTT_METHODs.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_PTTT_GROUP> dicPtttGroup = HisPtttGroupCFG.PTTT_GROUPs.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_DEATH_WITHIN> dicDeathWithin = this.listHisDeathWithin.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_EMOTIONLESS_METHOD> dicEmotionlessMethod = HisEmotionlessMethodCFG.EMOTIONLESS_METHODs.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_PTTT_CATASTROPHE> dicPtttCatastrophe = this.listHisPtttCatastrophe.ToDictionary(o => o.ID);
                    Dictionary<long, HIS_PTTT_CONDITION> dicPtttCondition = this.listHisPtttCondition.ToDictionary(o => o.ID);
                    foreach (var rdo in ListSsUserRemuneration)
                    {
                        if (dicDepa.ContainsKey(rdo.REQUEST_DEPARTMENT_ID ?? 0))
                        {
                            rdo.REQUEST_DEPARTMENT_NAME = dicDepa[rdo.REQUEST_DEPARTMENT_ID ?? 0].DEPARTMENT_NAME;
                        }
                        if (dicRoom.ContainsKey(rdo.REQUEST_ROOM_ID ?? 0))
                        {
                            rdo.REQUEST_ROOM_NAME = dicRoom[rdo.REQUEST_ROOM_ID ?? 0].ROOM_NAME;
                        }
                        if (dicDepa.ContainsKey(rdo.EXECUTE_DEPARTMENT_ID ?? 0))
                        {
                            rdo.EXECUTE_DEPARTMENT_NAME = dicDepa[rdo.EXECUTE_DEPARTMENT_ID ?? 0].DEPARTMENT_NAME;
                        }
                        if (dicRoom.ContainsKey(rdo.EXECUTE_ROOM_ID ?? 0))
                        {
                            rdo.EXECUTE_ROOM_NAME = dicRoom[rdo.EXECUTE_ROOM_ID ?? 0].ROOM_NAME;
                        }
                        if (dicPtttGroup.ContainsKey(rdo.PTTT_GROUP_ID ?? 0))
                        {
                            rdo.PTTT_GROUP_CODE = dicPtttGroup[rdo.PTTT_GROUP_ID ?? 0].PTTT_GROUP_CODE;
                            rdo.PTTT_GROUP_NAME = dicPtttGroup[rdo.PTTT_GROUP_ID ?? 0].PTTT_GROUP_NAME;
                        }

                        if (dicPtttGroup.ContainsKey(rdo.SV_PTTT_GROUP_ID ?? 0))
                        {
                            rdo.SV_PTTT_GROUP_CODE = dicPtttGroup[rdo.SV_PTTT_GROUP_ID ?? 0].PTTT_GROUP_CODE;
                            rdo.SV_PTTT_GROUP_NAME = dicPtttGroup[rdo.SV_PTTT_GROUP_ID ?? 0].PTTT_GROUP_NAME;
                        }
                        

                    }

                    Inventec.Common.Logging.LogSystem.Info("ket thuc so lan tinh cong:" + ListSsUserRemuneration.Count);

                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }
        private void CountPtttByUser() {
            List<USER_COUNT_PTTT> list = new List<USER_COUNT_PTTT>();
            list = new ManagerSql().GetUserPttt(filter);
            //var group = list.GroupBy(x => new { x.USER_NAME, x.EXECUTE_ROLE_NAME ,x.PTTT_GROUP_ID}).ToList();
            foreach (var item in list)
            {
                USER_COUNT_PTTT rdo = new USER_COUNT_PTTT();
                rdo.EXECUTE_USERNAME = item.USER_NAME;
                rdo.EXECUTE_LOGINNAME = item.LOGINNAME;
                if (item.EXECUTE_ROLE_NAME.Contains("Phẫu thuật viên phụ") || item.EXECUTE_ROLE_NAME.Contains("Thủ thuật phụ") || filter.ADD_SUPPORT_ROLEs != null && ("," + filter.ADD_SUPPORT_ROLEs + ",").Contains("," + item.EXECUTE_ROLE_CODE + ","))
                {
                    if (item.PTTT_GROUP_CODE == "T1" || item.PTTT_GROUP_CODE == "P1")
                    {
                        rdo.COUNT_PTTT_1_SUPPORT = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "T2" || item.PTTT_GROUP_CODE == "P2")
                    {
                        rdo.COUNT_PTTT_2_SUPPORT = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "T3" || item.PTTT_GROUP_CODE == "P3")
                    {
                        rdo.COUNT_PTTT_3_SUPPORT = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "TB" || item.PTTT_GROUP_CODE == "PB")
                    {
                        rdo.COUNT_PTTT_DB_SUPPORT = 1;
                    }
                    else
                    {
                        rdo.COUNT_PTTT_KHAC_SUPPORT = 1;
                    }
                }
                else if (item.EXECUTE_ROLE_NAME.Contains("Phẫu thuật viên chính") || item.EXECUTE_ROLE_NAME.Contains("Thủ thuật chính") || filter.ADD_MAIN_ROLEs != null && ("," + filter.ADD_MAIN_ROLEs + ",").Contains("," + item.EXECUTE_ROLE_CODE + ","))
                {
                    if (item.PTTT_GROUP_CODE == "T1" || item.PTTT_GROUP_CODE == "P1")
                    {
                        rdo.COUNT_PTTT_1_MAIN = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "T2" || item.PTTT_GROUP_CODE == "P2")
                    {
                        rdo.COUNT_PTTT_2_MAIN = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "T3" || item.PTTT_GROUP_CODE == "P3")
                    {
                        rdo.COUNT_PTTT_3_MAIN = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "TB" || item.PTTT_GROUP_CODE == "PB")
                    {
                        rdo.COUNT_PTTT_DB_MAIN = 1;
                    }
                    else
                    {
                        rdo.COUNT_PTTT_KHAC_MAIN = 1;
                    }
                }
                else if (item.EXECUTE_ROLE_NAME.Contains("Người giúp việc") || filter.ADD_HELPER_ROLEs != null && ("," + filter.ADD_HELPER_ROLEs + ",").Contains("," + item.EXECUTE_ROLE_CODE + ","))
                {
                    if (item.PTTT_GROUP_CODE == "T1" || item.PTTT_GROUP_CODE == "P1")
                    {
                        rdo.COUNT_PTTT_1_HELPER = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "T2" || item.PTTT_GROUP_CODE == "P2")
                    {
                        rdo.COUNT_PTTT_2_HELPER = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "T3" || item.PTTT_GROUP_CODE == "P3")
                    {
                        rdo.COUNT_PTTT_3_HELPER = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "TB" || item.PTTT_GROUP_CODE == "PB")
                    {
                        rdo.COUNT_PTTT_DB_HELPER = 1;
                    }
                    else
                    {
                        rdo.COUNT_PTTT_KHAC_HELPER = 1;
                    }
                }
                else
                {
                    if (item.PTTT_GROUP_CODE == "T1" || item.PTTT_GROUP_CODE == "P1")
                    {
                        rdo.COUNT_PTTT_1_KHAC = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "T2" || item.PTTT_GROUP_CODE == "P2")
                    {
                        rdo.COUNT_PTTT_2_KHAC = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "T3" || item.PTTT_GROUP_CODE == "P3")
                    {
                        rdo.COUNT_PTTT_3_KHAC = 1;
                    }
                    else if (item.PTTT_GROUP_CODE == "TB" || item.PTTT_GROUP_CODE == "PB")
                    {
                        rdo.COUNT_PTTT_DB_KHAC = 1;
                    }
                    else
                    {
                        rdo.COUNT_PTTT_KHAC_KHAC = 1;
                    }
                }
                listUserPttt.Add(rdo);
                var group = listUserPttt.GroupBy(x => x.EXECUTE_LOGINNAME).ToList();
                listUserPttt.Clear();
                foreach (var gr in group)
                {
                    USER_COUNT_PTTT r = new USER_COUNT_PTTT();
                    r.EXECUTE_LOGINNAME = gr.First().EXECUTE_LOGINNAME;
                    r.EXECUTE_USERNAME = gr.First().EXECUTE_USERNAME;
                    r.COUNT_PTTT_1_HELPER = gr.Sum(x => x.COUNT_PTTT_1_HELPER);
                    r.COUNT_PTTT_1_MAIN = gr.Sum(x => x.COUNT_PTTT_1_MAIN);
                    r.COUNT_PTTT_1_SUPPORT = gr.Sum(x => x.COUNT_PTTT_1_SUPPORT);
                    r.COUNT_PTTT_1_KHAC = gr.Sum(x => x.COUNT_PTTT_1_KHAC);
                    r.COUNT_PTTT_2_HELPER = gr.Sum(x => x.COUNT_PTTT_2_HELPER);
                    r.COUNT_PTTT_2_MAIN = gr.Sum(x => x.COUNT_PTTT_2_MAIN);
                    r.COUNT_PTTT_2_SUPPORT = gr.Sum(x => x.COUNT_PTTT_2_SUPPORT);
                    r.COUNT_PTTT_2_KHAC = gr.Sum(x => x.COUNT_PTTT_2_KHAC);
                    r.COUNT_PTTT_3_HELPER = gr.Sum(x => x.COUNT_PTTT_3_HELPER);
                    r.COUNT_PTTT_3_MAIN = gr.Sum(x => x.COUNT_PTTT_3_MAIN);
                    r.COUNT_PTTT_3_SUPPORT = gr.Sum(x => x.COUNT_PTTT_3_SUPPORT);
                    r.COUNT_PTTT_3_KHAC = gr.Sum(x => x.COUNT_PTTT_3_KHAC);
                    r.COUNT_PTTT_DB_HELPER = gr.Sum(x => x.COUNT_PTTT_DB_HELPER);
                    r.COUNT_PTTT_DB_MAIN = gr.Sum(x => x.COUNT_PTTT_DB_MAIN);
                    r.COUNT_PTTT_DB_SUPPORT = gr.Sum(x => x.COUNT_PTTT_DB_SUPPORT);
                    r.COUNT_PTTT_DB_KHAC = gr.Sum(x => x.COUNT_PTTT_DB_KHAC);
                    r.COUNT_PTTT_KHAC_HELPER = gr.Sum(x => x.COUNT_PTTT_KHAC_HELPER);
                    r.COUNT_PTTT_KHAC_MAIN = gr.Sum(x => x.COUNT_PTTT_KHAC_MAIN);
                    r.COUNT_PTTT_KHAC_SUPPORT = gr.Sum(x => x.COUNT_PTTT_KHAC_SUPPORT);
                    r.COUNT_PTTT_KHAC_KHAC = gr.Sum(x => x.COUNT_PTTT_KHAC_KHAC);
                    listUserPttt.Add(r);
                }
            }
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00714Filter)this.reportFilter).TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00714Filter)this.reportFilter).TIME_TO));

                //lọc lại danh sách tổng theo danh sách kíp
                var ssIds = ListSsUserRemuneration.Select(o => o.SERE_SERV_ID).Distinct().ToList();
                listRdo = listRdo.Where(o => ssIds.Contains(o.ID??0)).ToList();
                if (IsNotNullOrEmpty(filter.EXECUTE_DEPARTMENT_IDs))
                {
                    var depaGroup = ListSsUserRemuneration.GroupBy(o => o.EXECUTE_DEPARTMENT_ID).Select(p => p.First()).ToList();
                    dicSingleTag.Add("TOTAL_EXECUTE_DEPARTMENT_NAME", string.Join(";", depaGroup.OrderBy(o => o.EXECUTE_DEPARTMENT_NAME)));
                }
                //báo cáo tổng hợp pttt theo bác sỹ
                objectTag.AddObjectData(store,"ExecuteRoleCount", listUserPttt);



                var listUser = ListSsUserRemuneration.GroupBy(o => o.LOGINNAME).Select(p => p.First()).OrderBy(o => o.LOGINNAME).ToList();
                var listExecuteRole = ListSsUserRemuneration.GroupBy(o => o.EXECUTE_ROLE_CODE).Select(p => p.First()).OrderBy(o => o.EXECUTE_ROLE_CODE).ToList();
                var PtttGroups = listRdo.GroupBy(o => o.PTTT_GROUP_CODE).Select(p => p.First()).OrderBy(o => o.PTTT_GROUP_CODE).ToList();
                var PtGroups = PtttGroups.Where(o => o.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).ToList();
                var TtGroups = PtttGroups.Where(o => o.SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).ToList();
                objectTag.AddObjectData(store, "User", listUser);
                objectTag.AddObjectData(store, "ExecuteRole", listExecuteRole);
                //loại dịch vụ
                objectTag.AddObjectData(store, "Types", listRdo.GroupBy(o => new { o.SERVICE_TYPE_ID }).Select(p => p.First()).ToList());
                for (int i = 0; i < 1000; i++)
                {
                    if (i < listUser.Count)
                    {
                        dicSingleTag.Add(string.Format("LOGINNAME_{0}", i + 1), listUser[i].LOGINNAME);
                        dicSingleTag.Add(string.Format("USERNAME_{0}", i + 1), listUser[i].LOGINNAME);
                    }
                    if (i < listExecuteRole.Count)
                    {
                        dicSingleTag.Add(string.Format("EXECUTE_ROLE_CODE_{0}", i + 1), listExecuteRole[i].EXECUTE_ROLE_CODE);
                        dicSingleTag.Add(string.Format("EXECUTE_ROLE_NAME_{0}", i + 1), listExecuteRole[i].EXECUTE_ROLE_NAME);
                    }
                    if (i < PtttGroups.Count)
                    {
                        dicSingleTag.Add(string.Format("PTTT_GROUP_CODE_{0}", i + 1), PtttGroups[i].PTTT_GROUP_CODE);
                        dicSingleTag.Add(string.Format("PTTT_GROUP_NAME_{0}", i + 1), PtttGroups[i].PTTT_GROUP_NAME);
                    }
                    if (i < PtGroups.Count)
                    {
                        dicSingleTag.Add(string.Format("PT_GROUP_CODE_{0}", i + 1), PtGroups[i].PTTT_GROUP_CODE);
                        dicSingleTag.Add(string.Format("PT_GROUP_NAME_{0}", i + 1), PtGroups[i].PTTT_GROUP_NAME);
                    }
                    if (i < TtGroups.Count)
                    {
                        dicSingleTag.Add(string.Format("TT_GROUP_CODE_{0}", i + 1), TtGroups[i].PTTT_GROUP_CODE);
                        dicSingleTag.Add(string.Format("TT_GROUP_NAME_{0}", i + 1), TtGroups[i].PTTT_GROUP_NAME);
                    }
                }

                objectTag.AddObjectData(store, "Report", listRdo);
                objectTag.AddObjectData(store, "Remu", ListSsUserRemuneration);
                objectTag.AddRelationship(store, "Report", "Remu", "ID", "SERE_SERV_ID");
                objectTag.AddRelationship(store, "Remu", "Report", "SERE_SERV_ID", "ID");
                objectTag.AddObjectData(store, "ExecuteRooms", listRdo.GroupBy(o => o.EXECUTE_ROOM_ID).Select(p => p.First()).ToList());
                //mẫu 1,2,5:
                //vai tro
                objectTag.AddObjectData(store, "RoleUserTypes", ListSsUserRemuneration.GroupBy(o => new { o.EXECUTE_ROLE_CODE, o.LOGINNAME, o.SERVICE_TYPE_ID }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "RoleUserTypes", "Remu", new string[] { "EXECUTE_ROLE_CODE", "LOGINNAME", "SERVICE_TYPE_ID" }, new string[] { "EXECUTE_ROLE_CODE", "LOGINNAME", "SERVICE_TYPE_ID" });
                //nhanvien
                objectTag.AddObjectData(store, "UserTypes", ListSsUserRemuneration.GroupBy(o => new { o.LOGINNAME, o.SERVICE_TYPE_ID }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "UserTypes", "RoleUserTypes", new string[] { "LOGINNAME", "SERVICE_TYPE_ID" }, new string[] { "LOGINNAME", "SERVICE_TYPE_ID" });
                //Loaidichvu
                objectTag.AddRelationship(store, "Types", "UserTypes", new string[] { "SERVICE_TYPE_ID" }, new string[] { "SERVICE_TYPE_ID" });
                //mẫu 3:
                //vai tro
                objectTag.AddObjectData(store, "RoleTypeUsers", ListSsUserRemuneration.GroupBy(o => new { o.EXECUTE_ROLE_CODE, o.SERVICE_TYPE_ID, o.LOGINNAME }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "RoleTypeUsers", "Remu", new string[] { "EXECUTE_ROLE_CODE", "SERVICE_TYPE_ID", "LOGINNAME" }, new string[] { "EXECUTE_ROLE_CODE", "SERVICE_TYPE_ID", "LOGINNAME" });
                //loaidichvu
                objectTag.AddObjectData(store, "TypeUsers", ListSsUserRemuneration.GroupBy(o => new { o.SERVICE_TYPE_ID, o.LOGINNAME }).Select(p => p.First()).ToList());//<#UserTypes.USER_NAME;>
                objectTag.AddRelationship(store, "TypeUsers", "RoleTypeUsers", new string[] { "SERVICE_TYPE_ID", "LOGINNAME" }, new string[] { "SERVICE_TYPE_ID", "LOGINNAME" });
                //nhanvien
                objectTag.AddObjectData(store, "Users", ListSsUserRemuneration.GroupBy(o => new { o.LOGINNAME }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "Users", "TypeUsers", new string[] { "LOGINNAME" }, new string[] { "LOGINNAME" });

                //mẫu 4:
                objectTag.AddRelationship(store, "UserTypes", "Remu", new string[] { "LOGINNAME", "SERVICE_TYPE_ID" }, new string[] { "LOGINNAME", "SERVICE_TYPE_ID" });
                //mẫu 6:
                //dịch vụ
                objectTag.AddObjectData(store, "SvGroupTypes", listRdo.GroupBy(o => new { o.SERVICE_ID, o.PTTT_GROUP_ID, o.SERVICE_TYPE_ID }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "SvGroupTypes", "Report", new string[] { "SERVICE_ID", "PTTT_GROUP_ID", "SERVICE_TYPE_ID" }, new string[] { "SERVICE_ID", "PTTT_GROUP_ID", "SERVICE_TYPE_ID" });
                //loại PTTT
                objectTag.AddObjectData(store, "GroupTypes", listRdo.GroupBy(o => new { o.PTTT_GROUP_ID, o.SERVICE_TYPE_ID }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "GroupTypes", "SvGroupTypes", new string[] { "PTTT_GROUP_ID", "SERVICE_TYPE_ID" }, new string[] { "PTTT_GROUP_ID", "SERVICE_TYPE_ID" });
                //loại dịch vụ
                objectTag.AddRelationship(store, "Types", "GroupTypes", new string[] { "SERVICE_TYPE_ID" }, new string[] { "SERVICE_TYPE_ID" });

                //dich vu
                objectTag.AddObjectData(store, "Services", listRdo.GroupBy(o => new { o.SERVICE_CODE, o.PTTT_GROUP_CODE, o.REQUEST_ROOM_ID, o.SERVICE_TYPE_CODE }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "Services", "Report", new string[] { "SERVICE_CODE", "PTTT_GROUP_CODE", "REQUEST_ROOM_ID", "SERVICE_TYPE_CODE" }, new string[] { "SERVICE_CODE", "PTTT_GROUP_CODE", "REQUEST_ROOM_ID", "SERVICE_TYPE_CODE" });

                //loai PTTT
                objectTag.AddObjectData(store, "PtttGroups", listRdo.GroupBy(o => new { o.PTTT_GROUP_CODE, o.REQUEST_ROOM_ID, o.SERVICE_TYPE_CODE }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "PtttGroups", "Services", new string[] { "PTTT_GROUP_CODE", "REQUEST_ROOM_ID", "SERVICE_TYPE_CODE" }, new string[] { "PTTT_GROUP_CODE", "REQUEST_ROOM_ID", "SERVICE_TYPE_CODE" });

                //phong chi dinh
                objectTag.AddObjectData(store, "RequestRooms", listRdo.GroupBy(o => new { o.REQUEST_ROOM_ID, o.SERVICE_TYPE_CODE }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "RequestRooms", "PtttGroups", new string[] { "REQUEST_ROOM_ID", "SERVICE_TYPE_CODE" }, new string[] { "REQUEST_ROOM_ID", "SERVICE_TYPE_CODE" });

                //Loai dich vu
                objectTag.AddObjectData(store, "ServiceTypes", listRdo.GroupBy(o => new { o.SERVICE_TYPE_CODE }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "ServiceTypes", "RequestRooms", new string[] { "SERVICE_TYPE_CODE" }, new string[] { "SERVICE_TYPE_CODE" });

                //mẫu 9:
                //nhan vien
                objectTag.AddObjectData(store, "UserDepaTypes", ListSsUserRemuneration.GroupBy(o => new { o.LOGINNAME, o.EXECUTE_DEPARTMENT_ID, o.SERVICE_TYPE_ID }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "UserDepaTypes", "Remu", new string[] { "LOGINNAME", "EXECUTE_DEPARTMENT_ID", "SERVICE_TYPE_ID" }, new string[] { "LOGINNAME", "EXECUTE_DEPARTMENT_ID", "SERVICE_TYPE_ID" });
                //nhanvien
                objectTag.AddObjectData(store, "DepaTypes", ListSsUserRemuneration.GroupBy(o => new { o.EXECUTE_DEPARTMENT_ID, o.SERVICE_TYPE_ID }).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "DepaTypes", "UserDepaTypes", new string[] { "EXECUTE_DEPARTMENT_ID", "SERVICE_TYPE_ID" }, new string[] { "EXECUTE_DEPARTMENT_ID", "SERVICE_TYPE_ID" });
                //Loaidichvu
                objectTag.AddRelationship(store, "Types", "DepaTypes", new string[] { "SERVICE_TYPE_ID" }, new string[] { "SERVICE_TYPE_ID" });
                //mẫu 15:
                //Khoa
                objectTag.AddObjectData(store, "Depas", ListSsUserRemuneration.GroupBy(o => o.EXECUTE_DEPARTMENT_ID).Select(p => p.First()).ToList());
                //khoaUser
                objectTag.AddObjectData(store, "DepaUsers", ListSsUserRemuneration.GroupBy(o => new { o.EXECUTE_DEPARTMENT_ID, o.LOGINNAME }).Select(p => p.First()).ToList());
                //khoaUserRoleGroup
                objectTag.AddObjectData(store, "DepaUserRoleGroups", ListSsUserRemuneration.GroupBy(o => new { o.EXECUTE_DEPARTMENT_ID, o.LOGINNAME, o.EXECUTE_ROLE_CODE, o.PTTT_GROUP_CODE }).Select(p => p.First()).ToList());

                objectTag.AddRelationship(store, "Depas", "DepaUsers", new string[] { "EXECUTE_DEPARTMENT_ID" }, new string[] { "EXECUTE_DEPARTMENT_ID" });
                objectTag.AddRelationship(store, "DepaUsers", "DepaUserRoleGroups", new string[] { "LOGINNAME", "EXECUTE_DEPARTMENT_ID" }, new string[] { "LOGINNAME", "EXECUTE_DEPARTMENT_ID" });
                objectTag.AddRelationship(store, "DepaUserRoleGroups", "Report", new string[] { "EXECUTE_ROLE_CODE", "PTTT_GROUP_CODE", "LOGINNAME", "EXECUTE_DEPARTMENT_ID" }, new string[] { "EXECUTE_ROLE_CODE", "PTTT_GROUP_CODE", "LOGINNAME", "EXECUTE_DEPARTMENT_ID" });
                //Group
                objectTag.AddObjectData(store, "Groups", listRdo.GroupBy(o => new { o.PTTT_GROUP_CODE }).Select(p => p.First()).ToList());

                var groupByService = listRdo.GroupBy(o => o.SERVICE_ID).Select(s => new Mrs00714RDO(s.ToList())).ToList();
                objectTag.AddObjectData(store, "GroupByService", groupByService.OrderByDescending(o => o.AMOUNT).ToList());

                objectTag.AddObjectData(store, "Month", ListSsUserRemuneration.GroupBy(o => o.INTRUCTION_MONTH).Select(p => p.First()).OrderBy(o => o.INTRUCTION_MONTH).ToList());

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
