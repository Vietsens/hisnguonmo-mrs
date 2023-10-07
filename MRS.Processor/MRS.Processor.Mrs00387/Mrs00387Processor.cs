using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
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

namespace MRS.Processor.Mrs00387
{
    public class Mrs00387Processor : AbstractProcessor
    {
        Mrs00387Filter castFilter = null; 
        List<Mrs00387RDO> listRdo = new List<Mrs00387RDO>(); 
        List<V_HIS_EXP_MEST> lisExpMest = new List<V_HIS_EXP_MEST>(); 
        V_HIS_MEDI_STOCK mediStock = null;
        Dictionary<long, List<V_HIS_IMP_MEST_MEDICINE>> dicImpMest = new Dictionary<long, List<V_HIS_IMP_MEST_MEDICINE>>();
        List<long> TREATMENT_EXP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DM,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__THPK

        };
        List<long> MOBA_IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL

        }; 

        public Mrs00387Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00387Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00387Filter)this.reportFilter;
                if (castFilter.MEDI_STOCK_ID != null)
                {
                    mediStock = MANAGER.Config.HisMediStockCFG.HisMediStocks.FirstOrDefault(w => w.ID == castFilter.MEDI_STOCK_ID);
                    if (mediStock == null)
                    {
                        Inventec.Common.Logging.LogSystem.Error(String.Format("khong im thay kho co id = {0} ", castFilter.MEDI_STOCK_ID));
                        return false;
                    }
                }
                else mediStock = new V_HIS_MEDI_STOCK();
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu bao cao MRS00387: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 

                HisExpMestViewFilterQuery filter = new HisExpMestViewFilterQuery(); 
                //filter.HAS_AGGR = true; 
                filter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                filter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                filter.FINISH_TIME_TO = castFilter.TIME_TO;
                filter.EXP_MEST_TYPE_IDs = TREATMENT_EXP_MEST_TYPE_IDs; 
                filter.REQ_DEPARTMENT_ID = castFilter.REQUEST_DEPARTMENT_ID;
                filter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                var expMestList = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(filter); 
                Inventec.Common.Logging.LogSystem.Info("expMestList" + expMestList.Count.ToString()); 

                if (expMestList != null && expMestList.Count > 0)
                {
                    lisExpMest = expMestList; 
                }

                HisImpMestViewFilterQuery mfilter = new HisImpMestViewFilterQuery(); 
                //mfilter.HAS_AGGR = true; 
                mfilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                mfilter.IMP_TIME_FROM = castFilter.TIME_FROM; 
                mfilter.IMP_TIME_TO = castFilter.TIME_TO; 
                mfilter.IMP_MEST_TYPE_IDs = MOBA_IMP_MEST_TYPE_IDs;
                mfilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                mfilter.REQ_DEPARTMENT_ID = castFilter.REQUEST_DEPARTMENT_ID; 
                var m = new HisImpMestManager(paramGet).GetView(mfilter); 

                //HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery(); 
                //mobaFilter.HAS_AGGR = true; 
                //mobaFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                //mobaFilter.IMP_TIME_FROM = castFilter.TIME_FROM; 
                //mobaFilter.IMP_TIME_TO = castFilter.TIME_TO; 
                //var moba = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(mobaFilter); 

                if (IsNotNullOrEmpty(m))
                {
                    int start = 0; 
                    int count = m.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listSub = m.Skip(start).Take(limit).ToList(); 
                        HisImpMestMedicineViewFilterQuery impFilter = new HisImpMestMedicineViewFilterQuery(); 
                        impFilter.IMP_MEST_IDs = listSub.Select(s => s.ID).ToList(); 
                        var imps = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impFilter); 
                        Inventec.Common.Logging.LogSystem.Info("imps" + imps.Count.ToString()); 

                        if (IsNotNullOrEmpty(imps))
                        {
                            foreach (var item in imps)
                            {
                                if (!dicImpMest.ContainsKey(item.MEDICINE_TYPE_ID))
                                    dicImpMest[item.MEDICINE_TYPE_ID] = new List<V_HIS_IMP_MEST_MEDICINE>(); 
                                dicImpMest[item.MEDICINE_TYPE_ID].Add(item); 
                            }
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00387"); 
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
                    List<Mrs00387RDO> lstRdo = new List<Mrs00387RDO>(); 
                    List<V_HIS_EXP_MEST_MEDICINE> listMedicine = new List<V_HIS_EXP_MEST_MEDICINE>(); 

                    int start = 0; 
                    int count = lisExpMest.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listSub = lisExpMest.Skip(start).Take(limit).ToList(); 

                        HisExpMestMedicineViewFilterQuery filter = new HisExpMestMedicineViewFilterQuery(); 
                        filter.EXP_MEST_IDs = listSub.Select(s => s.ID).ToList();
                        filter.IS_EXPORT = true;
                        var medicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(filter); 
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
                        listMedicine = listMedicine.OrderByDescending(o => o.PRICE).ToList(); 
                        var groups = listMedicine.GroupBy(g => new { g.MEDICINE_TYPE_ID,g.PRICE}).ToList(); 
                        foreach (var item in groups)
                        {
                            Mrs00387RDO rdo = new Mrs00387RDO(item.First()); 

                            if (dicImpMest.ContainsKey(item.First().MEDICINE_TYPE_ID) )
                            {
                                var imp = dicImpMest[item.First().MEDICINE_TYPE_ID].ToList(); 
                                rdo.MOBA_AMOUNT = imp.Sum(s => s.AMOUNT); 
                                dicImpMest[item.First().MEDICINE_TYPE_ID].Clear(); 
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
                listRdo = listRdo.OrderBy(o => o.MEDICINE_TYPE_CODE).ToList(); 
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o=>o.MEDICINE_TYPE_NAME).ToList()); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
