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
using MOS.MANAGER.HisPtttPriority;

namespace MRS.Processor.Mrs00255
{
    public class Mrs00255Processor : AbstractProcessor
    {
        private List<Mrs00255RDO> listRdo = new List<Mrs00255RDO>();
        Dictionary<long, long?> dicRdo = new Dictionary<long, long?>();
        Dictionary<long, long?> dicParentRdo = new Dictionary<long, long?>();
        List<V_HIS_EKIP_USER> ListEkipUserAll = new List<V_HIS_EKIP_USER>();
        List<EKIP_USER_CFG> ListEkipUserCfg = new List<EKIP_USER_CFG>();
        Mrs00255Filter filter;
        List<MEMA_FOLLOW> ListMemaAgg = new List<MEMA_FOLLOW>();
        List<HIS_DEATH_WITHIN> listHisDeathWithin = new List<HIS_DEATH_WITHIN>();
        List<HIS_PTTT_CATASTROPHE> listHisPtttCatastrophe = new List<HIS_PTTT_CATASTROPHE>();
        List<HIS_PTTT_CONDITION> listHisPtttCondition = new List<HIS_PTTT_CONDITION>();
        List<MEMA_FOLLOW> listMemaDetail = new List<MEMA_FOLLOW>();
        List<HIS_PTTT_PRIORITY> listHisPtttPriority = new List<HIS_PTTT_PRIORITY>();
        List<HIS_REPORT_TYPE_CAT> listReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        List<HIS_SERVICE_RETY_CAT> listServiceRetyCat = new List<HIS_SERVICE_RETY_CAT>();
        List<PTTT_COUNT> listCount = new List<PTTT_COUNT>();
        string SERVICE_NAMEs = "";
        private List<Mrs00255RDO> listRdoCount = new List<Mrs00255RDO>();

