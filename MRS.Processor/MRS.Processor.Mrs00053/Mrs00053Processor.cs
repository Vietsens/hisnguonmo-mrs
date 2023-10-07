using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00053
{
    public class Mrs00053Processor : AbstractProcessor
    {
        Mrs00053Filter castFilter = null;
        List<Mrs00053RDO> ListRdo = new List<Mrs00053RDO>();
        List<V_HIS_TRANSACTION> ListCurrentBill = new List<V_HIS_TRANSACTION>();

        string cashier_Username = "";

        public Mrs00053Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00053Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00053Filter)this.reportFilter);
                    LoadDataToRam();
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
                ProcessListCurrentBill();
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
                    ListCurrentBill = ListCurrentBill.Where(o =>o.IS_CANCEL != 1 && o.AMOUNT <= 200000).ToList().OrderBy(o => o.TRANSACTION_CODE).ToList();
                    if (ListCurrentBill.Count > 0)
                    {
                        cashier_Username = castFilter.CASHIER_LOGINNAME + castFilter.LOGINNAME + " - " + ListCurrentBill[0].CASHIER_USERNAME;
                        foreach (var bill in ListCurrentBill)
                        {
                            Mrs00053RDO rdo = new Mrs00053RDO();
                            rdo.TRANSACTION_CODE = bill.TRANSACTION_CODE;
                            rdo.PATIENT_CODE = bill.TDL_PATIENT_CODE;
                            rdo.VIR_PATIENT_NAME = bill.TDL_PATIENT_NAME;
                            rdo.AMOUNT = bill.AMOUNT;
                            rdo.DESCRIPTION = bill.DESCRIPTION;
                            rdo.CREATE_TIME = bill.TRANSACTION_TIME;
                            rdo.CREATE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(bill.TRANSACTION_TIME);
                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void LoadDataToRam()
        {
            try
            {
                HisTransactionViewFilterQuery filter = new HisTransactionViewFilterQuery();
                filter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                filter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                filter.TRANSACTION_TYPE_IDs = new List<long> { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                ListCurrentBill = new HisTransactionManager().GetView(filter);

                if (castFilter.CASHIER_LOGINNAME != null)
                {
                    ListCurrentBill = ListCurrentBill.Where(o => castFilter.CASHIER_LOGINNAME == o.CASHIER_LOGINNAME).ToList();
                }
                if (castFilter.LOGINNAME != null)
                {
                    ListCurrentBill = ListCurrentBill.Where(o => castFilter.LOGINNAME == o.CASHIER_LOGINNAME).ToList();
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
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                dicSingleTag.Add("CASHIER_USERNAME", cashier_Username);

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
