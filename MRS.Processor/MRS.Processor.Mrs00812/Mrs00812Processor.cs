using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisAccountBook;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPayForm;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTransactionType;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00812
{
    public class Mrs00812Processor : AbstractProcessor
    {
        public Mrs00812Filter filter;
        public List<Mrs00812RDO> listRdo = new List<Mrs00812RDO>();
        public CommonParam commonParam = new CommonParam();
        public List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        public List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        public List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        public List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        public List<HIS_TRANSACTION> listTransaction = new List<HIS_TRANSACTION>();
        public List<HIS_SERVICE> listServiceParent = new List<HIS_SERVICE>();
        public List<HIS_TRANSACTION_TYPE> listTransactionType = new List<HIS_TRANSACTION_TYPE>();
        List<HIS_ACCOUNT_BOOK> listAccountBook = new List<HIS_ACCOUNT_BOOK>();
        List<HIS_PAY_FORM> listPayForm = new List<HIS_PAY_FORM>();

        public Mrs00812Processor(CommonParam param, string reportTypeName)
            : base(param, reportTypeName)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00812Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00812Filter)this.reportFilter;
            bool result = false;
            try
            {
                listRdo = new ManagerSql().Get(filter);
                var serviceIds = listRdo.Select(x => x.PARENT_ID ?? 0).Distinct().ToList();
                HisServiceFilterQuery serviceParentFilter = new HisServiceFilterQuery();
                serviceParentFilter.IDs = serviceIds;
                listServiceParent = new HisServiceManager(new CommonParam()).Get(serviceParentFilter);
                

                //danh sách sổ thu chi
                GetAccountBook();

                //danh sách loại giao dịch
                GetTransactionType();
                //danh sách hình thức thanh toán
                GetPayForm();
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        private void GetPayForm() {
            HisPayFormFilterQuery payFilter = new HisPayFormFilterQuery();
            payFilter.IDs = filter.PAY_FORM_IDs;
            listPayForm = new HisPayFormManager().Get(payFilter);

        }
        private void GetTransactionType()
        {

            HisTransactionTypeFilterQuery tranTypeFilter = new HisTransactionTypeFilterQuery();
            tranTypeFilter.IDs = filter.TRANSACTION_TYPE_IDs;
            listTransactionType = new HisTransactionTypeManager(new CommonParam()).Get(tranTypeFilter);
        }

        private void GetAccountBook()
        {
            this.listAccountBook = new HisAccountBookManager().Get(new HisAccountBookFilterQuery());
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                //bổ sung thông tin phụ
                foreach (var item in listRdo)
                {
                    var serviceParent = listServiceParent.Where(x => x.ID == item.PARENT_ID).FirstOrDefault();
                    if (serviceParent != null)
                    {
                        item.SERVICE_PARENT_CODE = serviceParent.SERVICE_CODE;
                        item.SERVICE_PARENT_NAME = serviceParent.SERVICE_NAME;
                    }
                    var accountBook = this.listAccountBook.FirstOrDefault(x => x.ID == item.ACCOUNT_BOOK_ID);
                    if (accountBook != null)
                    {
                        item.ACCOUNT_BOOK_CODE = accountBook.ACCOUNT_BOOK_CODE;
                        item.ACCOUNT_BOOK_NAME = accountBook.ACCOUNT_BOOK_NAME;
                    }
                    var payFrom = listPayForm.Where(x => x.ID == item.PAY_FORM_ID).FirstOrDefault();
                    if (payFrom!=null)
                    {
                        item.PAY_FORM_CODE = payFrom.PAY_FORM_CODE;
                        item.PAY_FORM_NAME = payFrom.PAY_FORM_NAME;
                    }
                }
                if (filter.CHECK_TYPE_HEIN_CARD.HasValue)
                {
                    if (filter.CHECK_TYPE_HEIN_CARD == 1)
                    {
                        listRdo = listRdo.Where(x => x.SERVICE_PARENT_CODE != null && x.SERVICE_PARENT_CODE == "CDV").ToList();
                    }
                    if (filter.CHECK_TYPE_HEIN_CARD == 0)
                    {
                        //listRdo = listRdo.Where(x => x.TDL_HEIN_CARD_NUMBER != null).ToList();
                        listRdo = listRdo.Where(x => x.SERVICE_PARENT_CODE != null && x.SERVICE_PARENT_CODE == "CCA").ToList();
                    }
                }

                //bổ sung thông tin chính
                foreach (var item in listRdo)
                {
                    if (item.SERVICE_PARENT_CODE != null)
                    {
                        if (item.SERVICE_PARENT_CODE=="CCA")
                        {
                            item.TOTAL_PRICE_AN_CA = item.AMOUNT * item.PRICE;
                        }
                        if (item.SERVICE_PARENT_CODE == "CDV")
                        {
                            item.TOTAL_PRICE_AN_DV = item.AMOUNT * item.PRICE;
                        }
                        if (item.SERVICE_PARENT_CODE =="QD792.AN") // tiền ăn daycare 199
                        {
                            item.TOTAL_PRICE_AN_DAYCARE = item.AMOUNT * item.PRICE;
                        }
                        if (item.SERVICE_PARENT_CODE =="VT005")// quân tư trang 199
                        {
                            item.TOTAL_PRICE_QTT = item.AMOUNT * item.PRICE;
                        }
                    }
                    else
                    {
                        item.TOTAL_PRICE_AN_DV = item.AMOUNT * item.PRICE;
                    }
                    item.TOTAL_PRICE = item.AMOUNT * item.PRICE;
                    item.INVOICE_NUMBER = item.NUM_ORDER;
                    item.DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.INTRUCTION_TIME??0);
                }

                var group = listRdo.GroupBy(x => string.Format("{0}_{1}_{2}_{3}", x.INVOICE_NUMBER, x.ACCOUNT_BOOK_CODE, x.DEPARTMENT_ID, x.PAY_FORM_NAME)).ToList();
                listRdo.Clear();
                foreach (var item in group)
                {
                    Mrs00812RDO rdo = new Mrs00812RDO();
                    rdo.INVOICE_NUMBER = item.First().INVOICE_NUMBER;
                    rdo.ACCOUNT_BOOK_CODE = item.First().ACCOUNT_BOOK_CODE;
                    rdo.PAY_FORM_NAME = item.First().PAY_FORM_NAME;
                    rdo.TOTAL_PRICE_AN_CA = item.Sum(x => x.TOTAL_PRICE_AN_CA);
                    rdo.TOTAL_PRICE_AN_DV = item.Sum(x => x.TOTAL_PRICE_AN_DV);
                    rdo.TOTAL_PRICE_QTT = item.Sum(x => x.TOTAL_PRICE_QTT);
                    rdo.TOTAL_PRICE_AN_DAYCARE = item.Sum(x => x.TOTAL_PRICE_AN_DAYCARE);
                    rdo.TOTAL_PRICE = item.Sum(x => x.TOTAL_PRICE);
                    rdo.DIC_PAR_TOTAL_PRICE = item.GroupBy(g=>g.SERVICE_PARENT_CODE??"NONE").ToDictionary(p=>p.Key,q=>q.Sum(x => x.TOTAL_PRICE));
                    rdo.PATIENT_ID = item.First().PATIENT_ID;
                    rdo.TREATMENT_CODE = item.First().TREATMENT_CODE;
                    rdo.TDL_PATIENT_CODE = item.First().TDL_PATIENT_CODE;
                    rdo.TDL_PATIENT_NAME = item.First().TDL_PATIENT_NAME;
                    rdo.DEPARTMENT_CODE = item.First().DEPARTMENT_CODE;
                    rdo.DEPARTMENT_NAME = item.First().DEPARTMENT_NAME;
                    rdo.DATE = item.First().DATE;
                    rdo.CASHIER_LOGINNAME = item.First().CASHIER_LOGINNAME;
                    rdo.CASHIER_USERNAME = item.First().CASHIER_USERNAME;
                    rdo.TRANSACTION_TYPE_NAME = item.First().TRANSACTION_TYPE_NAME;
                    listRdo.Add(rdo);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            if (filter.CASHIER_LOGINNAMEs!=null)
            {
                dicSingleTag.Add("CASHIER_USERNAMEs",string.Join(",",filter.CASHIER_LOGINNAMEs));
            }
            if (filter.TRANSACTION_TYPE_IDs != null)
            {
                dicSingleTag.Add("TRANSACTION_TYPE_NAMEs",string.Join(",", listTransactionType.Select(x => x.TRANSACTION_TYPE_NAME).ToList()));
            }
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", listRdo.OrderBy(x => x.INVOICE_NUMBER).ToList());
        }
    }
}
