using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMestPeriodMaty;
using MOS.MANAGER.HisMediStockPeriod;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00082
{
    public class Mrs00082Processor : AbstractProcessor
    {
        Mrs00082Filter castFilter = null; 
        List<Mrs00082RDO> listRdo = new List<Mrs00082RDO>(); 
        private string MEDI_STOCK_PERIOD_CODE; 
        private string MEDI_STOCK_PERIOD_NAME; 
        private string MEDI_STOCK_CODE; 
        private string MEDI_STOCK_NAME; 
        List<V_HIS_MEST_PERIOD_MATY> hisMestPeriodMaty; 

        public Mrs00082Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00082Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                castFilter = ((Mrs00082Filter)this.reportFilter); 

                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu tu db ve: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                CommonParam paramGet = new CommonParam(); 
                HisMestPeriodMatyViewFilterQuery filter = new HisMestPeriodMatyViewFilterQuery(); 
                filter.MEDI_STOCK_PERIOD_ID = castFilter.MEDI_STOCK_PERIOD_ID; 
                hisMestPeriodMaty = new MOS.MANAGER.HisMestPeriodMaty.HisMestPeriodMatyManager(paramGet).GetView(filter); 
                var MediStocPeriod = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).GetViewById(castFilter.MEDI_STOCK_PERIOD_ID); 

                Inventec.Common.Logging.LogSystem.Debug("Ket thuc lay du lieu tu Db: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => hisMestPeriodMaty), hisMestPeriodMaty)); 
                if (!paramGet.HasException)
                {
                    hisMestPeriodMaty = hisMestPeriodMaty.Where(o => o.BEGIN_AMOUNT > 0 || o.IN_AMOUNT > 0 || o.OUT_AMOUNT > 0).ToList(); 
                    //MEDI_STOCK_PERIOD_CODE = MediStocPeriod.MEDI_STOCK_PERIOD_CODE; 
                    MEDI_STOCK_PERIOD_NAME = MediStocPeriod.MEDI_STOCK_PERIOD_NAME; 
                    MEDI_STOCK_CODE = MediStocPeriod.MEDI_STOCK_CODE; 
                    MEDI_STOCK_NAME = MediStocPeriod.MEDI_STOCK_NAME; 
                    result = true; 
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
            bool result = false; 
            try
            {
                if (IsNotNullOrEmpty(hisMestPeriodMaty))
                {
                    listRdo = (from data in hisMestPeriodMaty select new Mrs00082RDO(data)).ToList(); 
                    result = true; 
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
                dicSingleTag.Add("MEDI_STOCK_PERIOD_NAME", MEDI_STOCK_PERIOD_NAME); 
                dicSingleTag.Add("MEDI_STOCK_CODE_AND_NAME", MEDI_STOCK_CODE + " - " + MEDI_STOCK_NAME); 

                objectTag.AddObjectData(store, "Report", listRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
