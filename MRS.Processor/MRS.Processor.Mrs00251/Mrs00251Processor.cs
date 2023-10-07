using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 
using MOS.Filter; 
using MOS.MANAGER.HisInvoiceBook;
using MRS.MANAGER.Core.MrsReport.Lib; 

namespace MRS.Processor.Mrs00251
{
    public class Mrs00251Processor : AbstractProcessor
    {
        private List<Mrs00251RDO> ListRdo = new List<Mrs00251RDO>(); 
        private List<V_HIS_INVOICE> ListInvoice = new List<V_HIS_INVOICE>(); 
        private List<V_HIS_INVOICE_BOOK> ListInvoiceBook = new List<V_HIS_INVOICE_BOOK>();
        CommonParam paramGet = new CommonParam();
        System.Data.DataTable listData = new System.Data.DataTable();
        public Mrs00251Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00251Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                
                listData = new ManagerSql().GetSum(((Mrs00251Filter)this.reportFilter), MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15));
                if (listData != null && listData.Rows != null && listData.Rows.Count > 0)
                {
                    return true;
                }
                HisInvoiceViewFilterQuery InvoiceViewFilter = new HisInvoiceViewFilterQuery()
                {
                    CREATE_TIME_FROM = ((Mrs00251Filter)this.reportFilter).TIME_FROM,//if (CREATE_TIME is null) TIME=CREATE_TIME; 
                    //CREATE_TIME_TO = ((Mrs00251Filter)this.reportFilter).TIME_TO
                }; 
                ListInvoice = new HisInvoiceManager(paramGet).GetView(InvoiceViewFilter); 


