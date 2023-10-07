using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMaterial;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisImpMest; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00388
{
    public class Mrs00388Processor : AbstractProcessor
    {
        Mrs00388Filter castFilter = null; 
        List<Mrs00388RDO> listRdo = new List<Mrs00388RDO>(); 
        List<V_HIS_EXP_MEST> lisExpMest = new List<V_HIS_EXP_MEST>(); 
        V_HIS_MEDI_STOCK mediStock = null; 
        Dictionary<long, List<V_HIS_IMP_MEST_MATERIAL>> dicImpMest = new Dictionary<long, List<V_HIS_IMP_MEST_MATERIAL>>(); 
        List<long> MOBA_IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL

        }; 
        public Mrs00388Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00388Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00388Filter)this.reportFilter; 
                mediStock = MANAGER.Config.HisMediStockCFG.HisMediStocks.FirstOrDefault(w => w.ID == castFilter.MEDI_STOCK_ID); 
                if (mediStock == null)
                {
                    Inventec.Common.Logging.LogSystem.Error(String.Format("khong im thay kho co id = {0} ", castFilter.MEDI_STOCK_ID)); 
                    return false; 
                }
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu bao cao MRS00388: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 

                HisExpMestViewFilterQuery filter = new HisExpMestViewFilterQuery(); 
                filter.HAS_AGGR = true; 
                filter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                filter.FINISH_DATE_FROM = castFilter.TIME_FROM; 
                filter.FINISH_DATE_TO = castFilter.TIME_TO; 

                var prescription = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(filter); 

                if (prescription != null && prescription.Count > 0)
                {
                    lisExpMest = prescription; 
                }

                //HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery(); 
                //mobaFilter.HAS_AGGR = true; 
                //mobaFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                //mobaFilter.IMP_TIME_FROM = castFilter.TIME_FROM; 
                //mobaFilter.IMP_TIME_TO = castFilter.TIME_TO; 
                //var moba = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(mobaFilter); 

                HisImpMestViewFilterQuery mfilter = new HisImpMestViewFilterQuery(); 
                mfilter.HAS_AGGR = true; 
                mfilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                mfilter.IMP_TIME_FROM = castFilter.TIME_FROM; 
                mfilter.IMP_TIME_TO = castFilter.TIME_TO; 
                mfilter.IMP_MEST_TYPE_IDs = MOBA_IMP_MEST_TYPE_IDs; 
                var m = new HisImpMestManager(paramGet).GetView(mfilter); 

                if (IsNotNullOrEmpty(m))
                {
                    int start = 0; 
                    int count = m.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listSub = m.Skip(start).Take(limit).ToList(); 

                        HisImpMestMaterialViewFilterQuery impFilter = new HisImpMestMaterialViewFilterQuery(); 
                        impFilter.IMP_MEST_IDs = listSub.Select(s => s.ID).ToList(); 
                        var imps = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impFilter); 
                        if (IsNotNullOrEmpty(imps))
                        {
                            foreach (var item in imps)
                            {
                                if (!dicImpMest.ContainsKey(item.MATERIAL_ID))
                                    dicImpMest[item.MATERIAL_ID] = new List<V_HIS_IMP_MEST_MATERIAL>(); 
                                dicImpMest[item.MATERIAL_ID].Add(item); 
                            }
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }

                    if (paramGet.HasException)
                    {
                        throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00387"); 
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

        protected override bool ProcessData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                if (IsNotNullOrEmpty(lisExpMest))
                {
                    List<Mrs00388RDO> lstRdo = new List<Mrs00388RDO>(); 
                    List<V_HIS_EXP_MEST_MATERIAL> listMedicine = new List<V_HIS_EXP_MEST_MATERIAL>(); 

                    int start = 0; 
                    int count = lisExpMest.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listSub = lisExpMest.Skip(start).Take(limit).ToList(); 

                        HisExpMestMaterialViewFilterQuery filter = new HisExpMestMaterialViewFilterQuery(); 
                        filter.EXP_MEST_IDs = listSub.Select(s => s.ID).ToList();
                         filter.IS_EXPORT = true;
                        var medicines = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(filter); 
                        if (IsNotNullOrEmpty(medicines))
                        {
                            listMedicine.AddRange(medicines); 
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }

                    if (paramGet.HasException)
                    {
                        throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00387"); 
                    }

                    if (IsNotNullOrEmpty(listMedicine))
                    {
                        var groups = listMedicine.GroupBy(g => new { g.MATERIAL_ID }).ToList(); 
                        foreach (var item in groups)
                        {
                            Mrs00388RDO rdo = new Mrs00388RDO(item.First());

                            if (dicImpMest.ContainsKey(item.First().MATERIAL_ID ?? 0))
                            {
                                var imp = dicImpMest[item.First().MATERIAL_ID ?? 0].ToList(); 
                                rdo.MOBA_AMOUNT = imp.Sum(s => s.AMOUNT); 
                            }

                            rdo.AMOUNT = item.Sum(s => s.AMOUNT); 
                            rdo.PRICE_VAT = rdo.PRICE * (1 + (rdo.VAT_RATIO ?? 0)) ?? 0; 

                            listRdo.Add(rdo); 
                        }
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO)); 

                if (IsNotNullOrEmpty(mediStock.MEDI_STOCK_NAME))
                {
                    dicSingleTag.Add("MEDI_STOCK_NAME", mediStock.MEDI_STOCK_NAME); 
                }
                listRdo = listRdo.OrderBy(o => o.MATERIAL_TYPE_CODE).ToList(); 
                objectTag.AddObjectData(store, "Report", listRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
