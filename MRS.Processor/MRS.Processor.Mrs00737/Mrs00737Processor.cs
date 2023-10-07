using System;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisAccountBook;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartment;

namespace MRS.Processor.Mrs00737
{
    public class Mrs00737Processor : AbstractProcessor
    {
        private const short BILL_TYPE_ID__NORMAL = 1;
        private const short BILL_TYPE_ID__SERVICE = 2;
        Mrs00737Filter filter = null;
        private List<Mrs00737RDO> listRdo = new List<Mrs00737RDO>();
        private List<Mrs00737RDO> listRdoCancel = new List<Mrs00737RDO>();
        private List<HIS_TREATMENT> ListHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_TRANSACTION> ListHisTransaction = new List<HIS_TRANSACTION>();
        List<HIS_SERE_SERV> ListHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_ACCOUNT_BOOK> ListHisAccountBook = new List<HIS_ACCOUNT_BOOK>();
        List<ACS_USER> acsUsers = new List<ACS_USER>();

        List<HIS_PAY_FORM> listPayForm = new List<HIS_PAY_FORM>();
        List<PAY_FORM_AGGREGATE> listPayFormAggregate = new List<PAY_FORM_AGGREGATE>();
        List<PAY_FORM_AGGREGATE> listPayFormAggregateCancel = new List<PAY_FORM_AGGREGATE>();

        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();

        public Mrs00737Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00737Filter);
        }

        protected override bool GetData()
        {
           
            filter = (Mrs00737Filter)base.reportFilter;
            bool result = true;
            CommonParam val = new CommonParam();
            try
            {
                listRdo = new ManagerSql().GetBillAmount((Mrs00737Filter)base.reportFilter);
                listRdoCancel = new ManagerSql().GetBillCancel((Mrs00737Filter)base.reportFilter);

                //get payform
                GetPayForm();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetPayForm()
        {
            listPayForm = HisPayFormCFG.ListPayForm;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                foreach (Mrs00737RDO item in listRdo)
                {
                    item.BILL_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TRANSACTION_DATE ?? 0);
                    item.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.IN_TIME ?? 0);
                    item.DIC_PAY_FORM_CODE = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, decimal>>(item.JSON_PAY_FORM_CODE);
                }
                foreach (Mrs00737RDO item2 in listRdoCancel)
                {
                    item2.BILL_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item2.TRANSACTION_DATE ?? 0);
                    item2.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item2.IN_TIME ?? 0);
                    item2.DIC_PAY_FORM_CODE = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, decimal>>(item2.JSON_PAY_FORM_CODE);
                }

                AggregatePayForm();
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void AggregatePayForm()
        {
            foreach (var item in listPayForm)
            {
                listPayFormAggregate.Add(CreateRdo(item, listRdo.Where(o => o.DIFF >= 0).ToList(), true));
                listPayFormAggregate.Add(CreateRdo(item, listRdo.Where(o => !(o.DIFF >= 0)).ToList(), false));
                listPayFormAggregateCancel.Add(CreateRdo(item, listRdoCancel.Where(o => o.DIFF >= 0).ToList(), true));
                listPayFormAggregateCancel.Add(CreateRdo(item, listRdoCancel.Where(o => !(o.DIFF >= 0)).ToList(), false));
                 
            }
        }
        PAY_FORM_AGGREGATE CreateRdo(HIS_PAY_FORM payForm, List<Mrs00737RDO> listRdo, bool IsHasDif)
        {
            PAY_FORM_AGGREGATE rdo = new PAY_FORM_AGGREGATE();
            rdo.PAY_FORM_CODE = payForm.PAY_FORM_CODE;
            rdo.PAY_FORM_NAME = payForm.PAY_FORM_NAME;
            rdo.IS_HAS_DIFF = IsHasDif;
            rdo.AMOUNT = listRdo.Where(o=>o.PAY_FORM_NORMAL_CODE == payForm.PAY_FORM_CODE).Sum(s => s.BILL_AMOUNT_VP ?? 0)+listRdo.Where(o=>o.PAY_FORM_SERVICE_CODE == payForm.PAY_FORM_CODE).Sum(s => s.BILL_AMOUNT_DV ?? 0);
            return rdo;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00737Filter)base.reportFilter).FEE_LOCK_TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00737Filter)base.reportFilter).FEE_LOCK_TIME_TO));
            
            objectTag.AddObjectData(store, "Report", listRdo);

            objectTag.AddObjectData(store, "ReportMore0", listRdo.Where(o => o.DIFF >= 0).ToList());
            objectTag.AddObjectData(store, "CashierMore0", listRdo.Where(o => o.DIFF >= 0).GroupBy(o => o.BILL_CASHIER_LOGINNAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "CashierMore0", "ReportMore0", "BILL_CASHIER_LOGINNAME", "BILL_CASHIER_LOGINNAME");

            objectTag.AddObjectData(store, "ReportLeft0", listRdo.Where(o => o.DIFF < 0).ToList());
            objectTag.AddObjectData(store, "CashierLeft0", listRdo.Where(o => o.DIFF < 0).GroupBy(o => o.BILL_CASHIER_LOGINNAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "CashierLeft0", "ReportLeft0", "BILL_CASHIER_LOGINNAME", "BILL_CASHIER_LOGINNAME");

            objectTag.AddObjectData(store, "ReportCancel", listRdoCancel);

            objectTag.AddObjectData(store, "ReportCancelMore0", listRdoCancel.Where(o => o.DIFF >= 0 ).ToList());
            objectTag.AddObjectData(store, "CashierCancelMore0", listRdoCancel.Where(o => o.DIFF >= 0 ).GroupBy(o => o.BILL_CASHIER_LOGINNAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "CashierCancelMore0", "ReportCancelMore0", "BILL_CASHIER_LOGINNAME", "BILL_CASHIER_LOGINNAME");

            objectTag.AddObjectData(store, "ReportCancelLeft0", listRdoCancel.Where(o => o.DIFF < 0 ).ToList());
            objectTag.AddObjectData(store, "CashierCancelLeft0", listRdoCancel.Where(o => o.DIFF < 0 ).GroupBy(o => o.BILL_CASHIER_LOGINNAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "CashierCancelLeft0", "ReportCancelLeft0", "BILL_CASHIER_LOGINNAME", "BILL_CASHIER_LOGINNAME");

            //tổng hợp theo hình thức thanh toán

            objectTag.AddObjectData(store, "PfMore0", listPayFormAggregate.Where(o => o.IS_HAS_DIFF).ToList());
            objectTag.AddObjectData(store, "PfLeft0", listPayFormAggregate.Where(o => !o.IS_HAS_DIFF).ToList());
            objectTag.AddObjectData(store, "PfCancelMore0", listPayFormAggregateCancel.Where(o => o.IS_HAS_DIFF).ToList());
            objectTag.AddObjectData(store, "PfCancelLeft0", listPayFormAggregateCancel.Where(o => !o.IS_HAS_DIFF).ToList());
        }
    }
}
