using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00554
{
    class Mrs00554Processor : AbstractProcessor
    {
        Mrs00554Filter castFilter;
        List<Mrs00554RDO> ListRdoPt = new List<Mrs00554RDO>();
        List<Mrs00554RDO> ListRdoTt = new List<Mrs00554RDO>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        List<HIS_SERE_SERV_PTTT> ListSereServPttt = new List<HIS_SERE_SERV_PTTT>();

        #region Config
        long PTTT_CATASTROPHE_ID_GMHS;//Gay me hoi suc
        long PTTT_CATASTROPHE_ID_NK;//Nhiem khuan
        long DEATH_WITHIN_ID_24H;//Tu vong trong 24h
        long DEATH_WITHIN_ID_TBM;//Tu vong tren ban mo

        const string PTTT_CATASTROPHE_CODE_GMHS = "MRS.HIS_PTTT_CATASTROPHE.PTTT_CATASTROPHE_CODE_GMHS";
        const string PTTT_CATASTROPHE_CODE_NK = "MRS.HIS_PTTT_CATASTROPHE.PTTT_CATASTROPHE_CODE_NK";
        const string DEATH_WITHIN_CODE_TBM = "MRS.HIS_DEATH_WITHIN.DEATH_WITHIN_CODE_TBM";
        const string DEATH_WITHIN_CODE_24H = "MRS.HIS_RS.HIS_DEATH_WITHIN.DEATH_WITHIN_CODE.24HOURS";
        #endregion

        public Mrs00554Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00554Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00554Filter)this.reportFilter;
                CommonParam paramCommon = new CommonParam();
                GetConfigValue();

                HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                reqFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                reqFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                reqFilter.SERVICE_REQ_TYPE_IDs = new List<long>() 
                {
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT
                };
                reqFilter.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT;
                var serviceReq = new HisServiceReqManager(paramCommon).Get(reqFilter);

                if (IsNotNullOrEmpty(serviceReq))
                {
                    var skip = 0;
                    while (serviceReq.Count - skip > 0)
                    {
                        var listSVRIds = serviceReq.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                        ssFilter.SERVICE_REQ_IDs = listSVRIds.Select(s => s.ID).ToList();
                        var sereServs = new HisSereServManager(paramCommon).Get(ssFilter);
                        if (IsNotNullOrEmpty(sereServs))
                        {
                            ListSereServ.AddRange(sereServs);
                        }
                    }
                }

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    var treatmentIds = ListSereServ.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList();

                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listTreaIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisTreatmentFilterQuery treaFilter = new HisTreatmentFilterQuery();
                        treaFilter.IDs = listTreaIds;
                        var treatments = new HisTreatmentManager(paramCommon).Get(treaFilter);
                        if (IsNotNullOrEmpty(treatments))
                        {
                            ListTreatment.AddRange(treatments);
                        }
                    }

                    skip = 0;
                    while (ListSereServ.Count - skip > 0)
                    {
                        var listSsIds = ListSereServ.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisSereServPtttFilterQuery ptttFilter = new HisSereServPtttFilterQuery();
                        ptttFilter.SERE_SERV_IDs = listSsIds.Select(s => s.ID).ToList();
                        var pttt = new HisSereServPtttManager(paramCommon).Get(ptttFilter);
                        if (IsNotNullOrEmpty(pttt))
                        {
                            ListSereServPttt.AddRange(pttt);
                        }
                    }
                }

                if (paramCommon.HasException)
                {
                    Inventec.Common.Logging.LogSystem.Error("Co loi trong qua trinh get du lieu MRS00554");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetConfigValue()
        {
            try
            {
                var deathWithin = new MOS.MANAGER.HisDeathWithin.HisDeathWithinManager(new CommonParam()).Get(new MOS.MANAGER.HisDeathWithin.HisDeathWithinFilterQuery());
                var ptttCatastrophe = new MOS.MANAGER.HisPtttCatastrophe.HisPtttCatastropheManager(new CommonParam()).Get(new MOS.MANAGER.HisPtttCatastrophe.HisPtttCatastropheFilterQuery());

                if (Loader.dictionaryConfig.ContainsKey(DEATH_WITHIN_CODE_24H))
                {
                    var config = Loader.dictionaryConfig[DEATH_WITHIN_CODE_24H];
                    if (IsNotNull(config))
                    {
                        var key = !String.IsNullOrWhiteSpace(config.VALUE) ? config.VALUE : config.DEFAULT_VALUE;
                        if (!String.IsNullOrWhiteSpace(key))
                        {
                            var death = deathWithin.FirstOrDefault(o => o.DEATH_WITHIN_CODE == key);
                            DEATH_WITHIN_ID_24H = IsNotNull(death) ? death.ID : 0;
                        }
                    }
                }

                if (Loader.dictionaryConfig.ContainsKey(DEATH_WITHIN_CODE_TBM))
                {
                    var config = Loader.dictionaryConfig[DEATH_WITHIN_CODE_TBM];
                    if (IsNotNull(config))
                    {
                        var key = !String.IsNullOrWhiteSpace(config.VALUE) ? config.VALUE : config.DEFAULT_VALUE;
                        if (!String.IsNullOrWhiteSpace(key))
                        {
                            var death = deathWithin.FirstOrDefault(o => o.DEATH_WITHIN_CODE == key);
                            DEATH_WITHIN_ID_TBM = IsNotNull(death) ? death.ID : 0;
                        }
                    }
                }

                if (Loader.dictionaryConfig.ContainsKey(PTTT_CATASTROPHE_CODE_GMHS))
                {
                    var config = Loader.dictionaryConfig[PTTT_CATASTROPHE_CODE_GMHS];
                    if (IsNotNull(config))
                    {
                        var key = !String.IsNullOrWhiteSpace(config.VALUE) ? config.VALUE : config.DEFAULT_VALUE;
                        if (!String.IsNullOrWhiteSpace(key))
                        {
                            var pttt = ptttCatastrophe.FirstOrDefault(o => o.PTTT_CATASTROPHE_CODE == key);
                            PTTT_CATASTROPHE_ID_GMHS = IsNotNull(pttt) ? pttt.ID : 0;
                        }
                    }
                }

                if (Loader.dictionaryConfig.ContainsKey(PTTT_CATASTROPHE_CODE_NK))
                {
                    var config = Loader.dictionaryConfig[PTTT_CATASTROPHE_CODE_NK];
                    if (IsNotNull(config))
                    {
                        var key = !String.IsNullOrWhiteSpace(config.VALUE) ? config.VALUE : config.DEFAULT_VALUE;
                        if (!String.IsNullOrWhiteSpace(key))
                        {
                            var pttt = ptttCatastrophe.FirstOrDefault(o => o.PTTT_CATASTROPHE_CODE == key);
                            PTTT_CATASTROPHE_ID_NK = IsNotNull(pttt) ? pttt.ID : 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ListRdoPt.Clear();
                ListRdoTt.Clear();
                if (IsNotNullOrEmpty(ListSereServPttt))
                {
                    Dictionary<long, HIS_TREATMENT> dicTreatment = new Dictionary<long, HIS_TREATMENT>();
                    Dictionary<long, HIS_SERE_SERV> dicSereServ = new Dictionary<long, HIS_SERE_SERV>();
                    Dictionary<long, HIS_PTTT_GROUP> dicPtttGroup = new Dictionary<long, HIS_PTTT_GROUP>();

                    if (IsNotNullOrEmpty(ListTreatment))
                    {
                        dicTreatment = ListTreatment.ToDictionary(d => d.ID);
                    }

                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        dicSereServ = ListSereServ.ToDictionary(d => d.ID);
                    }

                    var lstPtttGroup = new MOS.MANAGER.HisPtttGroup.HisPtttGroupManager(new CommonParam()).Get(new MOS.MANAGER.HisPtttGroup.HisPtttGroupFilterQuery());
                    if (IsNotNullOrEmpty(lstPtttGroup))
                    {
                        dicPtttGroup = lstPtttGroup.ToDictionary(d => d.ID);
                    }

                    var groups = ListSereServPttt.GroupBy(o => o.PTTT_GROUP_ID).ToList();

                    foreach (var group in groups)
                    {
                        var list = group.ToList<HIS_SERE_SERV_PTTT>();
                        if (IsNotNullOrEmpty(list))
                        {
                            foreach (var pttt in list)
                            {
                                if (!dicSereServ.ContainsKey(pttt.SERE_SERV_ID)) continue;
                                if (!dicTreatment.ContainsKey(pttt.TDL_TREATMENT_ID ?? 0)) continue;

                                Mrs00554RDO rdo = new Mrs00554RDO();
                                if (dicPtttGroup.ContainsKey(pttt.PTTT_GROUP_ID ?? 0))
                                {
                                    rdo.PTTT_GROUP_NAME = dicPtttGroup[pttt.PTTT_GROUP_ID ?? 0].PTTT_GROUP_NAME;
                                }

                                if (dicTreatment[pttt.TDL_TREATMENT_ID ?? 0].IS_EMERGENCY == 1)
                                    rdo.TOTAL_PTTT_CC = 1;
                                else
                                    rdo.TOTAL_PTTT_KH = 1;

                                if (pttt.PTTT_CATASTROPHE_ID.HasValue)
                                {
                                    if (pttt.PTTT_CATASTROPHE_ID.Value == PTTT_CATASTROPHE_ID_GMHS)
                                    {
                                        rdo.TOTAL_TB_GMHS = 1;
                                    }
                                    else if (pttt.PTTT_CATASTROPHE_ID.Value == PTTT_CATASTROPHE_ID_NK)
                                    {
                                        rdo.TOTAL_TB_NK = 1;
                                    }
                                    else
                                    {
                                        rdo.TOTAL_TB_K = 1;
                                    }
                                }

                                if (pttt.DEATH_WITHIN_ID.HasValue)
                                {
                                    if (pttt.DEATH_WITHIN_ID.Value == DEATH_WITHIN_ID_24H)
                                    {
                                        rdo.TOTAL_TV_24H = 1;
                                    }
                                    else if (pttt.DEATH_WITHIN_ID.Value == DEATH_WITHIN_ID_TBM)
                                    {
                                        rdo.TOTAL_TV_TBM = 1;
                                    }
                                }

                                if (dicSereServ[pttt.SERE_SERV_ID].TDL_SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT)
                                {
                                    ListRdoPt.Add(rdo);
                                }
                                else if (dicSereServ[pttt.SERE_SERV_ID].TDL_SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT)
                                {
                                    ListRdoTt.Add(rdo);
                                }
                            }
                        }
                    }

                    ListRdoPt = ListRdoPt.GroupBy(o => o.PTTT_GROUP_NAME).Select(
                       g => new Mrs00554RDO
                       {
                           PTTT_GROUP_NAME = g.First().PTTT_GROUP_NAME,
                           TOTAL_PTTT_CC = g.Sum(s => s.TOTAL_PTTT_CC),
                           TOTAL_PTTT_KH = g.Sum(s => s.TOTAL_PTTT_KH),
                           TOTAL_TB_GMHS = g.Sum(s => s.TOTAL_TB_GMHS),
                           TOTAL_TB_K = g.Sum(s => s.TOTAL_TB_K),
                           TOTAL_TB_NK = g.Sum(s => s.TOTAL_TB_NK),
                           TOTAL_TV_24H = g.Sum(s => s.TOTAL_TV_24H),
                           TOTAL_TV_TBM = g.Sum(s => s.TOTAL_TV_TBM),
                       }).ToList();

                    ListRdoTt = ListRdoTt.GroupBy(o => o.PTTT_GROUP_NAME).Select(
                       g => new Mrs00554RDO
                       {
                           PTTT_GROUP_NAME = g.First().PTTT_GROUP_NAME,
                           TOTAL_PTTT_CC = g.Sum(s => s.TOTAL_PTTT_CC),
                           TOTAL_PTTT_KH = g.Sum(s => s.TOTAL_PTTT_KH),
                           TOTAL_TB_GMHS = g.Sum(s => s.TOTAL_TB_GMHS),
                           TOTAL_TB_K = g.Sum(s => s.TOTAL_TB_K),
                           TOTAL_TB_NK = g.Sum(s => s.TOTAL_TB_NK),
                           TOTAL_TV_24H = g.Sum(s => s.TOTAL_TV_24H),
                           TOTAL_TV_TBM = g.Sum(s => s.TOTAL_TV_TBM),
                       }).ToList();
                }
            }
            catch (Exception ex)
            {
                ListRdoPt.Clear();
                ListRdoTt.Clear();
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));

                objectTag.AddObjectData(store, "ReportPt", ListRdoPt);
                objectTag.AddObjectData(store, "ReportTt", ListRdoTt);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
