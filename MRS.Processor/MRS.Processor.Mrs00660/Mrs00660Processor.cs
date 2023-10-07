using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisEkipUser;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisServiceRetyCat;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00660
{
    class Mrs00660Processor : AbstractProcessor
    {
        Mrs00660Filter castFilter;
        List<Mrs00660RDO> ListRdo = new List<Mrs00660RDO>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        Dictionary<long, List<V_HIS_EKIP_USER>> DicEkipUser = new Dictionary<long, List<V_HIS_EKIP_USER>>();
        List<V_HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();

        string RetyCatDB = "660_DB";
        string RetyCatA = "660_A";
        string RetyCatB = "660_B";
        string RetyCatC = "660_C";
        string RetyCatI = "660_I";

        string ReportType = "";

        public Mrs00660Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            ReportType = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00660Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00660Filter)reportFilter);

                HisSereServExtFilterQuery sereServExt = new HisSereServExtFilterQuery();
                sereServExt.BEGIN_TIME_FROM = castFilter.TIME_FROM;
                sereServExt.BEGIN_TIME_TO = castFilter.TIME_TO;
                var listSsExt = new HisSereServExtManager().Get(sereServExt);

                HisServiceRetyCatViewFilterQuery retyCatFilter = new HisServiceRetyCatViewFilterQuery();
                retyCatFilter.REPORT_TYPE_CODE__EXACT = ReportType;
                ListServiceRetyCat = new HisServiceRetyCatManager().GetView(retyCatFilter);

                if (IsNotNullOrEmpty(ListServiceRetyCat))
                {
                    ListServiceRetyCat = ListServiceRetyCat.Where(o =>
                        o.CATEGORY_CODE == RetyCatDB ||
                        o.CATEGORY_CODE == RetyCatA ||
                        o.CATEGORY_CODE == RetyCatB ||
                        o.CATEGORY_CODE == RetyCatC ||
                        o.CATEGORY_CODE == RetyCatI).ToList();
                }

                var listSereServIds = listSsExt.Select(s => s.SERE_SERV_ID).Distinct().ToList();
                if (IsNotNullOrEmpty(listSereServIds))
                {
                    var skip = 0;
                    while (listSereServIds.Count - skip > 0)
                    {
                        var listIDs = listSereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery filterSs = new HisSereServFilterQuery();
                        filterSs.IDs = listIDs;
                        filterSs.TDL_SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT };
                        filterSs.HAS_EXECUTE = true;
                        var listSereServSub = new HisSereServManager().Get(filterSs);
                        if (listSereServSub != null)
                            ListSereServ.AddRange(listSereServSub);
                    }

                    if (IsNotNullOrEmpty(ListSereServ)) ListSereServ = ListSereServ.Where(o => o.SERVICE_REQ_ID.HasValue && o.TDL_TREATMENT_ID.HasValue && o.EKIP_ID.HasValue).ToList();
                }

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    List<V_HIS_EKIP_USER> ListEkipUser = new List<V_HIS_EKIP_USER>();
                    var ekipIds = ListSereServ.Select(o => o.EKIP_ID ?? 0).Distinct().ToList();
                    if (IsNotNullOrEmpty(ekipIds))
                    {
                        var skip = 0;
                        while (ekipIds.Count - skip > 0)
                        {
                            var listIds = ekipIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var HisEkipUserfilter = new HisEkipUserViewFilterQuery
                            {
                                EKIP_IDs = listIds
                            };
                            var listEkipUserSub = new HisEkipUserManager().GetView(HisEkipUserfilter);
                            if (IsNotNullOrEmpty(listEkipUserSub))
                                ListEkipUser.AddRange(listEkipUserSub);
                        }
                    }

                    if (IsNotNullOrEmpty(ListEkipUser))
                    {
                        foreach (var item in ListEkipUser)
                        {
                            if (!DicEkipUser.ContainsKey(item.EKIP_ID))
                                DicEkipUser[item.EKIP_ID] = new List<V_HIS_EKIP_USER>();

                            DicEkipUser[item.EKIP_ID].Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListSereServ) && IsNotNullOrEmpty(DicEkipUser))
                {
                    ListRdo = new List<Mrs00660RDO>();

                    List<Thread> threadData = new List<Thread>();
                    List<List<HIS_SERE_SERV>> listData = new List<List<HIS_SERE_SERV>>();
                    var maxReq = ListSereServ.Count / 5 + 1;
                    Inventec.Common.Logging.LogSystem.Info(maxReq + "/" + ListSereServ.Count);
                    int skip = 0;
                    while (ListSereServ.Count - skip > 0)
                    {
                        var datas = ListSereServ.Skip(skip).Take(maxReq).ToList();
                        skip += maxReq;
                        Thread hein = new Thread(ProcessListData);
                        threadData.Add(hein);
                        listData.Add(datas);
                    }

                    for (int i = 0; i < threadData.Count; i++)
                    {
                        threadData[i].Start(listData[i]);
                    }

                    for (int i = 0; i < threadData.Count; i++)
                    {
                        threadData[i].Join();
                    }

                    ProcessDataTotal();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessDataTotal()
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.GroupBy(g => g.LOGINNAME).Select(s => new Mrs00660RDO()
                    {
                        LOGINNAME = s.First().LOGINNAME,
                        USERNAME = s.First().USERNAME,
                        REMUNERATION_PRICE = s.Sum(o => o.REMUNERATION_PRICE ?? 0),
                        COUNT_DB_C = s.Sum(o => o.COUNT_DB_C),
                        COUNT_DB_P1 = s.Sum(o => o.COUNT_DB_P1),
                        COUNT_DB_P2 = s.Sum(o => o.COUNT_DB_P2),
                        COUNT_DB_K = s.Sum(o => o.COUNT_DB_K),
                        COUNT_A_C = s.Sum(o => o.COUNT_A_C),
                        COUNT_A_P1 = s.Sum(o => o.COUNT_A_P1),
                        COUNT_A_P2 = s.Sum(o => o.COUNT_A_P2),
                        COUNT_A_K = s.Sum(o => o.COUNT_A_K),
                        COUNT_B_C = s.Sum(o => o.COUNT_B_C),
                        COUNT_B_P1 = s.Sum(o => o.COUNT_B_P1),
                        COUNT_B_P2 = s.Sum(o => o.COUNT_B_P2),
                        COUNT_B_K = s.Sum(o => o.COUNT_B_K),
                        COUNT_C_C = s.Sum(o => o.COUNT_C_C),
                        COUNT_C_P1 = s.Sum(o => o.COUNT_C_P1),
                        COUNT_C_P2 = s.Sum(o => o.COUNT_C_P2),
                        COUNT_C_K = s.Sum(o => o.COUNT_C_K),
                        COUNT_I_C = s.Sum(o => o.COUNT_I_C),
                        COUNT_I_P1 = s.Sum(o => o.COUNT_I_P1),
                        COUNT_I_P2 = s.Sum(o => o.COUNT_I_P2),
                        COUNT_I_K = s.Sum(o => o.COUNT_I_K),
                        COUNT_K_C = s.Sum(o => o.COUNT_K_C),
                        COUNT_K_P1 = s.Sum(o => o.COUNT_K_P1),
                        COUNT_K_P2 = s.Sum(o => o.COUNT_K_P2),
                        COUNT_K_K = s.Sum(o => o.COUNT_K_K)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListData(object obj)
        {
            try
            {
                ProcessSereServ((List<HIS_SERE_SERV>)obj);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessSereServ(List<HIS_SERE_SERV> list)
        {
            try
            {
                if (IsNotNullOrEmpty(list))
                {
                    var grCategory = ListServiceRetyCat.GroupBy(o => o.REPORT_TYPE_CAT_ID).ToList();
                    foreach (var grService in grCategory)
                    {
                        if (!IsNotNullOrEmpty(list)) break;

                        var lstSereServ = list.Where(o => grService.Select(s => s.SERVICE_ID).Contains(o.SERVICE_ID)).ToList();
                        if (IsNotNullOrEmpty(lstSereServ))
                        {
                            ProcessDataEkipUser(lstSereServ, grService.First().CATEGORY_CODE);

                            //bo cac dich vu duoc cau hinh tranh tinh 2 lan.
                            list = list.Where(o => !lstSereServ.Select(s => s.ID).Contains(o.ID)).ToList();
                        }
                    }

                    if (IsNotNullOrEmpty(list))
                    {
                        ProcessDataEkipUser(list, "");
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        /// <summary>
        /// Chính = vai trò thực hiện mã '01'
        /// Phụ 1 = vai trò thực hiện mã '02'
        /// Phụ 2 = vai trò thực hiện mã '03'
        /// Khác = và các vai trò thực hiện còn lại
        /// </summary>
        /// <param name="lstSereServ"></param>
        /// <param name="grService"></param>
        private void ProcessDataEkipUser(List<HIS_SERE_SERV> lstSereServ, string categoryCode)
        {
            try
            {
                if (IsNotNullOrEmpty(lstSereServ))
                {
                    foreach (var sereServ in lstSereServ)
                    {
                        if (!sereServ.EKIP_ID.HasValue) continue;

                        if (!DicEkipUser.ContainsKey(sereServ.EKIP_ID.Value)) continue;

                        foreach (var ekipUser in DicEkipUser[sereServ.EKIP_ID.Value])
                        {
                            Mrs00660RDO rdo = new Mrs00660RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00660RDO>(rdo, ekipUser);

                            if (categoryCode == RetyCatDB)
                            {
                                if (ekipUser.EXECUTE_ROLE_CODE == "01")
                                    rdo.COUNT_DB_C = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "02")
                                    rdo.COUNT_DB_P1 = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "03")
                                    rdo.COUNT_DB_P2 = 1;
                                else
                                    rdo.COUNT_DB_K = 1;
                            }
                            else if (categoryCode == RetyCatA)
                            {
                                if (ekipUser.EXECUTE_ROLE_CODE == "01")
                                    rdo.COUNT_A_C = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "02")
                                    rdo.COUNT_A_P1 = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "03")
                                    rdo.COUNT_A_P2 = 1;
                                else
                                    rdo.COUNT_A_K = 1;
                            }
                            else if (categoryCode == RetyCatB)
                            {
                                if (ekipUser.EXECUTE_ROLE_CODE == "01")
                                    rdo.COUNT_B_C = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "02")
                                    rdo.COUNT_B_P1 = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "03")
                                    rdo.COUNT_B_P2 = 1;
                                else
                                    rdo.COUNT_B_K = 1;
                            }
                            else if (categoryCode == RetyCatC)
                            {
                                if (ekipUser.EXECUTE_ROLE_CODE == "01")
                                    rdo.COUNT_C_C = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "02")
                                    rdo.COUNT_C_P1 = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "03")
                                    rdo.COUNT_C_P2 = 1;
                                else
                                    rdo.COUNT_C_K = 1;
                            }
                            else if (categoryCode == RetyCatI)
                            {
                                if (ekipUser.EXECUTE_ROLE_CODE == "01")
                                    rdo.COUNT_I_C = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "02")
                                    rdo.COUNT_I_P1 = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "03")
                                    rdo.COUNT_I_P2 = 1;
                                else
                                    rdo.COUNT_I_K = 1;
                            }
                            else
                            {
                                if (ekipUser.EXECUTE_ROLE_CODE == "01")
                                    rdo.COUNT_K_C = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "02")
                                    rdo.COUNT_K_P1 = 1;
                                else if (ekipUser.EXECUTE_ROLE_CODE == "03")
                                    rdo.COUNT_K_P2 = 1;
                                else
                                    rdo.COUNT_K_K = 1;
                            }

                            ListRdo.Add(rdo);
                        }
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
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
