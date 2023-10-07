using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00330
{
    public class D_HIS_TREATMENT : HIS_TREATMENT
    {
        public string ROOM_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
    }
    class Mrs00330RDO : HIS_TRANSACTION
    {
        public string TRANSACTION_TYPE_ORDER { get; set; }
        public string CASHIER_ROOM_CODE { get; set; }
        public string CASHIER_ROOM_NAME { get; set; }
        public string PAY_FORM_NAME { get; set; }

        public string ACCOUNT_BOOK_CODE { set; get; }
        public string ACCOUNT_BOOK_NAME { set; get; }
        public string CREATE_TIME_STR { set; get; }
        public string HEIN_CARD_NUMBER { set; get; }
        public string TREATMENT_CODE { set; get; }
        public string VIR_PATIENT_NAME { set; get; }
        public string VIR_ADDRESS { set; get; }
        public string TEMPLATE_CODE { set; get; }
        public string NUM_ORDER_STR { get; set; }

        public decimal AMOUNT_DEPOSIT { get; set; }
        public decimal AMOUNT_REPAY { get; set; }

        public decimal TOTAL_BILL_PRICE { get; set; }

        public decimal PRICE_EXAM { get; set; }//khám
        public decimal PRICE_TEST { get; set; }//xét nghiệm
        public decimal PRICE_DIIM { get; set; }//CDHA
        public decimal PRICE_MISU { get; set; }//thủ thuật
        public decimal PRICE_SURG { get; set; }//phẫu thuật
        public decimal PRICE_FUEX { get; set; }//thăm dò chức năng
        public decimal PRICE_ENDO { get; set; }//nội soi
        public decimal PRICE_SUIM { get; set; }//siêu âm
        public decimal PRICE_BED { get; set; }//giường
        public decimal PRICE_PRES { get; set; }//thuốc
        public decimal PRICE_BLOOD { get; set; }//máu
        public decimal PRICE_OTHER { get; set; }//khác

        public decimal PRICE_BNCCT { get; set; }
        public decimal PRICE_BNTT { get; set; }

        public string END_ROOM_NAME { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string TRANSACTION_TYPE_CODE { get; set; }
        public string TRANSACTION_TYPE_NAME { get; set; }

        public Dictionary<string, decimal> DIC_SVT_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PAR_PRICE { get; set; }

        public string NOTE_PAR_PRICE { get; set; }

        //public Dictionary<string, decimal> DIC_CATE_PRICE { get; set; }

        //public string NOTE_CATE_PRICE { get; set; }

        public Mrs00330RDO() { }

        public Mrs00330RDO(HIS_TRANSACTION data, List<HIS_CASHIER_ROOM> ListCashierRoom, List<HIS_PAY_FORM> ListPayForm, List<HIS_ACCOUNT_BOOK> ListAccountBook)
        {
            DIC_SVT_PRICE = new Dictionary<string, decimal>();
            DIC_HSVT_PRICE = new Dictionary<string, decimal>();
            DIC_PAR_PRICE = new Dictionary<string, decimal>();
            NOTE_PAR_PRICE = "";
            //DIC_CATE_PRICE = new Dictionary<string, decimal>();
            //NOTE_CATE_PRICE = "";
            if (data != null)
            {
                this.TRANSACTION_TYPE_ORDER = data.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT?"1":(data.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU?"2":"3");
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00330RDO>(this, data);
                if (ListCashierRoom != null)
                {
                    var cashierRoom = ListCashierRoom.FirstOrDefault(o => o.ID == data.CASHIER_ROOM_ID);
                    if (cashierRoom != null)
                    {
                        this.CASHIER_ROOM_CODE = cashierRoom.CASHIER_ROOM_CODE;
                        this.CASHIER_ROOM_NAME = cashierRoom.CASHIER_ROOM_NAME;
                    }
                }
                if (ListPayForm != null)
                {
                    var payForm = ListPayForm.FirstOrDefault(o => o.ID == data.PAY_FORM_ID);
                    if (payForm != null)
                    {
                        this.PAY_FORM_NAME = payForm.PAY_FORM_NAME;
                    }
                }
                if (ListAccountBook != null)
                {
                    var accountBook = ListAccountBook.FirstOrDefault(o => o.ID == data.ACCOUNT_BOOK_ID);
                    if (accountBook != null)
                    {
                        this.ACCOUNT_BOOK_CODE = accountBook.ACCOUNT_BOOK_CODE;
                        this.ACCOUNT_BOOK_NAME = accountBook.ACCOUNT_BOOK_NAME;
                        this.TEMPLATE_CODE = accountBook.TEMPLATE_CODE;
                    }
                }
                //this.CASHIER_LOGINNAME = data.CASHIER_LOGINNAME;
                //this.DESCRIPTION = data.DESCRIPTION;
                this.NUM_ORDER_STR = string.Format("{0:0000000}", Convert.ToInt64(NUM_ORDER));

                //this.TRANSACTION_CODE = data.TRANSACTION_CODE;
                this.TREATMENT_CODE = data.TDL_TREATMENT_CODE;
                this.VIR_PATIENT_NAME = data.TDL_PATIENT_NAME;
                this.VIR_ADDRESS = data.TDL_PATIENT_ADDRESS;
                //this.CASHIER_USERNAME = data.CASHIER_USERNAME;
                this.EXEMPTION = data.EXEMPTION ?? 0;
                if (data.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                    this.AMOUNT_DEPOSIT = data.AMOUNT;
                if (data.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                    this.AMOUNT_REPAY = data.AMOUNT;
                
            }
        }


        public decimal PRICE_GPBL { get; set; }

        public decimal PRICE_BHTT { get; set; }
    }
}