        public Mrs00255Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00255Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00255Filter)reportFilter);
            var result = true;
            try
            {
                listRdo = new ManagerSql().GetRdo(filter);
                dicRdo = listRdo.GroupBy(o => o.ID ?? 0).ToDictionary(p => p.Key, q => q.First().EKIP_ID);
                dicParentRdo = listRdo.Where(o => o.PARENT_ID != null).GroupBy(o => o.PARENT_ID ?? 0).ToDictionary(p => p.Key, q => q.First().EKIP_ID);
                ListEkipUserCfg = ManagerSql.GetEkipUserCfg();
                CountPttt(listRdo);
                HisDeathWithinFilterQuery deathWithinFilter = new HisDeathWithinFilterQuery();
                listHisDeathWithin = new HisDeathWithinManager().Get(deathWithinFilter);

                HisReportTypeCatFilterQuery HisReportTypeCatfilter = new HisReportTypeCatFilterQuery();
                HisReportTypeCatfilter.REPORT_TYPE_CODE__EXACT = this.ReportTypeCode;
                listReportTypeCat = new HisReportTypeCatManager().Get(HisReportTypeCatfilter) ?? new List<HIS_REPORT_TYPE_CAT>();
                listReportTypeCat = listReportTypeCat.Where(o => o.REPORT_TYPE_CODE == "MRS00255").ToList();
                if (listReportTypeCat != null && listReportTypeCat.Count > 0)
                {
                    HisServiceRetyCatFilterQuery HisServiceRetyCatfilter = new HisServiceRetyCatFilterQuery();
                    HisServiceRetyCatfilter.REPORT_TYPE_CAT_IDs = listReportTypeCat.Select(o => o.ID).ToList();
                    listServiceRetyCat = new HisServiceRetyCatManager().Get(HisServiceRetyCatfilter) ?? new List<HIS_SERVICE_RETY_CAT>();
                }
                listServiceRetyCat = listServiceRetyCat.Where(o => listReportTypeCat.Exists(p => p.ID == o.REPORT_TYPE_CAT_ID)).ToList();

                HisPtttConditionFilterQuery ConditionFilter = new HisPtttConditionFilterQuery();
                listHisPtttCondition = new HisPtttConditionManager().Get(ConditionFilter);
                HisPtttCatastropheFilterQuery CatastropheFilter = new HisPtttCatastropheFilterQuery();
                listHisPtttCatastrophe = new HisPtttCatastropheManager().Get(CatastropheFilter);
                listHisPtttPriority = new HisPtttPriorityManager().Get(new HisPtttPriorityFilterQuery());
                if (filter.SERVICE_ID != null || filter.SERVICE_IDs != null)
                {
                    HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                    serviceFilter.IDs = filter.SERVICE_IDs;
                    serviceFilter.ID = filter.SERVICE_ID;
                    var services = new HisServiceManager().Get(serviceFilter);
                    if (services != null)
                    {
                        SERVICE_NAMEs = string.Join(",", services.Select(o => o.SERVICE_NAME).ToList());
                    }
                }
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
                if (IsNotNullOrEmpty(listRdo))
                {

                    var keyOrder = "";
                    //khi có điều kiện lọc từ template thì đổi sang key từ template
                    if (this.dicDataFilter.ContainsKey("KEY_ORDER") && this.dicDataFilter["KEY_ORDER"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_ORDER"].ToString()))
                    {
                        keyOrder = this.dicDataFilter["KEY_ORDER"].ToString();
                    }

                    int start = 0;
                    int count = listRdo.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);

                        List<Mrs00255RDO> listRdoLocal = listRdo.Skip(start).Take(limit).ToList();
                        long[] list = { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT };

                        HisSereServViewFilterQuery followFilter = new HisSereServViewFilterQuery();
                        followFilter.SERVICE_TYPE_IDs = list.ToList();
                        followFilter.TREATMENT_IDs = listRdoLocal.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                        var ListSereServ1 = new HisSereServManager().GetView(followFilter);

                        HisEkipUserViewFilterQuery ekipUserFilter = new HisEkipUserViewFilterQuery();
                        ekipUserFilter.EKIP_IDs = listRdoLocal.Select(s => s.EKIP_ID ?? 0).Distinct().ToList();
                        List<V_HIS_EKIP_USER> listEkipUserLocal = new HisEkipUserManager().GetView(ekipUserFilter) ?? new List<V_HIS_EKIP_USER>();
                        if (IsNotNullOrEmpty(listEkipUserLocal))
                        {
                            ListEkipUserAll.AddRange(listEkipUserLocal);
                        }

                        ////
                        long roleMain = HisExecuteRoleCFG.HisExecuteRoleId__Main;
                        long Anesthetist = HisExecuteRoleCFG.HisExecuteRoleId__Anesthetist;
                        long rolePM1 = HisExecuteRoleCFG.HisExecuteRoleId__PM1;
                        long rolePM2 = HisExecuteRoleCFG.HisExecuteRoleId__PM2;
                        long rolePMe1 = HisExecuteRoleCFG.HisExecuteRoleId__PMe1;
                        long rolePMe2 = HisExecuteRoleCFG.HisExecuteRoleId__PMe2;
                        long roleYTDD = HisExecuteRoleCFG.HisExecuteRoleId__YTDD;
                        long roleDCVPTTT = HisExecuteRoleCFG.HisExecuteRoleId__DCVPTTT;

                        foreach (var rdo in listRdoLocal)
                        {
                            Decimal followmedifee = 0;
                            Decimal followmedi = 0;
                            Decimal followmatefee = 0;
                            Decimal followmate = 0;
                            var ekipUser = listEkipUserLocal.Where(o => o.EKIP_ID == rdo.EKIP_ID).ToList();
                            List<string> mainUsernames = ekipUser.Where(o => o.EKIP_ID == rdo.EKIP_ID && o.IS_SURG_MAIN == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.USERNAME).Distinct().ToList();
                            List<string> mainLoginnames = ekipUser.Where(o => o.EKIP_ID == rdo.EKIP_ID && o.IS_SURG_MAIN == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.LOGINNAME).Distinct().ToList();
                            string mainLoginnameStr = string.Join(", ", mainLoginnames);
                            string mainUsernameStr = string.Join(", ", mainUsernames);
                            if (ListSereServ1.Where(o => o.PARENT_ID == rdo.ID).ToList().Count != 0)
                            {
                                var listfollow = ListSereServ1.Where(o => o.PARENT_ID == rdo.ID).ToList();

                                foreach (var medi in listfollow)
                                {
                                    if (medi.IS_EXPEND != null)
                                    {
                                        if (medi.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC)
                                        {
                                            followmedifee = followmedifee + (Decimal)medi.VIR_TOTAL_PRICE_NO_EXPEND;
                                        }
                                        else
                                        {
                                            followmatefee = followmatefee + (Decimal)medi.VIR_TOTAL_PRICE_NO_EXPEND;
                                        }
                                    }
                                    else
                                    {
                                        if (medi.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC)
                                        {
                                            followmedi = followmedi + (Decimal)medi.VIR_TOTAL_PRICE_NO_EXPEND;
                                        }
                                        else
                                        {
                                            followmate = followmate + (Decimal)medi.VIR_TOTAL_PRICE_NO_EXPEND;
                                        }
                                    }


                                    MEMA_FOLLOW mema = new MEMA_FOLLOW();
                                    var reqDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.REQUEST_DEPARTMENT_ID);
                                    if (reqDepartment != null)
                                    {
                                        mema.REQUEST_DEPARTMENT_CODE = reqDepartment.DEPARTMENT_CODE;
                                        mema.REQUEST_DEPARTMENT_NAME = reqDepartment.DEPARTMENT_NAME;
                                    }
                                    mema.MEMA_REQUEST_LOGINNAME = medi.TDL_REQUEST_LOGINNAME;
                                    mema.MEMA_REQUEST_USERNAME = medi.TDL_REQUEST_USERNAME;
                                    mema.SERVICE_CODE = rdo.SERVICE_CODE;
                                    mema.SERVICE_NAME = rdo.SERVICE_NAME;
                                    mema.AMOUNT = rdo.AMOUNT;
                                    mema.PARENT_IDs = string.Format("{0},", (rdo.ID ?? 0));
                                    mema.PARENT_ID = rdo.ID ?? 0;
                                    mema.VIR_PRICE_NO_EXPEND = medi.VIR_PRICE_NO_EXPEND ?? 0;
                                    mema.MEMA_TYPE = medi.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC ? "THUOC" : "VATTU";
                                    mema.TREATMENT_CODE = rdo.TREATMENT_CODE;
                                    mema.PATIENT_NAME = rdo.VIR_PATIENT_NAME;
                                    mema.MEMA_CODE = medi.TDL_SERVICE_CODE;
                                    mema.MEMA_NAME = medi.TDL_SERVICE_NAME;
                                    mema.MEMA_UNIT_CODE = medi.SERVICE_UNIT_CODE;
                                    mema.MEMA_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                                    mema.MEMA_AMOUNT = medi.AMOUNT;
                                    mema.MAIN_USERNAME = mainUsernameStr;
                                    mema.MAIN_LOGINNAME = mainLoginnameStr;
                                    listMemaDetail.Add(mema);
                                }
                            }


                            var requestDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.REQUEST_DEPARTMENT_ID);
                            if (requestDepartment != null)
                            {
                                rdo.REQUEST_DEPARTMENT_NAME = requestDepartment.DEPARTMENT_NAME;
                            }
                            var requestRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.REQUEST_ROOM_ID);
                            if (requestRoom != null)
                            {
                                rdo.REQUEST_ROOM_NAME = requestRoom.ROOM_NAME;
                            }
                            var executeDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.EXECUTE_DEPARTMENT_ID);
                            if (executeDepartment != null)
                            {
                                rdo.EXECUTE_DEPARTMENT_NAME = executeDepartment.DEPARTMENT_NAME;
                            }
                            var executeRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.EXECUTE_ROOM_ID);
                            if (executeRoom != null)
                            {
                                rdo.EXECUTE_ROOM_NAME = executeRoom.ROOM_NAME;
                            }

                            var serviceRetyCat = this.listServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == rdo.SERVICE_ID);
                            if (serviceRetyCat != null)
                            {
                                var reportTypeCat = this.listReportTypeCat.FirstOrDefault(o => o.ID == serviceRetyCat.REPORT_TYPE_CAT_ID);
                                if (reportTypeCat != null)
                                {
                                    rdo.CATEGORY_CODE = reportTypeCat.CATEGORY_CODE;
                                    rdo.CATEGORY_NAME = reportTypeCat.CATEGORY_NAME;
                                }
                            }
                            var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.PATIENT_TYPE_ID);
                            if (patientType != null)
                            {
                                rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                            }
                            var levelEkip = 3;
                            if (dicRdo.ContainsKey(rdo.PARENT_ID ?? -1) && dicRdo[rdo.PARENT_ID ?? -1] == rdo.EKIP_ID || dicParentRdo.ContainsKey(rdo.ID ?? -1) && dicParentRdo[rdo.ID ?? -1] == rdo.EKIP_ID)
                            {
                                levelEkip = 1;
                            }
                            else if (dicRdo.ContainsKey(rdo.PARENT_ID ?? -1) || dicParentRdo.ContainsKey(rdo.ID ?? -1))
                            {
                                levelEkip = 2;

                            }

                            rdo.KEY_ORDER = string.Format(keyOrder, levelEkip, rdo.TREATMENT_CODE);
                            rdo.HAS_PARRENT = rdo.PARENT_ID != null ? "Phụ" : "Chính";
                            rdo.IS_EKIP_AGAIN = rdo.EKIP_ID != null && dicRdo.ContainsKey(rdo.PARENT_ID ?? -1) && dicRdo[rdo.PARENT_ID ?? -1] == rdo.EKIP_ID ? 1 : 0;
                            rdo.MEDICINE_VIR_TOTAL_PRICE = followmedi;
                            rdo.MEDICINE_FEE_VIR_TOTAL_PRICE = followmedifee;
                            rdo.MATERIAL_VIR_TOTAL_PRICE = followmate;
                            rdo.MATERIAL_FEE_VIR_TOTAL_PRICE = followmatefee;

                            var ptttMethod = HisPtttMethodCFG.PTTT_METHODs.FirstOrDefault(o => o.ID == rdo.PTTT_METHOD_ID);
                            if (ptttMethod != null)
                            {
                                rdo.PTTT_METHOL_NAME = ptttMethod.PTTT_METHOD_NAME;
                            }
                            var ptttGroup = HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == rdo.PTTT_GROUP_ID);
                            if (ptttGroup != null)
                            {
                                rdo.PTTT_GROUP_CODE = ptttGroup.PTTT_GROUP_CODE;
                                rdo.PTTT_GROUP_NAME = ptttGroup.PTTT_GROUP_NAME;
                            }
                            else
                            {
                                rdo.PTTT_GROUP_NAME = "Khác";
                            }
                            var ptttGroupSv = HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == rdo.SV_PTTT_GROUP_ID);
                            if (ptttGroupSv != null)
                            {
                                rdo.SV_PTTT_GROUP_CODE = ptttGroupSv.PTTT_GROUP_CODE;
                                rdo.SV_PTTT_GROUP_NAME = ptttGroupSv.PTTT_GROUP_NAME;
                            }
                            else
                            {
                                rdo.SV_PTTT_GROUP_NAME = "Khác";
                            }
                            var ptttDeathWithin = this.listHisDeathWithin.FirstOrDefault(o => o.ID == rdo.DEATH_WITHIN_ID);
                            if (ptttDeathWithin != null)
                            {
                                rdo.DEATH_WITHIN_NAME = ptttDeathWithin.DEATH_WITHIN_NAME;
                            }
                            var EmotionlessMethod = HisEmotionlessMethodCFG.EMOTIONLESS_METHODs.FirstOrDefault(o => o.ID == rdo.EMOTIONLESS_METHOD_ID);
                            if (EmotionlessMethod != null)
                            {
                                rdo.EMOTIONLESS_METHOD_NAME = EmotionlessMethod.EMOTIONLESS_METHOD_NAME;
                            }
                            var ptttCatastrophe = this.listHisPtttCatastrophe.FirstOrDefault(o => o.ID == rdo.PTTT_CATASTROPHE_ID);
                            if (ptttCatastrophe != null)
                            {
                                rdo.PTTT_CATASTROPHE_NAME = ptttCatastrophe.PTTT_CATASTROPHE_NAME;
                            }

                            var ptttCondition = this.listHisPtttCondition.FirstOrDefault(o => o.ID == rdo.PTTT_CONDITION_ID);
                            if (ptttCondition != null)
                            {
                                rdo.PTTT_CONDITION_NAME = ptttCondition.PTTT_CONDITION_NAME;
                            }
                            var ptttPriority = listHisPtttPriority.Where(x => x.ID == rdo.PTTT_PRIORITY_ID).FirstOrDefault();
                            if (ptttPriority != null)
                            {
                                rdo.PTTT_PRIORITY_NAME = ptttPriority.PTTT_PRIORITY_NAME;
                                rdo.PTTT_PRIORITY_CODE = ptttPriority.PTTT_PRIORITY_CODE;
                            }
                            var realPtttMethod = HisPtttMethodCFG.PTTT_METHODs.FirstOrDefault(o => o.ID == rdo.REAL_PTTT_METHOD_ID);
                            if (realPtttMethod != null)
                            {
                                rdo.REAL_PTTT_METHOD_NAME = realPtttMethod.PTTT_METHOD_NAME;
                            }

                            if (rdo.EKIP_ID.HasValue && IsNotNullOrEmpty(ListEkipUserCfg))
                            {
                                foreach (var item in ekipUser)
                                {
                                    if (!item.REMUNERATION_PRICE.HasValue && item.EKIP_ID == rdo.EKIP_ID)
                                    {
                                        var cfg = ListEkipUserCfg.Where(o => o.EXECUTE_ROLE_ID == item.EXECUTE_ROLE_ID && o.PTTT_GROUP_ID == rdo.PTTT_GROUP_ID && o.SERVICE_TYPE_ID == rdo.SERVICE_TYPE_ID).ToList();
                                        if (cfg != null && cfg.Count > 0)
                                        {
                                            EKIP_USER_CFG pr = cfg.FirstOrDefault(o => o.EMOTIONLESS_METHOD_ID == rdo.EMOTIONLESS_METHOD_ID || o.EMOTIONLESS_METHOD_ID == rdo.EMOTIONLESS_METHOD_SECOND_ID);
                                            if (pr == null)
                                            {
                                                pr = cfg.First();
                                            }

                                            item.REMUNERATION_PRICE = pr.PRICE;
                                        }
                                    }
                                    rdo.REMUNERATION_PRICE += item.REMUNERATION_PRICE ?? 0;
                                }
                            }


                            rdo.START_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.L_START_TIME ?? 0);

                            if (rdo.L_BEGIN_TIME != null && rdo.L_END_TIME != null)
                            {
                                long begin_time = (long)rdo.L_BEGIN_TIME;
                                long end_time = (long)rdo.L_END_TIME;
                                System.DateTime? begin = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(begin_time);
                                System.DateTime? end = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(end_time);
                                TimeSpan total = end.Value - begin.Value;
                                rdo.SURG_TIME_TOTAL = (int)total.TotalMinutes;
                                rdo.TIME_TOTAL_STR = rdo.SURG_TIME_TOTAL.ToString() + " phút ";
                            }

                            rdo.EXECUTE_ROLE_MAIN_LOGINNAME = string.Join(",", ekipUser.Where(o => o.EKIP_ID == rdo.EKIP_ID && o.EXECUTE_ROLE_ID == roleMain).Select(p => p.LOGINNAME).Distinct().ToList());
                            rdo.EXECUTE_ROLE_MAIN = string.Join(",", ekipUser.Where(o => o.EKIP_ID == rdo.EKIP_ID && o.EXECUTE_ROLE_ID == roleMain).Select(p => p.USERNAME).Distinct().ToList());
                            rdo.EXECUTE_ROLE_ANESTHETIST = string.Join(",", ekipUser.Where(o => o.EKIP_ID == rdo.EKIP_ID && o.EXECUTE_ROLE_ID == Anesthetist).Select(p => p.USERNAME).Distinct().ToList());
                            rdo.EXECUTE_ROLE_PM1 = string.Join(",", ekipUser.Where(o => o.EKIP_ID == rdo.EKIP_ID && o.EXECUTE_ROLE_ID == rolePM1).Select(p => p.USERNAME).Distinct().ToList());
                            rdo.EXECUTE_ROLE_PM2 = string.Join(",", ekipUser.Where(o => o.EKIP_ID == rdo.EKIP_ID && o.EXECUTE_ROLE_ID == rolePM2).Select(p => p.USERNAME).Distinct().ToList());
                            rdo.EXECUTE_ROLE_PMe1 = string.Join(",", ekipUser.Where(o => o.EKIP_ID == rdo.EKIP_ID && o.EXECUTE_ROLE_ID == rolePMe1).Select(p => p.USERNAME).Distinct().ToList());
                            rdo.EXECUTE_ROLE_PMe2 = string.Join(",", ekipUser.Where(o => o.EKIP_ID == rdo.EKIP_ID && o.EXECUTE_ROLE_ID == rolePMe2).Select(p => p.USERNAME).Distinct().ToList());
                            rdo.EXECUTE_ROLE_NURSE = string.Join(",", ekipUser.Where(o => o.EKIP_ID == rdo.EKIP_ID && o.EXECUTE_ROLE_ID == roleYTDD).Select(p => p.USERNAME).Distinct().ToList());
                            rdo.EXECUTE_ROLE_DCVPTTT = string.Join(",", ekipUser.Where(o => o.EKIP_ID == rdo.EKIP_ID && o.EXECUTE_ROLE_ID == roleDCVPTTT).Select(p => p.USERNAME).Distinct().ToList());

                            if (IsNotNullOrEmpty(ekipUser))
                            {
                                rdo.DICR_EXECUTE_USERNAME = ekipUser.GroupBy(o => o.EXECUTE_ROLE_CODE).ToDictionary(q => q.Key, q => string.Join("\r\n", q.Select(o => o.USERNAME).Distinct().ToList()));
                                rdo.EXECUTE_ROLE_CODEs = string.Join(",", rdo.DICR_EXECUTE_USERNAME.Keys.Distinct().ToList());
                                rdo.USERNAMEs = string.Join(",", ekipUser.Select(o => o.USERNAME).Distinct().ToList());
                            }

                            rdo.PTTT_NUM_ORDER = rdo.PARENT_ID != null ? 2 : 1;
                            decimal tyle = 0;
                            if (rdo.ORIGINAL_PRICE > 0)
                            {
                                tyle = rdo.HEIN_LIMIT_PRICE.HasValue ? (rdo.HEIN_LIMIT_PRICE.Value / (rdo.ORIGINAL_PRICE * (1 + rdo.VAT_RATIO))) * 100 : (rdo.PRICE / rdo.ORIGINAL_PRICE) * 100;
                            }

                            rdo.BHYT_RATIO = tyle;
                            rdo.EKIP_ID = rdo.EKIP_ID ?? 0;
                            rdo.AGE = Inventec.Common.DateTime.Calculation.Age(rdo.DOB ?? 0);

                            rdo.IN_CODE = rdo.IN_CODE;
                            HIS_PATIENT_TYPE TdlPatientType = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                            rdo.TREATMENT_PATIENT_TYPE_NAME = TdlPatientType.PATIENT_TYPE_NAME;

                            rdo.BEGIN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.L_BEGIN_TIME ?? 0);
                            rdo.END_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.L_END_TIME ?? 0);
                            rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.L_IN ?? 0);
                            rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.L_OUT ?? 0);
                            rdo.INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.L_INTRUCTION ?? 0);
                        }
                        ////
                        listRdoCount.AddRange(listRdoLocal);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    //group các thuoc vat tu hao phi PTTT
                    string keyGroupMemaFollow = "{0}_{1}_{2}_{3}";
                    if (this.dicDataFilter.ContainsKey("KEY_GROUP_MEMA_FOLLOW") && this.dicDataFilter["KEY_GROUP_MEMA_FOLLOW"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_MEMA_FOLLOW"].ToString()))
                    {
                        keyGroupMemaFollow = this.dicDataFilter["KEY_GROUP_MEMA_FOLLOW"].ToString();
                    }
                    //string key = string.Format(keyGroupMemaFollow, rdo.SERVICE_CODE, memaType, medi.TDL_SERVICE_CODE, mainLoginnameStr, rdo.EXECUTE_DEPARTMENT_ID, rdo.REQUEST_DEPARTMENT_ID, medi.VIR_PRICE_NO_EXPEND ?? 0);
                    var groupPttt = listRdo.GroupBy(g => string.Format(keyGroupMemaFollow, g.SERVICE_CODE, "NONE", "NONE", "NONE", g.EXECUTE_DEPARTMENT_ID, g.REQUEST_DEPARTMENT_ID, "NONE")).ToList();
                    foreach (var group in groupPttt)
                    {
                        var ptttIds = group.Select(o => o.ID).ToList();
                        var listMemaSub = listMemaDetail.Where(o => ptttIds.Contains(o.PARENT_ID)).ToList();
                        var pttt = group.First();
                        if (listMemaSub.Count > 0)
                        {
                            var groupMema = listMemaSub.GroupBy(g => string.Format(keyGroupMemaFollow, "NONE", g.MEMA_TYPE, g.MEMA_CODE, g.MAIN_LOGINNAME, "NONE", "NONE", g.VIR_PRICE_NO_EXPEND)).ToList();
                            foreach (var item in groupMema)
                            {
                                var medi = item.First();
                                if (medi != null)
                                {
                                    MEMA_FOLLOW memaAvg = new MEMA_FOLLOW();
                                    var reqDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == pttt.REQUEST_DEPARTMENT_ID);
                                    if (reqDepartment != null)
                                    {
                                        memaAvg.REQUEST_DEPARTMENT_CODE = reqDepartment.DEPARTMENT_CODE;
                                        memaAvg.REQUEST_DEPARTMENT_NAME = reqDepartment.DEPARTMENT_NAME;
                                    }
                                    var exeDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == pttt.EXECUTE_DEPARTMENT_ID);
                                    if (exeDepartment != null)
                                    {
                                        memaAvg.EXECUTE_DEPARTMENT_CODE = exeDepartment.DEPARTMENT_CODE;
                                        memaAvg.EXECUTE_DEPARTMENT_NAME = exeDepartment.DEPARTMENT_NAME;
                                    }
                                    memaAvg.SERVICE_CODE = pttt.SERVICE_CODE;
                                    memaAvg.SERVICE_NAME = pttt.SERVICE_NAME;
                                    memaAvg.AMOUNT = group.Sum(s => s.AMOUNT);
                                    memaAvg.MEMA_AMOUNT = item.Sum(s => s.MEMA_AMOUNT);
                                    memaAvg.MEMA_TYPE = medi.MEMA_TYPE;
                                    memaAvg.MEMA_CODE = medi.MEMA_CODE;
                                    memaAvg.MEMA_NAME = medi.MEMA_NAME;
                                    memaAvg.MEMA_UNIT_CODE = medi.MEMA_UNIT_CODE;
                                    memaAvg.MEMA_UNIT_NAME = medi.MEMA_UNIT_NAME;
                                    memaAvg.MEMA_AMOUNT = medi.MEMA_AMOUNT;
                                    memaAvg.MAIN_USERNAME = medi.MAIN_USERNAME;
                                    memaAvg.MAIN_LOGINNAME = medi.MAIN_LOGINNAME;
                                    memaAvg.VIR_PRICE_NO_EXPEND = medi.VIR_PRICE_NO_EXPEND;
                                    ListMemaAgg.Add(memaAvg);
                                }
                            }
                        }
                        else
                        {
                            MEMA_FOLLOW memaAvg = new MEMA_FOLLOW();
                            var reqDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == pttt.REQUEST_DEPARTMENT_ID);
                            if (reqDepartment != null)
                            {
                                memaAvg.REQUEST_DEPARTMENT_CODE = reqDepartment.DEPARTMENT_CODE;
                                memaAvg.REQUEST_DEPARTMENT_NAME = reqDepartment.DEPARTMENT_NAME;
                            }
                            var exeDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == pttt.EXECUTE_DEPARTMENT_ID);
                            if (exeDepartment != null)
                            {
                                memaAvg.EXECUTE_DEPARTMENT_CODE = exeDepartment.DEPARTMENT_CODE;
                                memaAvg.EXECUTE_DEPARTMENT_NAME = exeDepartment.DEPARTMENT_NAME;
                            }
                            memaAvg.SERVICE_CODE = pttt.SERVICE_CODE;
                            memaAvg.SERVICE_NAME = pttt.SERVICE_NAME;
                            memaAvg.AMOUNT = group.Sum(s => s.AMOUNT);
                            //memaAvg.MEMA_AMOUNT = item.Sum(s => s.MEMA_AMOUNT);
                            //memaAvg.MEMA_TYPE = medi.MEMA_TYPE;
                            //memaAvg.MEMA_CODE = medi.MEMA_CODE;
                            //memaAvg.MEMA_NAME = medi.MEMA_NAME;
                            //memaAvg.MEMA_UNIT_CODE = medi.MEMA_UNIT_CODE;
                            //memaAvg.MEMA_UNIT_NAME = medi.MEMA_UNIT_NAME;
                            //memaAvg.MEMA_AMOUNT = medi.MEMA_AMOUNT;
                            //memaAvg.MAIN_USERNAME = medi.MAIN_USERNAME;
                            //memaAvg.MAIN_LOGINNAME = medi.MAIN_LOGINNAME;
                            //memaAvg.VIR_PRICE_NO_EXPEND = medi.VIR_PRICE_NO_EXPEND;
                            ListMemaAgg.Add(memaAvg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }
        private void CountPttt(List<Mrs00255RDO> list)
        {
            try
            {
                if (IsNotNullOrEmpty(list))
                {
                    PTTT_COUNT r = new PTTT_COUNT();
                    foreach (var item in list)
                    {
                        if (item.PTTT_GROUP_ID != null)
                        {
                            if (item.PTTT_GROUP_NAME == "Thủ thuật loại 1")
                            {
                                if (item.PTTT_PRIORITY_ID != null)
                                {
                                    if (item.PTTT_PRIORITY_NAME == "Mổ cấp cứu")
                                    {
                                        r.TT_TYPE_1_CC += 1;
                                    }
                                }
                                if (item.PTTT_PRIORITY_NAME == null)
                                {
                                    r.TT_TYPE_1_KH += 1;
                                }
                                if (item.PTTT_CATASTROPHE_ID != null)
                                {
                                    if (item.PTTT_CATASTROPHE_NAME == "Do gây mê")
                                    {
                                        r.TT_TYPE_1_GMSH += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do nhiễm khuẩn")
                                    {
                                        r.TT_TYPE_1_NK += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do tai biến")
                                    {
                                        r.TT_TYPE_1_TB += 1;
                                    }
                                }
                                if (item.DEATH_WITHIN_ID != null)
                                {
                                    if (item.DEATH_WITHIN_NAME == "Tử vong trên bàn phẫu thuật")
                                    {
                                        r.TT_TYPE_1_DIE += 1;
                                    }
                                    if (item.DEATH_WITHIN_NAME == "Trong 24 giờ vào")
                                    {
                                        r.TT_TYPE_1_DIE_24 += 1;
                                    }
                                }
                            }
                            if (item.PTTT_GROUP_NAME == "Thủ thuật loại 2")
                            {
                                if (item.PTTT_PRIORITY_ID != null)
                                {
                                    if (item.PTTT_PRIORITY_NAME == "Mổ cấp cứu")
                                    {
                                        r.TT_TYPE_2_CC += 1;
                                    }
                                }
                                if (item.PTTT_PRIORITY_NAME == null)
                                {
                                    r.TT_TYPE_2_KH += 1;
                                }
                                if (item.PTTT_CATASTROPHE_ID != null)
                                {
                                    if (item.PTTT_CATASTROPHE_NAME == "Do gây mê")
                                    {
                                        r.TT_TYPE_2_GMHS += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do nhiễm khuẩn")
                                    {
                                        r.TT_TYPE_2_NK += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do tai biến")
                                    {
                                        r.TT_TYPE_2_TB += 1;
                                    }
                                }
                                if (item.DEATH_WITHIN_ID != null)
                                {
                                    if (item.DEATH_WITHIN_NAME == "Tử vong trên bàn phẫu thuật")
                                    {
                                        r.TT_TYPE_2_DIE += 1;
                                    }
                                    if (item.DEATH_WITHIN_NAME == "Trong 24 giờ vào")
                                    {
                                        r.TT_TYPE_2_DIE_24 += 1;
                                    }
                                }
                            }
                            if (item.PTTT_GROUP_NAME == "Thủ thuật loại 3")
                            {
                                if (item.PTTT_PRIORITY_ID != null)
                                {

                                    if (item.PTTT_PRIORITY_NAME == "Mổ cấp cứu")
                                    {
                                        r.TT_TYPE_3_CC += 1;
                                    }
                                }
                                if (item.PTTT_PRIORITY_NAME == null)
                                {
                                    r.TT_TYPE_3_KH += 1;
                                }
                                if (item.PTTT_CATASTROPHE_ID != null)
                                {
                                    if (item.PTTT_CATASTROPHE_NAME == "Do gây mê")
                                    {
                                        r.TT_TYPE_3_GMHS += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do nhiễm khuẩn")
                                    {
                                        r.TT_TYPE_3_NK += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do tai biến")
                                    {
                                        r.TT_TYPE_3_TB += 1;
                                    }
                                }
                                if (item.DEATH_WITHIN_ID != null)
                                {
                                    if (item.DEATH_WITHIN_NAME == "Tử vong trên bàn phẫu thuật")
                                    {
                                        r.TT_TYPE_3_DIE = +1;
                                    }
                                    if (item.DEATH_WITHIN_NAME == "Trong 24 giờ vào")
                                    {
                                        r.TT_TYPE_3_DIE_24 += 1;
                                    }
                                }
                            }
                            if (item.PTTT_GROUP_NAME == "Phẫu thuật loại 1")
                            {
                                if (item.PTTT_PRIORITY_ID != null)
                                {

                                    if (item.PTTT_PRIORITY_NAME == "Mổ cấp cứu")
                                    {
                                        r.PT_TYPE_1_CC += 1;
                                    }
                                }
                                if (item.PTTT_PRIORITY_NAME == null)
                                {
                                    r.PT_TYPE_1_KH += 1;
                                }
                                if (item.PTTT_CATASTROPHE_ID != null)
                                {
                                    if (item.PTTT_CATASTROPHE_NAME == "Do gây mê")
                                    {
                                        r.PT_TYPE_1_GMSH += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do nhiễm khuẩn")
                                    {
                                        r.PT_TYPE_1_NK += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do tai biến")
                                    {
                                        r.PT_TYPE_1_TB += 1;
                                    }
                                }
                                if (item.DEATH_WITHIN_ID != null)
                                {
                                    if (item.DEATH_WITHIN_NAME == "Tử vong trên bàn phẫu thuật")
                                    {
                                        r.PT_TYPE_1_DIE += 1;
                                    }
                                    if (item.DEATH_WITHIN_NAME == "Trong 24 giờ vào")
                                    {
                                        r.PT_TYPE_1_DIE_24 += 1;
                                    }
                                }
                            }
                            if (item.PTTT_GROUP_NAME == "Phẫu thuật loại 2")
                            {
                                if (item.PTTT_PRIORITY_ID != null)
                                {

                                    if (item.PTTT_PRIORITY_NAME == "Mổ cấp cứu")
                                    {
                                        r.PT_TYPE_2_CC += 1;
                                    }
                                }
                                if (item.PTTT_PRIORITY_NAME == null)
                                {
                                    r.PT_TYPE_2_KH += 1;
                                }
                                if (item.PTTT_CATASTROPHE_ID != null)
                                {
                                    if (item.PTTT_CATASTROPHE_NAME == "Do gây mê")
                                    {
                                        r.PT_TYPE_2_GMHS += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do nhiễm khuẩn")
                                    {
                                        r.PT_TYPE_1_NK += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do tai biến")
                                    {
                                        r.PT_TYPE_2_TB += 1;
                                    }
                                }
                                if (item.DEATH_WITHIN_ID != null)
                                {
                                    if (item.DEATH_WITHIN_NAME == "Tử vong trên bàn phẫu thuật")
                                    {
                                        r.PT_TYPE_2_DIE += 1;
                                    }
                                    if (item.DEATH_WITHIN_NAME == "Trong 24 giờ vào")
                                    {
                                        r.PT_TYPE_2_DIE_24 += 1;
                                    }
                                }
                            }
                            if (item.PTTT_GROUP_NAME == "Phẫu thuật loại 3")
                            {
                                if (item.PTTT_PRIORITY_ID != null)
                                {

                                    if (item.PTTT_PRIORITY_NAME == "Mổ cấp cứu")
                                    {
                                        r.PT_TYPE_3_CC += 1;
                                    }
                                }
                                if (item.PTTT_PRIORITY_NAME == null)
                                {
                                    r.PT_TYPE_3_KH += 1;
                                }
                                if (item.PTTT_CATASTROPHE_ID != null)
                                {
                                    if (item.PTTT_CATASTROPHE_NAME == "Do gây mê")
                                    {
                                        r.PT_TYPE_3_GMHS += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do nhiễm khuẩn")
                                    {
                                        r.PT_TYPE_3_NK += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do tai biến")
                                    {
                                        r.PT_TYPE_3_TB += 1;
                                    }
                                }
                                if (item.DEATH_WITHIN_ID != null)
                                {
                                    if (item.DEATH_WITHIN_NAME == "Tử vong trên bàn phẫu thuật")
                                    {
                                        r.PT_TYPE_3_DIE = +1;
                                    }
                                    if (item.DEATH_WITHIN_NAME == "Trong 24 giờ vào")
                                    {
                                        r.PT_TYPE_3_DIE_24 += 1;
                                    }
                                }
                            }
                            if (item.PTTT_GROUP_NAME == "Phẫu thuật loại đặc biệt")
                            {
                                if (item.PTTT_PRIORITY_ID != null)
                                {

                                    if (item.PTTT_PRIORITY_NAME == "Mổ cấp cứu")
                                    {
                                        r.PT_TYPE_DB_CC += 1;
                                    }
                                }
                                if (item.PTTT_PRIORITY_NAME == null)
                                {
                                    r.PT_TYPE_DB_KH += 1;
                                }
                                if (item.PTTT_CATASTROPHE_ID != null)
                                {
                                    if (item.PTTT_CATASTROPHE_NAME == "Do gây mê")
                                    {
                                        r.PT_TYPE_DB_GMHS += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do nhiễm khuẩn")
                                    {
                                        r.PT_TYPE_DB_NK += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_NAME == "Do tai biến")
                                    {
                                        r.PT_TYPE_DB_TB += 1;
                                    }
                                }
                                if (item.DEATH_WITHIN_ID != null)
                                {
                                    if (item.DEATH_WITHIN_NAME == "Tử vong trên bàn phẫu thuật")
                                    {
                                        r.PT_TYPE_DB_DIE += 1;
                                    }
                                    if (item.DEATH_WITHIN_NAME == "Trong 24 giờ vào")
                                    {
                                        r.PT_TYPE_DB_DIE_24 += 1;
                                    }
                                }
                                if (item.PTTT_GROUP_NAME == "Thủ thuật loại đặc biệt")
                                {
                                    if (item.PTTT_PRIORITY_ID != null)
                                    {

                                        if (item.PTTT_PRIORITY_NAME == "Mổ cấp cứu")
                                        {
                                            r.TT_TYPE_DB_CC += 1;
                                        }
                                    }
                                    if (item.PTTT_PRIORITY_NAME == null)
                                    {
                                        r.TT_TYPE_DB_KH += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_ID != null)
                                    {
                                        if (item.PTTT_CATASTROPHE_NAME == "Do gây mê")
                                        {
                                            r.TT_TYPE_DB_GMHS += 1;
                                        }
                                        if (item.PTTT_CATASTROPHE_NAME == "Do nhiễm khuẩn")
                                        {
                                            r.TT_TYPE_DB_NK += 1;
                                        }
                                        if (item.PTTT_CATASTROPHE_NAME == "Do tai biến")
                                        {
                                            r.TT_TYPE_DB_TB += 1;
                                        }
                                    }
                                    if (item.DEATH_WITHIN_ID != null)
                                    {
                                        if (item.DEATH_WITHIN_NAME == "Tử vong trên bàn phẫu thuật")
                                        {
                                            r.TT_TYPE_DB_DIE += 1;
                                        }
                                        if (item.DEATH_WITHIN_NAME == "Trong 24 giờ vào")
                                        {
                                            r.TT_TYPE_DB_DIE_24 += 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                                {
                                    if (item.PTTT_PRIORITY_ID != null)
                                    {

                                        if (item.PTTT_PRIORITY_NAME == "Mổ cấp cứu")
                                        {
                                            r.TT_TYPE_0_CC += 1;
                                        }
                                    }
                                    if (item.PTTT_PRIORITY_NAME == null)
                                    {
                                        r.TT_TYPE_0_KH += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_ID != null)
                                    {
                                        if (item.PTTT_CATASTROPHE_NAME == "Do gây mê")
                                        {
                                            r.TT_TYPE_0_GMHS += 1;
                                        }
                                        if (item.PTTT_CATASTROPHE_NAME == "Do nhiễm khuẩn")
                                        {
                                            r.TT_TYPE_0_NK += 1;
                                        }
                                        if (item.PTTT_CATASTROPHE_NAME == "Do tai biến")
                                        {
                                            r.TT_TYPE_0_TB += 1;
                                        }
                                    }
                                    if (item.DEATH_WITHIN_ID != null)
                                    {
                                        if (item.DEATH_WITHIN_NAME == "Tử vong trên bàn phẫu thuật")
                                        {
                                            r.TT_TYPE_0_DIE += 1;
                                        }
                                        if (item.DEATH_WITHIN_NAME == "Trong 24 giờ vào")
                                        {
                                            r.TT_TYPE_0_DIE_24 += 1;
                                        }
                                    }
                                }
                                if (item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
                                {
                                    if (item.PTTT_PRIORITY_ID != null)
                                    {

                                        if (item.PTTT_PRIORITY_NAME == "Mổ cấp cứu")
                                        {
                                            r.PT_TYPE_0_CC += 1;
                                        }
                                    }
                                    if (item.PTTT_PRIORITY_NAME == null)
                                    {
                                        r.PT_TYPE_0_KH += 1;
                                    }
                                    if (item.PTTT_CATASTROPHE_ID != null)
                                    {
                                        if (item.PTTT_CATASTROPHE_NAME == "Do gây mê")
                                        {
                                            r.PT_TYPE_0_GMHS += 1;
                                        }
                                        if (item.PTTT_CATASTROPHE_NAME == "Do nhiễm khuẩn")
                                        {
                                            r.PT_TYPE_0_NK += 1;
                                        }
                                        if (item.PTTT_CATASTROPHE_NAME == "Do tai biến")
                                        {
                                            r.PT_TYPE_0_TB += 1;
                                        }
                                    }
                                    if (item.DEATH_WITHIN_ID != null)
                                    {
                                        if (item.DEATH_WITHIN_NAME == "Tử vong trên bàn phẫu thuật")
                                        {
                                            r.PT_TYPE_0_DIE += 1;
                                        }
                                        if (item.DEATH_WITHIN_NAME == "Trong 24 giờ vào")
                                        {
                                            r.PT_TYPE_0_DIE_24 += 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    listCount.Add(r);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00255Filter)this.reportFilter).TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00255Filter)this.reportFilter).TIME_TO));
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(x => x.KEY_ORDER).ThenBy(x => x.PTTT_GROUP_NAME).ToList());
                objectTag.AddObjectData(store, "ExecuteRooms", listRdo.GroupBy(o => o.EXECUTE_ROOM_ID).Select(p => p.First()).ToList());
                objectTag.AddObjectData(store, "ReportPT", listRdo.Where(o => o.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).ToList());
                objectTag.AddObjectData(store, "ReportTT", listRdo.Where(o => o.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList());
                objectTag.AddObjectData(store, "CountPTTT", listCount);
                dicSingleTag.Add("SERVICE_NAMEs", SERVICE_NAMEs);

                List<V_HIS_EKIP_USER> priceRole = new List<V_HIS_EKIP_USER>();
                List<V_HIS_EKIP_USER> executeRole = new List<V_HIS_EKIP_USER>();
                if (IsNotNullOrEmpty(ListEkipUserAll))
                {
                    ListEkipUserAll = ListEkipUserAll.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                    var roleGroup = ListEkipUserAll.GroupBy(o => o.EXECUTE_ROLE_ID).ToList();
                    priceRole = roleGroup.Where(o => o.Sum(s => s.REMUNERATION_PRICE ?? 0) > 0).Select(s => s.First()).ToList();
                    executeRole = roleGroup.Select(s => s.First()).ToList();
                }

                objectTag.AddObjectData(store, "ReportPriceRole", priceRole);
                objectTag.AddObjectData(store, "ReportExecuteRole", executeRole);
                objectTag.AddObjectData(store, "ReportEkipUser", ListEkipUserAll);
                objectTag.AddObjectData(store, "MemaFollows", listMemaDetail);
                objectTag.AddObjectData(store, "MemaAvgs", ListMemaAgg.OrderBy(o => string.IsNullOrWhiteSpace(o.MAIN_USERNAME)).ThenBy(o => o.MAIN_USERNAME).ThenBy(o => o.SERVICE_NAME).ThenBy(o => o.MEMA_TYPE).ThenBy(o => o.MEMA_NAME).ToList());
                objectTag.AddObjectData(store, "TreatmentServices", listMemaDetail.GroupBy(o => new { o.MAIN_LOGINNAME, o.TREATMENT_CODE }).Select(p => p.First()).OrderBy(o => string.IsNullOrWhiteSpace(o.MAIN_USERNAME)).ThenBy(o => o.MAIN_USERNAME).ToList());
                objectTag.AddObjectData(store, "TreatmentDepaUserReqs", listMemaDetail.GroupBy(o => new { o.REQUEST_DEPARTMENT_CODE, o.MEMA_REQUEST_LOGINNAME, o.TREATMENT_CODE }).Select(p => p.First()).OrderBy(o => o.REQUEST_DEPARTMENT_CODE).ThenBy(o => o.MEMA_REQUEST_LOGINNAME).ToList());
                objectTag.AddObjectData(store, "MemaNames", listMemaDetail.GroupBy(o => new { o.MEMA_TYPE, o.MEMA_CODE }).Select(p => p.First()).OrderBy(o => o.MEMA_TYPE).ThenBy(o => o.MEMA_NAME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
