using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMestPeriodMety;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00075
{
    public class Mrs00075Processor : AbstractProcessor
    {
        Mrs00075Filter castFilter = null; 
        List<Mrs00075RDO> ListRdo = new List<Mrs00075RDO>(); 
        decimal startBeginAmount; 


        public Mrs00075Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00075Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00075Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau get du lieu tu MOS ve:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery(); 
                impMediFilter.IMP_TIME_FROM = castFilter.TIME_FROM; 
                impMediFilter.IMP_TIME_TO = castFilter.TIME_TO; 
                impMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMediFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                impMediFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager().GetView(impMediFilter); 

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery(); 
                expMediFilter.EXP_TIME_FROM = castFilter.TIME_FROM; 
                expMediFilter.EXP_TIME_TO = castFilter.TIME_TO; 
                expMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMediFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                expMediFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                expMediFilter.IS_EXPORT = true; 
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager().GetView(expMediFilter); 

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00075"); 
                }

                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    ListRdo.AddRange((from r in hisImpMestMedicine select new Mrs00075RDO(r)).ToList()); 
                }
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    ListRdo.AddRange((from r in hisExpMestMedicine select new Mrs00075RDO(r)).ToList()); 
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
                //if (IsNotNullOrEmpty(ListRdo))
                {
                    ProcessBeginAndEndAmount(); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessBeginAndEndAmount()
        {
            try
            {
                CommonParam paramGet = new CommonParam();
                if (castFilter.MEDI_STOCK_IDs != null)
                {
                    foreach (var item in castFilter.MEDI_STOCK_IDs)
                    {

                        ProcessGetPeriod(paramGet, item);
                    }
                }
                else if (castFilter.MEDI_STOCK_ID != null)
                {

                    ProcessGetPeriod(paramGet, castFilter.MEDI_STOCK_ID ?? 0);
                }
                else
                {
                    foreach (var item in HisMediStockCFG.HisMediStocks.Select(o => o.ID).ToList())
                    {

                        ProcessGetPeriod(paramGet, item);
                    }
                }
                ListRdo = ListRdo.OrderBy(o => o.EXECUTE_TIME).ToList(); 
                ListRdo = ListRdo.GroupBy(g => g.EXECUTE_DATE_STR).Select(s => new Mrs00075RDO { EXECUTE_DATE_STR = s.First().EXECUTE_DATE_STR, EXP_AMOUNT = s.Sum(s1 => s1.EXP_AMOUNT), IMP_AMOUNT = s.Sum(s2 => s2.IMP_AMOUNT) }).ToList(); 
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
        private void ProcessGetPeriod(CommonParam paramGet,long mediStockId)
        {
            try
            {
                HisMediStockPeriodViewFilterQuery periodFilter = new HisMediStockPeriodViewFilterQuery(); 
                periodFilter.TO_TIME_TO = castFilter.TIME_FROM; 
                periodFilter.MEDI_STOCK_ID = mediStockId; 
                List<V_HIS_MEDI_STOCK_PERIOD> hisMediStockPeriod = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(paramGet).GetView(periodFilter); 
                if (!paramGet.HasException)
                {
                    if (IsNotNullOrEmpty(hisMediStockPeriod))
                    {
                        //Trường hợp có kỳ được chốt gần nhất
                        V_HIS_MEDI_STOCK_PERIOD neighborPeriod = hisMediStockPeriod.OrderByDescending(d => d.TO_TIME).ToList()[0]; 
                        if (neighborPeriod != null)
                        {
                            ProcessBeinAmountMedicineByMediStockPeriod(paramGet, neighborPeriod, mediStockId); 
                        }
                    }
                    else
                    {
                        // Trường hợp không có kỳ chốt gần nhât - chưa được chốt kỳ nào
                        ProcessBeinAmountMedicineNotMediStockPriod(paramGet, mediStockId); 
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
        private void ProcessBeinAmountMedicineByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod, long mediStockId)
        {
            try
            {
                HisMestPeriodMetyViewFilterQuery periodMetyFilter = new HisMestPeriodMetyViewFilterQuery(); 
                periodMetyFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID; 
                periodMetyFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                List<V_HIS_MEST_PERIOD_METY> hisMestPeriodMety = new MOS.MANAGER.HisMestPeriodMety.HisMestPeriodMetyManager(paramGet).GetView(periodMetyFilter); 
                if (IsNotNullOrEmpty(hisMestPeriodMety))
                {
                    startBeginAmount = hisMestPeriodMety.First().VIR_END_AMOUNT ?? 0; 
                }
                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery(); 
                impMediFilter.IMP_TIME_FROM = neighborPeriod.TO_TIME + 1; 
                impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1; 
                impMediFilter.MEDI_STOCK_ID = mediStockId; 
                impMediFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMediFilter); 
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    startBeginAmount += hisImpMestMedicine.Sum(s => s.AMOUNT); 
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = neighborPeriod.TO_TIME; 
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1; 
                expMediFilter.MEDI_STOCK_ID = mediStockId; 
                expMediFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                expMediFilter.IS_EXPORT = true; 
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMediFilter); 
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
        private void ProcessBeinAmountMedicineNotMediStockPriod(CommonParam paramGet, long mediStockId)
        {
            try
            {
                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery(); 
                impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1; 
                impMediFilter.MEDI_STOCK_ID = mediStockId; 
                impMediFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager().GetView(impMediFilter); 
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    startBeginAmount += hisImpMestMedicine.Sum(s => s.AMOUNT); 
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery(); 
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1; 
                expMediFilter.MEDI_STOCK_ID = mediStockId; 
                expMediFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                expMediFilter.IS_EXPORT = true; 
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager().GetView(expMediFilter); 
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                V_HIS_MEDICINE_TYPE medicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager().GetViewById(castFilter.MEDICINE_TYPE_ID); 
                if (medicineType != null)
                {
                    dicSingleTag.Add("MEDICINE_TYPE_CODE", medicineType.MEDICINE_TYPE_CODE); 
                    dicSingleTag.Add("MEDICINE_TYPE_NAME", medicineType.MEDICINE_TYPE_NAME); 
                    dicSingleTag.Add("SERVICE_UNIT_NAME", medicineType.SERVICE_UNIT_NAME); 
                    dicSingleTag.Add("NATIONAL_NAME", medicineType.NATIONAL_NAME); 
                    dicSingleTag.Add("MANUFACTURER_NAME", medicineType.MANUFACTURER_NAME); 
                }

                HIS_MEDI_STOCK mediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager().GetById(castFilter.MEDI_STOCK_ID??0); 
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
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