                HisInvoiceBookViewFilterQuery InvoiceBookViewFilter = new HisInvoiceBookViewFilterQuery()
                {
                    CREATE_TIME_FROM = ((Mrs00251Filter)this.reportFilter).TIME_FROM - 100000000000,//if (CREATE_TIME is null) TIME=CREATE_TIME; 
                    //CREATE_TIME_TO = ((Mrs00251Filter)this.reportFilter).TIME_TO
                }; 
                ListInvoiceBook = new HisInvoiceBookManager(paramGet).GetView(InvoiceBookViewFilter); 
                //ListInvoice = ListInvoice.Select(o => new V_HIS_INVOICE { TEMPLATE_CODE = ListInvoiceBook.Where(p => p.ID == o.INVOICE_BOOK_ID).First().TEMPLATE_CODE, SYMBOL_CODE = ListInvoiceBook.Where(p => p.ID == o.INVOICE_BOOK_ID).First().SYMBOL_CODE, TOTAL = o.TOTAL, VIR_NUM_ORDER = o.VIR_NUM_ORDER, IS_CANCEL = o.IS_CANCEL, IS_DELETE = o.IS_DELETE }).ToList(); 
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
            var result = true; 
            try
            {
                var filter = (Mrs00251Filter)this.reportFilter;

                if (listData != null && listData.Rows != null && listData.Rows.Count > 0)
                {
                    return true;
                }
                ListRdo.Clear(); 
                var groupByReport = ListInvoiceBook.GroupBy(o => new { o.TEMPLATE_CODE, o.SYMBOL_CODE }).ToList(); 

                foreach (var group in groupByReport)
                {
                    List<V_HIS_INVOICE_BOOK> ListSub = group.ToList<V_HIS_INVOICE_BOOK>(); 
                    //(3):
                    var createdTimeBefore = ListSub.Where(o => o.CREATE_TIME < filter.TIME_FROM).ToList().Count == 0 ? 0 : ListSub.Where(o => o.CREATE_TIME < filter.TIME_FROM).ToList().Sum(p => p.TOTAL) + ListSub.Where(o => o.CREATE_TIME < filter.TIME_FROM).ToList().OrderBy(q => q.CREATE_TIME).First().FROM_NUM_ORDER - 1; 
                    //(4):
                    var BUY_NUM_ORDER_FROM = ListSub.Where(o => o.CREATE_TIME > filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO).ToList().Count == 0 ? 0 : ListSub.Where(o => o.CREATE_TIME > filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO).ToList().OrderBy(p => p.FROM_NUM_ORDER).First().FROM_NUM_ORDER; //Cái đã tạo trong kì "từ"
                    //(5):
                    var BUY_NUM_ORDER_TO = BUY_NUM_ORDER_FROM == 0 ? 0 : BUY_NUM_ORDER_FROM + ListSub.Where(o => o.CREATE_TIME > filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO).ToList().Sum(p => p.TOTAL) - 1;  //Cái đã tạo trong kì "đến"
                    //(7):
                    var ListInvoiceThis = ListInvoice.Where(o => o.TEMPLATE_CODE == ListSub.First().TEMPLATE_CODE && o.SYMBOL_CODE == ListSub.First().SYMBOL_CODE).ToList(); 
                    var TOTAL_USED_NUM_ORDER_FROM = ListInvoiceThis.Where(o => o.CREATE_TIME >= filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO).ToList().Count == 0 ? 0 : ListInvoiceThis.Where(o => o.CREATE_TIME >= filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO).ToList().OrderBy(p => p.NUM_ORDER).First().NUM_ORDER; //Cái đã sử dụng trong kì "từ"
                    //(1):
                    Decimal BEGIN_NUM_ORDER_FROM = 0; 
                    if (TOTAL_USED_NUM_ORDER_FROM > 0)
                    {
                        if (BUY_NUM_ORDER_FROM == 0 || (BUY_NUM_ORDER_FROM > 0 && TOTAL_USED_NUM_ORDER_FROM < BUY_NUM_ORDER_FROM)) BEGIN_NUM_ORDER_FROM = TOTAL_USED_NUM_ORDER_FROM; 
                        else
                            BEGIN_NUM_ORDER_FROM = 0; 

                    }
                    else
                        BEGIN_NUM_ORDER_FROM = ListSub.Where(o => o.CURRENT_NUM_ORDER != 0).ToList().Count == 0 ? (ListSub.Where(o => o.CREATE_TIME < filter.TIME_FROM).ToList().Count == 0 ? 0 : ListSub.First().FROM_NUM_ORDER) : (ListSub.Where(o => o.CURRENT_NUM_ORDER != 0).ToList().OrderBy(p => p.CURRENT_NUM_ORDER).Last().CURRENT_NUM_ORDER < createdTimeBefore ? (Decimal)(ListSub.Where(o => o.CURRENT_NUM_ORDER != 0).ToList().OrderBy(p => p.CURRENT_NUM_ORDER).Last().CURRENT_NUM_ORDER + 1) : 0); //cái chưa dùng trước kì "từ"
                    //(2):
                    var BEGIN_NUM_ORDER_TO = BEGIN_NUM_ORDER_FROM == 0 ? 0 : createdTimeBefore; //cái chưa dùng trước kì "đến"
                    //(6):
                    var A = BUY_NUM_ORDER_FROM == 0 ? 0 : BUY_NUM_ORDER_TO - BUY_NUM_ORDER_FROM + 1; 
                    var B = BEGIN_NUM_ORDER_TO == 0 ? 0 : BEGIN_NUM_ORDER_TO - BEGIN_NUM_ORDER_FROM + 1; 

                    var TOTAL = A + B;  //tổng cái chưa dùng trước kì báo cáo.

                    //(8):
                    var TOTAL_USED_NUM_ORDER_TO = TOTAL_USED_NUM_ORDER_FROM == 0 ? 0 : ListInvoice.Where(o => o.TEMPLATE_CODE == ListSub.First().TEMPLATE_CODE && o.SYMBOL_CODE == ListSub.First().SYMBOL_CODE && o.CREATE_TIME >= filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO).ToList().OrderBy(p => p.NUM_ORDER).Last().NUM_ORDER; //Cái đã sử dụng trong kì "đến"
                    var TOTAL_USERED_COUNT = TOTAL_USED_NUM_ORDER_FROM == 0 ? 0 : TOTAL_USED_NUM_ORDER_TO - TOTAL_USED_NUM_ORDER_FROM + 1; //tống cái đã sử dụng trong kì
                    var USERED_COUNT = ListInvoice.Where(o => o.TEMPLATE_CODE == ListSub.First().TEMPLATE_CODE && o.SYMBOL_CODE == ListSub.First().SYMBOL_CODE && o.CREATE_TIME >= filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO && o.IS_ACTIVE == 1).ToList().Count; //tống cái đã thành công trong kì
                    var DEL_COUNT = ListInvoice.Where(o => o.TEMPLATE_CODE == ListSub.First().TEMPLATE_CODE && o.SYMBOL_CODE == ListSub.First().SYMBOL_CODE && o.CREATE_TIME >= filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO && o.IS_DELETE == 1).ToList().Count; //tống cái đã xóa trong kì
                    var CAN_COUNT = ListInvoice.Where(o => o.TEMPLATE_CODE == ListSub.First().TEMPLATE_CODE && o.SYMBOL_CODE == ListSub.First().SYMBOL_CODE && o.CREATE_TIME >= filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO && o.IS_CANCEL == 1).ToList().Count; //tống cái đã hủy trong kì
                    var DEL_VIR_NUM_ORDERs = ListInvoice.Where(o => o.TEMPLATE_CODE == ListSub.First().TEMPLATE_CODE && o.SYMBOL_CODE == ListSub.First().SYMBOL_CODE && o.CREATE_TIME >= filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO && o.IS_DELETE == 1).ToList().Select(p => p.VIR_NUM_ORDER); //những cái đã xóa
                    var CAN_VIR_NUM_ORDERs = ListInvoice.Where(o => o.TEMPLATE_CODE == ListSub.First().TEMPLATE_CODE && o.SYMBOL_CODE == ListSub.First().SYMBOL_CODE && o.CREATE_TIME >= filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO && o.IS_CANCEL == 1).ToList().Select(p => p.VIR_NUM_ORDER); //những cái đã hủy

                    Decimal? END_NUM_ORDER_FROM = 0, END_NUM_ORDER_TO = 0; 
                    Decimal? end = BUY_NUM_ORDER_TO > BEGIN_NUM_ORDER_TO ? BUY_NUM_ORDER_TO : BEGIN_NUM_ORDER_TO; 
                    END_NUM_ORDER_FROM = ListInvoice.Where(o => o.TEMPLATE_CODE == ListSub.First().TEMPLATE_CODE && o.SYMBOL_CODE == ListSub.First().SYMBOL_CODE && o.CREATE_TIME >= filter.TIME_TO).ToList().Count > 0 ? ListInvoice.Where(o => o.TEMPLATE_CODE == ListSub.First().TEMPLATE_CODE && o.SYMBOL_CODE == ListSub.First().SYMBOL_CODE && o.CREATE_TIME >= filter.TIME_TO).ToList().OrderBy(p => p.NUM_ORDER).First().NUM_ORDER : (ListSub.Where(o => o.CURRENT_NUM_ORDER != 0).ToList().Count == 0 ? (ListSub.Where(o => o.CREATE_TIME > filter.TIME_FROM && o.CREATE_TIME < filter.TIME_TO).ToList().Count == 0 ? BEGIN_NUM_ORDER_FROM : ListSub.OrderBy(o => o.CREATE_TIME).First().FROM_NUM_ORDER) : (ListSub.Where(o => o.CURRENT_NUM_ORDER != 0).ToList().OrderBy(p => p.CURRENT_NUM_ORDER).Last().CURRENT_NUM_ORDER < end ? ListSub.Where(o => o.CURRENT_NUM_ORDER != 0).ToList().OrderBy(p => p.CURRENT_NUM_ORDER).Last().CURRENT_NUM_ORDER + 1 : 0)); //cái chưa dùng trong kì "từ"
                    END_NUM_ORDER_TO = END_NUM_ORDER_FROM == 0 ? 0 : end; //cái chưa dùng trong kì "đến"

                    var END_NUM = END_NUM_ORDER_FROM == 0 ? 0 : END_NUM_ORDER_TO - END_NUM_ORDER_FROM + 1; //tổng cái chưa dùng trong kì báo cáo.

                    Mrs00251RDO rdo = new Mrs00251RDO(); 

                    rdo.TEMPLATE_CODE = ListSub.First().TEMPLATE_CODE; 
                    rdo.SYMBOL_CODE = ListSub.First().SYMBOL_CODE; 
                    rdo.TOTAL = (Decimal)TOTAL; 
                    rdo.BEGIN_VIR_NUM_ORDER_FROM = BEGIN_NUM_ORDER_FROM == 0 ? "" : string.Format("{0:0000000}", Convert.ToInt64(BEGIN_NUM_ORDER_FROM)); 
                    rdo.BEGIN_VIR_NUM_ORDER_TO = BEGIN_NUM_ORDER_TO == 0 ? "" : string.Format("{0:0000000}", Convert.ToInt64(BEGIN_NUM_ORDER_TO)); 
                    rdo.BUY_VIR_NUM_ORDER_FROM = BUY_NUM_ORDER_FROM == 0 ? "" : string.Format("{0:0000000}", Convert.ToInt64(BUY_NUM_ORDER_FROM)); 
                    rdo.BUY_VIR_NUM_ORDER_TO = BUY_NUM_ORDER_TO == 0 ? "" : string.Format("{0:0000000}", Convert.ToInt64(BUY_NUM_ORDER_TO)); 
                    rdo.TOTAL_USERED_VIR_NUM_ORDER_FROM = TOTAL_USED_NUM_ORDER_FROM == 0 ? "" : string.Format("{0:0000000}", Convert.ToInt64(TOTAL_USED_NUM_ORDER_FROM)); 
                    rdo.TOTAL_USERED_VIR_NUM_ORDER_TO = TOTAL_USED_NUM_ORDER_TO == 0 ? "" : string.Format("{0:0000000}", Convert.ToInt64(TOTAL_USED_NUM_ORDER_TO)); 
                    rdo.TOTAL_USERED_COUNT = TOTAL_USERED_COUNT; 
                    rdo.USERED_COUNT = USERED_COUNT; 
                    rdo.DEL_COUNT = DEL_COUNT; 
                    rdo.CAN_COUNT = CAN_COUNT; 
                    rdo.END_VIR_NUM_ORDER_FROM = END_NUM_ORDER_FROM == 0 ? "" : string.Format("{0:0000000}", Convert.ToInt64(END_NUM_ORDER_FROM)); 
                    rdo.END_VIR_NUM_ORDER_TO = END_NUM_ORDER_TO == 0 ? "" : string.Format("{0:0000000}", Convert.ToInt64(END_NUM_ORDER_TO)); 

                    rdo.DEL_VIR_NUM_ORDERs = string.Join(", ", DEL_VIR_NUM_ORDERs); 
                    rdo.CAN_VIR_NUM_ORDERs = string.Join(", ", CAN_VIR_NUM_ORDERs); 

                    rdo.END_NUM = (Decimal)END_NUM; 
                    if (!(BUY_NUM_ORDER_FROM == 0 && TOTAL_USED_NUM_ORDER_FROM == 0))
                        ListRdo.Add(rdo); 
                }
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00251Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00251Filter)this.reportFilter).TIME_TO));
            if (listData != null && listData.Rows != null && listData.Rows.Count > 0)
            {
                objectTag.AddObjectData(store, "Report", listData);
                return;
            }
            objectTag.AddObjectData(store, "Report", ListRdo); 
        }
    }
}
