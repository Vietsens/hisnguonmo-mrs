using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00056
{
    public class Mrs00056Processor : AbstractProcessor
    {
        Mrs00056Filter castFilter = null;
        List<Mrs00056RDO> ListRdo = new List<Mrs00056RDO>();
        List<HIS_TRANSACTION> ListCurrentBill = new List<HIS_TRANSACTION>();

        public Mrs00056Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00056Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00056Filter)this.reportFilter);
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00056 " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                LoadDataToRam();
                ProcessListCurrentBill();
                result = true;
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
                if (IsNotNullOrEmpty(ListCurrentBill))
                {
                    ListRdo = (from r in ListCurrentBill select new Mrs00056RDO(r)).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListCurrentBill()
        {
            try
            {
                if (ListCurrentBill != null && ListCurrentBill.Count > 0)
                {
                    ListCurrentBill = ListCurrentBill.Where(o => o.IS_CANCEL != 1 && /* o.IS_TRANSFER_ACCOUNTING != 1 &&*/ o.AMOUNT > 200000).ToList().OrderBy(o => o.TRANSACTION_CODE).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListCurrentBill.Clear();
            }
        }

        private void LoadDataToRam()
        {
            try
            {
                HisTransactionFilterQuery filter = new HisTransactionFilterQuery();
                filter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                filter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                ListCurrentBill = new HisTransactionManager().Get(filter);
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
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                ListRdo = ListRdo.OrderBy(o => o.CREATE_TIME).ThenBy(t => t.TRANSACTION_CODE).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
