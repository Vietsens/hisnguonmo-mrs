using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using MOS.MANAGER.HisInvoiceBook;
using ACS.Filter;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceBook;
using MOS.MANAGER.HisInvoiceDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00152
{
    public class Mrs00152Processor : AbstractProcessor
    {
        private Mrs00152Filter filter;
        private List<Mrs00152RDO> listMrs00152Rdos = new List<Mrs00152RDO>();
        private List<HIS_INVOICE_DETAIL> listInvoiceDetailViews;
        private List<V_HIS_INVOICE> listInvoiceViews;
        private List<HIS_INVOICE_BOOK> hisInvoiceBook;
        private List<string> CREATORs;


        public Mrs00152Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00152Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00152Filter)this.reportFilter);
            var result = true;
            try
            {
                var paramGet = new CommonParam();
                //-------------------------------------------------------------------------------------------------- V_HIS_INVOICE

                this.CREATORs = ((Mrs00152Filter)this.reportFilter).LOGINNAMEs;
                var hisInvoiceBookfilter = new HisInvoiceBookFilterQuery();
                hisInvoiceBookfilter.ID = ((Mrs00152Filter)this.reportFilter).INVOICE_BOOK_ID;
                hisInvoiceBook = new HisInvoiceBookManager(paramGet).Get(hisInvoiceBookfilter);

                var metyFilterInvoice = new HisInvoiceViewFilterQuery
                {
                    CREATE_TIME_FROM = ((Mrs00152Filter)this.reportFilter).DATE_FROM,
                    CREATE_TIME_TO = ((Mrs00152Filter)this.reportFilter).DATE_TO,
                    INVOICE_BOOK_ID = ((Mrs00152Filter)this.reportFilter).INVOICE_BOOK_ID,
                    CREATORs = this.CREATORs
                };

                listInvoiceViews = new HisInvoiceManager(param).GetView(metyFilterInvoice);
                //-------------------------------------------------------------------------------------------------- HIS_INVOICE_DETAIL
                var listInvoiceIds = listInvoiceViews.Select(s => s.ID).ToList();
                listInvoiceDetailViews = new List<HIS_INVOICE_DETAIL>();
                var skip = 0;
                while (listInvoiceIds.Count - skip > 0)
                {
                    var listIds = listInvoiceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyInvoiceIds = new HisInvoiceDetailFilterQuery
                    {
                        INVOICE_IDs = listIds,
                    };
                    var invoiceDetailsViews = new HisInvoiceDetailManager(paramGet).Get(metyInvoiceIds);
                    listInvoiceDetailViews.AddRange(invoiceDetailsViews);
                }
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
                foreach (var listInvoiceView in listInvoiceViews.OrderBy(s => s.CREATE_TIME))
                {
                    var virTotalPrice = listInvoiceDetailViews.Where(s => s.INVOICE_ID == listInvoiceView.ID).Sum(s => s.VIR_TOTAL_PRICE);
                    var note = string.Empty;
                    if (listInvoiceView.IS_CANCEL != null)
                    {
                        note = string.Format("Hủy: (Lí do hủy - {0})", listInvoiceView.CANCEL_REASON);
                        virTotalPrice = 0;
                    }
                    var rdo = new Mrs00152RDO
                    {
                        NUM_ORDER = listInvoiceView.VIR_NUM_ORDER,
                        CREATE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listInvoiceView.CREATE_TIME.Value),
                        BUYER_NAME = listInvoiceView.BUYER_NAME,
                        SELLER_TAX_CODE = listInvoiceView.BUYER_TAX_CODE,
                        VIR_TOTAL_PRICE = virTotalPrice,
                        NOTE = note
                    };
                    listMrs00152Rdos.Add(rdo);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00152Filter)this.reportFilter).DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00152Filter)this.reportFilter).DATE_TO));
                if (IsNotNullOrEmpty(hisInvoiceBook))
                    dicSingleTag.Add("SYMBOL_CODE", hisInvoiceBook.First().SYMBOL_CODE);
                dicSingleTag.Add("USER_CREATE_INVOICE", (this.CREATORs != null && this.CREATORs.Count > 0) ? string.Join(", ", this.CREATORs) : "");
                dicSingleTag.Add("VIR_TOTAL_PATIENT_PRICE", listMrs00152Rdos.Sum(s => s.VIR_TOTAL_PRICE));
                dicSingleTag.Add("VIR_TOTAL_PATIENT_PRICE_STRING", Inventec.Common.String.Convert.CurrencyToVneseString(listMrs00152Rdos.Sum(s => s.VIR_TOTAL_PRICE).ToString()));
                objectTag.AddObjectData(store, "Report", listMrs00152Rdos);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
