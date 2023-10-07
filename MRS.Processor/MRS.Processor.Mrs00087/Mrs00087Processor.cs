using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00087
{
    public class Mrs00087Processor : AbstractProcessor
    {
        Mrs00087Filter castFilter = null;
        List<Mrs00087RDO> ListRdo = new List<Mrs00087RDO>();
        List<V_HIS_TRANSACTION> ListCurrentBill = new List<V_HIS_TRANSACTION>();

        public Mrs00087Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00087Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00087Filter)this.reportFilter);

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
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(ListCurrentBill))
                {
                    ListRdo = (from r in ListCurrentBill select new Mrs00087RDO(r)).ToList();
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

        private void ProcessListCurrentBill()
        {
            try
            {
                if (ListCurrentBill != null && ListCurrentBill.Count > 0)
                {
                    ListCurrentBill = ListCurrentBill.Where(o => o.IS_CANCEL != 1 && /* o.IS_TRANSFER_ACCOUNTING != 1 &&*/ o.AMOUNT <= 200000).ToList().OrderBy(o => o.TRANSACTION_CODE).ToList();
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
                HisTransactionViewFilterQuery filter = new HisTransactionViewFilterQuery();
                filter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                filter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                filter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                ListCurrentBill = new HisTransactionManager().GetView(filter);
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
