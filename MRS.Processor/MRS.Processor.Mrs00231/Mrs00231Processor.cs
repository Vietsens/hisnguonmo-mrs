using MOS.MANAGER.HisService;
using MOS.MANAGER.HisEkip;
using MOS.MANAGER.HisExecuteRole;
using MOS.MANAGER.HisEkipUser;
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
 
using MOS.MANAGER.HisPtttGroup; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisSereServPttt; 
using MOS.MANAGER.HisServiceReq; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00231
{
    public class Mrs00231Processor : AbstractProcessor
    {
        private List<Mrs00231RDO> listMrs00231Rdos = new List<Mrs00231RDO>(); 

        private List<Mrs00231RDO> name = new List<Mrs00231RDO>(); 
        private List<V_HIS_SERE_SERV> listSereServs; 
        private List<V_HIS_EKIP_USER> listEkipUsers; 
        private List<V_HIS_SERE_SERV_PTTT> listSereServPttts; 
        private List<HIS_PTTT_GROUP> listPtttGroups; 
        private List<V_HIS_SERE_SERV> listSereServGroupServiceTypeIds = new List<V_HIS_SERE_SERV>(); 
        private Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>(); 
        public Mrs00231Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00231Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            var paramGet = new CommonParam(); 
            try
            {
                List<long> cofig = new List<long>(); 
                cofig.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT); 
                cofig.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT); 
                //yeu cau
                HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery()
                {

                    FINISH_TIME_FROM = ((Mrs00231Filter)this.reportFilter).FINISH_TIME_FROM,
                    FINISH_TIME_TO = ((Mrs00231Filter)this.reportFilter).FINISH_TIME_TO,
                    EXECUTE_DEPARTMENT_ID = ((Mrs00231Filter)this.reportFilter).DEPARTMENT_ID,
                }; 
                List<HIS_SERVICE_REQ> listServiceReq = new HisServiceReqManager(paramGet).Get(reqFilter); 

                //YC - DV
                dicServiceReq = listServiceReq.ToDictionary(o => o.ID); 
                listSereServs = new List<V_HIS_SERE_SERV>(); 
                if (IsNotNullOrEmpty(dicServiceReq))
                {
                    int skip = 0; 
                    while (dicServiceReq.Keys.Count - skip > 0)
                    {
                        var limit = dicServiceReq.Keys.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var sereServFilter = new HisSereServViewFilterQuery()
                        {
                            SERVICE_TYPE_IDs = cofig,
                            SERVICE_REQ_IDs = limit
                        }; 
                        var sereServs = new HisSereServManager(paramGet).GetView(sereServFilter); 
                        listSereServs.AddRange(sereServs); 
                    }
                }

                //--------------------------------------------------------------------------------------------------

                var listSereServId = listSereServs.Select(s => s.ID).ToList(); 
                listSereServPttts = new List<V_HIS_SERE_SERV_PTTT>(); 
                if (IsNotNullOrEmpty(listSereServId))
                {
                    var skip = 0; 
                    while (listSereServId.Count() - skip > 0)
                    {
                        var ListDSs = listSereServId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var sereServPtttFilter = new HisSereServPtttViewFilterQuery()
                        {
                            SERE_SERV_IDs = ListDSs,
                            PTTT_GROUP_IDs = ((Mrs00231Filter)this.reportFilter).PTTT_GROUP_IDs,

                        }; 
                        var sereServPttts = new HisSereServPtttManager(paramGet).GetView(sereServPtttFilter); 
                        listSereServPttts.AddRange(sereServPttts); 
                    }
                }
                listSereServPttts = listSereServPttts.GroupBy(p => p.SERE_SERV_ID).Select(g => g.First()).ToList(); 
                //--------------------------------------------------------------------------------------------------

                var listEkipId = listSereServs.Where(s => s.EKIP_ID.HasValue).Select(s => s.EKIP_ID.Value).ToList(); 
                listEkipUsers = new List<V_HIS_EKIP_USER>(); 
                if (IsNotNullOrEmpty(listEkipId))
                {
                    int skip = 0; 
                    while (listEkipId.Count() - skip > 0)
                    {
                        var ListDSs = listEkipId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var ekipUserFilter = new HisEkipUserViewFilterQuery()
                        {
                            EKIP_IDs = ListDSs,
                        }; 
                        var ekipUsers = new HisEkipUserManager(paramGet).GetView(ekipUserFilter); 
                        listEkipUsers.AddRange(ekipUsers); 
                    }
                }

                //--------------------------------------------------------------------------------------------------

                var listPtttGroupId = listSereServPttts.Where(s => s.PTTT_GROUP_ID.HasValue).Select(s => s.PTTT_GROUP_ID.Value).ToList(); 
                listPtttGroups = new List<HIS_PTTT_GROUP>(); 

                if (IsNotNullOrEmpty(listPtttGroupId))
                {
                    int skip = 0; 
                    while (listPtttGroupId.Count() - skip > 0)
                    {
                        var ListDSs = listPtttGroupId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var ptttGroupFilter = new HisPtttGroupFilterQuery()
                        {
                            IDs = ListDSs,
                        }; 
                        var ptttGroups = new HisPtttGroupManager(paramGet).Get(ptttGroupFilter); 
                        listPtttGroups.AddRange(ptttGroups); 
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
            bool result = true; 
            try
            {
                int STT = 0; 
                foreach (var listSereServPttt in listSereServPttts)
                {
                    var listSereServ = listSereServs.Where(s => s.ID == listSereServPttt.SERE_SERV_ID).ToList(); 
                    var listSereServGroupServiceTypeIds = listSereServ.GroupBy(s => s.TDL_SERVICE_TYPE_ID).Select(s => s.First());  //cha
                    foreach (var listSereServGroupServiceTypeId in listSereServGroupServiceTypeIds)
                    {
                        var ekipUsersGroupByLoginNames = listEkipUsers.Where(s => s.EKIP_ID == listSereServGroupServiceTypeId.EKIP_ID).GroupBy(s => s.LOGINNAME).ToList(); 
                        foreach (var ekipUsersGroupByLoginName in ekipUsersGroupByLoginNames)
                        {
                            var loginName = ekipUsersGroupByLoginName.First().LOGINNAME; 
                            var userName = ekipUsersGroupByLoginName.First().USERNAME; 
                            var bSPT1 = 0;  var bSGM1 = 0; 
                            var bSPT2 = 0;  var bSGM2 = 0; 
                            var bSPT3 = 0;  var bSGM3 = 0; 
                            var bSPT4 = 0;  var bSGM4 = 0; 

                            var bSTT1 = 0;  var bSPM11 = 0; 
                            var bSTT2 = 0;  var bSPM12 = 0; 
                            var bSTT3 = 0;  var bSPM13 = 0; 
                            var bSTT4 = 0;  var bSPM14 = 0; 

                            var bSPM21 = 0;  var bSPMe11 = 0; 
                            var bSPM22 = 0;  var bSPMe12 = 0; 
                            var bSPM23 = 0;  var bSPMe13 = 0; 
                            var bSPM24 = 0;  var bSPMe14 = 0; 

                            var bSPMe21 = 0;  var tYDD1 = 0; 
                            var bSPMe22 = 0;  var tYDD2 = 0; 
                            var bSPMe23 = 0;  var tYDD3 = 0; 
                            var bSPMe24 = 0;  var tYDD4 = 0; 
                            var dCVPTTT1 = 0;  var dCVPTTT2 = 0;  var dCVPTTT3 = 0;  var dCVPTTT4 = 0; 
                            foreach (var ekipUser in ekipUsersGroupByLoginName)
                            {
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Main)
                                {
                                    bSPT4 = bSPT4 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Main)
                                {
                                    bSPT1 = bSPT1 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Main)
                                {
                                    bSPT2 = bSPT2 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Main)
                                {
                                    bSPT3 = bSPT3 + 1; 
                                }
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__TT)
                                {
                                    bSTT4 = bSTT4 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__TT)
                                {
                                    bSTT1 = bSTT1 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__TT)
                                {
                                    bSTT2 = bSTT2 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__TT)
                                {
                                    bSTT3 = bSTT3 + 1; 
                                }
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Anesthetist)
                                {
                                    bSGM4 = bSGM4 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Anesthetist)
                                {
                                    bSGM1 = bSGM1 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Anesthetist)
                                {
                                    bSGM2 = bSGM2 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Anesthetist)
                                {
                                    bSGM3 = bSGM3 + 1; 
                                }
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PM1)
                                {
                                    bSGM4 = bSGM4 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PM1)
                                {
                                    bSPM11 = bSPM11 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PM1)
                                {
                                    bSPM12 = bSPM12 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PM1)
                                {
                                    bSPM13 = bSPM13 + 1; 
                                }
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PM2)
                                {
                                    bSPM24 = bSPM24 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PM2)
                                {
                                    bSPM21 = bSPM21 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PM2)
                                {
                                    bSPM22 = bSPM22 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PM2)
                                {
                                    bSPM23 = bSPM23 + 1; 
                                }
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PMe1)
                                {
                                    bSPMe14 = bSPMe14 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PMe1)
                                {
                                    bSPMe11 = bSPMe11 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PMe1)
                                {
                                    bSPMe12 = bSPMe12 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PMe1)
                                {
                                    bSPMe13 = bSPMe13 + 1; 
                                }
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PMe2)
                                {
                                    bSPMe24 = bSPMe24 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PMe2)
                                {
                                    bSPMe21 = bSPMe21 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PMe2)
                                {
                                    bSPMe22 = bSPMe22 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PMe2)
                                {
                                    bSPMe23 = bSPMe23 + 1; 
                                }
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__YTDD)
                                {
                                    tYDD4 = tYDD4 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__YTDD)
                                {
                                    tYDD1 = tYDD1 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__YTDD)
                                {
                                    tYDD2 = tYDD2 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__YTDD)
                                {
                                    tYDD3 = tYDD3 + 1; 
                                }
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__DCVPTTT)
                                {
                                    dCVPTTT4 = dCVPTTT4 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__DCVPTTT)
                                {
                                    dCVPTTT1 = dCVPTTT1 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__DCVPTTT)
                                {
                                    dCVPTTT2 = dCVPTTT2 + 1; 
                                }
                                if (listSereServPttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3 && ekipUser.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__DCVPTTT)
                                {
                                    dCVPTTT3 = dCVPTTT3 + 1; 
                                }
                                STT++; 
                                var rdo = new Mrs00231RDO()
                                {
                                    STT = STT,
                                    LONGIN_NAME = loginName,
                                    USER_NAME = userName,
                                    SERVICE_TYPE_ID = listSereServGroupServiceTypeIds.First().TDL_SERVICE_TYPE_ID,
                                    SERVICE_TYPE_NAME = listSereServGroupServiceTypeIds.First().SERVICE_TYPE_NAME,
                                    bSPTTT1 = listSereServGroupServiceTypeId.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT ? bSPT1 : bSTT1,
                                    bSPTTT2 = listSereServGroupServiceTypeId.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT ? bSPT2 : bSTT2,
                                    bSPTTT3 = listSereServGroupServiceTypeId.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT ? bSPT3 : bSTT3,
                                    bSPTTT4 = listSereServGroupServiceTypeId.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT ? bSPT4 : bSTT4,

                                    //bSTT1 = bSTT1,
                                    //bSTT2 = bSTT2,
                                    //bSTT3 = bSTT3,
                                    //bSTT4 = bSTT4,

                                    bSGM1 = bSGM1,
                                    bSGM2 = bSGM2,
                                    bSGM3 = bSGM3,
                                    bSGM4 = bSGM4,

                                    bSPM11 = bSPM11,
                                    bSPM12 = bSPM12,
                                    bSPM13 = bSPM13,
                                    bSPM14 = bSPM14,

                                    bSPM21 = bSPM21,
                                    bSPM22 = bSPM22,
                                    bSPM23 = bSPM23,
                                    bSPM24 = bSPM24,

                                    bSPMe11 = bSPMe11,
                                    bSPMe12 = bSPMe12,
                                    bSPMe13 = bSPMe13,
                                    bSPMe14 = bSPMe14,

                                    bSPMe21 = bSPM21,
                                    bSPMe22 = bSPM22,
                                    bSPMe23 = bSPM23,
                                    bSPMe24 = bSPM24,

                                    tYDD1 = tYDD1,
                                    tYDD2 = tYDD2,
                                    tYDD3 = tYDD3,
                                    tYDD4 = tYDD4,

                                    dCVPTTT1 = dCVPTTT1,
                                    dCVPTTT2 = dCVPTTT2,
                                    dCVPTTT3 = dCVPTTT3,
                                    dCVPTTT4 = dCVPTTT4,

                                };  listMrs00231Rdos.Add(rdo); 
                            }
                            name.Add(listMrs00231Rdos.FirstOrDefault()); 
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
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            objectTag.AddObjectData(store, "Report", listMrs00231Rdos); 

            dicSingleTag.Add("FINISH_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00231Filter)this.reportFilter).FINISH_TIME_FROM)); 
            dicSingleTag.Add("FINISH_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00231Filter)this.reportFilter).FINISH_TIME_TO)); 
            // dicSingleTag.Add("DEPARTMENT_NAME", listSereServs.First().EXECUTE_DEPARTMENT_NAME); 
            string PTTT_GROUP_NAMEs = ""; 
            if (IsNotNullOrEmpty(listPtttGroups))
                foreach (var listPtttGroup in listPtttGroups)
                {
                    PTTT_GROUP_NAMEs = PTTT_GROUP_NAMEs + " - " + listPtttGroup.PTTT_GROUP_NAME; 
                }
            dicSingleTag.Add("PTTT_GROUP_NAMEs", PTTT_GROUP_NAMEs); 

            name = name.GroupBy(o => o.SERVICE_TYPE_ID).Select(p => p.First()).ToList(); 
            objectTag.AddObjectData(store, "Name", name); 
            objectTag.AddRelationship(store, "Name", "Report", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID"); 
        }
    }
}
