using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMaterialType;

using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Core.MrsReport.RDO; 
using MOS.MANAGER.HisExpMest; 
using MOS.MANAGER.HisExpMestMaterial; 
using MOS.MANAGER.HisImpMest; 
using MOS.MANAGER.HisImpMestMaterial; 
using MOS.MANAGER.HisMediStockPeriod; 
using MOS.MANAGER.HisMestPeriodMaty; 
using MOS.MANAGER.HisMestPeriodMety; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00076
{
    public class Mrs00076Processor : AbstractProcessor
    {
        decimal startBeginAmount; 
        CommonParam paramGet = new CommonParam(); 
        List<Mrs00076RDO> ListRdo = new List<Mrs00076RDO>(); 
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialView = new List<V_HIS_EXP_MEST_MATERIAL>(); 
        List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>(); 
        List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>(); 
        HIS_DEPARTMENT department = new HIS_DEPARTMENT(); 
        HIS_MEDI_STOCK mediStock = new HIS_MEDI_STOCK(); 
        List<V_HIS_EXP_MEST> listHisExpMest = new List<V_HIS_EXP_MEST>(); 
        public Mrs00076Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00076Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                var filter = (Mrs00076Filter)this.reportFilter; 


                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery(); 
                impMateFilter.IMP_TIME_FROM = filter.TIME_FROM; 
                impMateFilter.IMP_TIME_TO = filter.TIME_TO; 
                impMateFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                impMateFilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;
                impMateFilter.MATERIAL_TYPE_ID = filter.MATERIAL_TYPE_ID; 
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                hisImpMestMaterial = new HisImpMestMaterialManager().GetView(impMateFilter); 

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery(); 
                expMateFilter.EXP_TIME_FROM = filter.TIME_FROM; 
                expMateFilter.EXP_TIME_TO = filter.TIME_TO; 
                expMateFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                expMateFilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;
                expMateFilter.MATERIAL_TYPE_ID = filter.MATERIAL_TYPE_ID; 
                expMateFilter.IS_EXPORT = true; 
                hisExpMestMaterial = new HisExpMestMaterialManager().GetView(expMateFilter); 
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
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    ListRdo.AddRange((from r in hisImpMestMaterial select new Mrs00076RDO(r)).ToList()); 
                }
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    ListRdo.AddRange((from r in hisExpMestMaterial select new Mrs00076RDO(r)).ToList()); 
                }
                //if (IsNotNullOrEmpty(ListRdo))
                {
                    ProcessBeginAndEndAmount(); 
                }
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 

            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            if (((Mrs00076Filter)this.reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00076Filter)this.reportFilter).TIME_FROM)); 
            }
            if (((Mrs00076Filter)this.reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00076Filter)this.reportFilter).TIME_TO)); 
            }

            V_HIS_MATERIAL_TYPE materialType = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager().GetViewById(((Mrs00076Filter)this.reportFilter).MATERIAL_TYPE_ID); 
            if (materialType != null)
            {
                dicSingleTag.Add("MATERIAL_TYPE_CODE", materialType.MATERIAL_TYPE_CODE); 
                dicSingleTag.Add("MATERIAL_TYPE_NAME", materialType.MATERIAL_TYPE_NAME); 
                dicSingleTag.Add("SERVICE_UNIT_NAME", materialType.SERVICE_UNIT_NAME); 
                dicSingleTag.Add("NATIONAL_NAME", materialType.NATIONAL_NAME); 
                dicSingleTag.Add("MANUFACTURER_NAME", materialType.MANUFACTURER_NAME); 
            }

            HIS_MEDI_STOCK mediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager().GetById(((Mrs00076Filter)this.reportFilter).MEDI_STOCK_ID??0); 
            if (IsNotNull(mediStock))
            {
                dicSingleTag.Add("MEDI_STOCK_CODE", mediStock.MEDI_STOCK_CODE); 
                dicSingleTag.Add("MEDI_STOCK_NAME", mediStock.MEDI_STOCK_NAME); 
            }

            if (IsNotNullOrEmpty(ListRdo))
            {
                dicSingleTag.Add("MEDI_BEGIN_AMOUNT", ListRdo.First().BEGIN_AMOUNT); 
                dicSingleTag.Add("MEDI_END_AMOUNT", ListRdo.Last().END_AMOUNT);
            }
            else
            {
                dicSingleTag.Add("MEDI_BEGIN_AMOUNT", startBeginAmount);
                dicSingleTag.Add("MEDI_END_AMOUNT", startBeginAmount);
            }

            objectTag.AddObjectData(store, "Report", ListRdo); 


        }

        private void ProcessBeginAndEndAmount()
        {
            try
            {
                CommonParam paramGet = new CommonParam();

                if (((Mrs00076Filter)this.reportFilter).MEDI_STOCK_IDs != null)
                {
                    foreach (var item in ((Mrs00076Filter)this.reportFilter).MEDI_STOCK_IDs)
                    {

                        ProcessGetPeriod(paramGet, item);
                    }
                }
                else if (((Mrs00076Filter)this.reportFilter).MEDI_STOCK_ID != null)
                {

                    ProcessGetPeriod(paramGet, ((Mrs00076Filter)this.reportFilter).MEDI_STOCK_ID ?? 0);
                }
                else
                {
                    foreach (var item in HisMediStockCFG.HisMediStocks.Select(o => o.ID).ToList())
                    {

                        ProcessGetPeriod(paramGet, item);
                    }
                }
                ListRdo = ListRdo.OrderBy(o => o.EXECUTE_TIME).ToList();
                ListRdo = ListRdo.GroupBy(g => g.EXECUTE_DATE_STR).Select(s => new Mrs00076RDO { EXECUTE_DATE_STR = s.First().EXECUTE_DATE_STR, EXP_AMOUNT = s.Sum(s1 => s1.EXP_AMOUNT), IMP_AMOUNT = s.Sum(s2 => s2.IMP_AMOUNT) }).ToList();
                decimal previousEndAmount = startBeginAmount;
                foreach (var rdo in ListRdo)
                {
                    rdo.CalculateAmount(previousEndAmount);
                    previousEndAmount = rdo.END_AMOUNT;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
        // Lay kỳ chốt gần nhất với thời gian từ của báo cáo
        private void ProcessGetPeriod(CommonParam paramGet, long mediStockId)
        {
            try
            {
                var filter = (Mrs00076Filter)this.reportFilter; 
                HisMediStockPeriodViewFilterQuery periodFilter = new HisMediStockPeriodViewFilterQuery(); 
                periodFilter.TO_TIME_TO = filter.TIME_FROM; 
                periodFilter.MEDI_STOCK_ID = mediStockId; 
                List<V_HIS_MEDI_STOCK_PERIOD> hisMediStockPeriod = new HisMediStockPeriodManager(paramGet).GetView(periodFilter); 
                if (!paramGet.HasException)
                {
                    if (IsNotNullOrEmpty(hisMediStockPeriod))
                    {
                        //Trường hợp có kỳ được chốt gần nhất
                        V_HIS_MEDI_STOCK_PERIOD neighborPeriod = hisMediStockPeriod.OrderByDescending(d => d.TO_TIME).ToList()[0]; 
                        if (neighborPeriod != null)
                        {
                            ProcessBeinAmountMaterialByMediStockPeriod(paramGet, neighborPeriod, mediStockId); 
                        }
                    }
                    else
                    {
                        // Trường hợp không có kỳ chốt gần nhât - chưa được chốt kỳ nào
                        ProcessBeinAmountMaterialNotMediStockPriod(paramGet, mediStockId); 
                    }

                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu."); 
                    }
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu."); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
        // Tính số lượng đầu kỳ có kỳ dữ liệu gần nhất
        private void ProcessBeinAmountMaterialByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod,long mediStockId)
        {
            try
            {
                var filter = (Mrs00076Filter)this.reportFilter; 

                HisMestPeriodMatyViewFilterQuery periodMatyFilter = new HisMestPeriodMatyViewFilterQuery(); 
                periodMatyFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID; 
                periodMatyFilter.MATERIAL_TYPE_ID = filter.MATERIAL_TYPE_ID; 
                List<V_HIS_MEST_PERIOD_MATY> hisMestPeriodMaty = new HisMestPeriodMatyManager(paramGet).GetView(periodMatyFilter); 
                if (IsNotNullOrEmpty(hisMestPeriodMaty))
                {
                    startBeginAmount += hisMestPeriodMaty.First().VIR_END_AMOUNT ?? 0; 
                }
                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery(); 
                impMateFilter.IMP_TIME_FROM = neighborPeriod.TO_TIME + 1; 
                impMateFilter.IMP_TIME_TO = filter.TIME_FROM - 1; 
                impMateFilter.MEDI_STOCK_ID = mediStockId; 
                impMateFilter.MATERIAL_TYPE_ID = filter.MATERIAL_TYPE_ID; 
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMedicine = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter); 
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    startBeginAmount += hisImpMestMedicine.Sum(s => s.AMOUNT); 
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery(); 
                expMateFilter.EXP_TIME_FROM = neighborPeriod.TO_TIME; 
                expMateFilter.EXP_TIME_TO = filter.TIME_FROM - 1; 
                expMateFilter.MEDI_STOCK_ID = mediStockId; 
                expMateFilter.MATERIAL_TYPE_ID = filter.MATERIAL_TYPE_ID; 
                expMateFilter.IS_EXPORT = true; 
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMedicine = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter); 
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    startBeginAmount -= hisExpMestMedicine.Sum(s => s.AMOUNT); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        //Tính số lượng đầu kỳ không có kỳ dữ liệu gần nhất
        private void ProcessBeinAmountMaterialNotMediStockPriod(CommonParam paramGet, long mediStockId)
        {
            try
            {
                var filter = (Mrs00076Filter)this.reportFilter; 

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery(); 
                impMateFilter.IMP_TIME_TO = filter.TIME_FROM - 1; 
                impMateFilter.MEDI_STOCK_ID = mediStockId; 
                impMateFilter.MATERIAL_TYPE_ID = filter.MATERIAL_TYPE_ID; 
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMedicine = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter); 
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    startBeginAmount += hisImpMestMedicine.Sum(s => s.AMOUNT); 
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery(); 
                expMateFilter.EXP_TIME_TO = filter.TIME_FROM - 1; 
                expMateFilter.MEDI_STOCK_ID = mediStockId; 
                expMateFilter.MATERIAL_TYPE_ID = filter.MATERIAL_TYPE_ID; 
                expMateFilter.IS_EXPORT = true; 
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMedicine = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter); 
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    startBeginAmount -= hisExpMestMedicine.Sum(s => s.AMOUNT); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
