using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMestPeriodMety;
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

namespace MRS.Processor.Mrs00081
{
    public class Mrs00081Processor : AbstractProcessor
    {
        Mrs00081Filter castFilter = null;
        List<Mrs00081RDO> listRdo = new List<Mrs00081RDO>();
        private string MEDI_STOCK_PERIOD_CODE;
        private string MEDI_STOCK_PERIOD_NAME;
        private string MEDI_STOCK_CODE;
        private string MEDI_STOCK_NAME;
        List<V_HIS_MEST_PERIOD_METY> hisMestPeriodMety;

        public Mrs00081Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00081Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00081Filter)this.reportFilter);
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu tu db ve: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                if (castFilter != null)
                {
                    CommonParam paramGet = new CommonParam();
                    HisMestPeriodMetyViewFilterQuery filter = new HisMestPeriodMetyViewFilterQuery();
                    filter.MEDI_STOCK_PERIOD_ID = castFilter.MEDI_STOCK_PERIOD_ID;
                    hisMestPeriodMety = new MOS.MANAGER.HisMestPeriodMety.HisMestPeriodMetyManager(paramGet).GetView(filter);
                    var MediStocPeriod = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).GetViewById(castFilter.MEDI_STOCK_PERIOD_ID);
                    if (!paramGet.HasException)
                    {
                        //MEDI_STOCK_PERIOD_CODE = MediStocPeriod.MEDI_STOCK_PERIOD_CODE; 
                        MEDI_STOCK_PERIOD_NAME = MediStocPeriod.MEDI_STOCK_PERIOD_NAME;
                        MEDI_STOCK_CODE = MediStocPeriod.MEDI_STOCK_CODE;
                        MEDI_STOCK_NAME = MediStocPeriod.MEDI_STOCK_NAME;
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info("Ket thuc lay du lieu tu Db: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => hisMestPeriodMety), hisMestPeriodMety));
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
                if (IsNotNullOrEmpty(hisMestPeriodMety))
                {
                    hisMestPeriodMety = hisMestPeriodMety.Where(o => o.BEGIN_AMOUNT > 0 || o.IN_AMOUNT > 0 || o.OUT_AMOUNT > 0).ToList();
                    listRdo = (from data in hisMestPeriodMety select new Mrs00081RDO(data)).ToList();
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
