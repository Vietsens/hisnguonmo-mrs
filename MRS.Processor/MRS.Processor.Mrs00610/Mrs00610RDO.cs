using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00610
{
    public class Mrs00610RDO
    {
        public decimal IN_AMOUNT { get; set; }
        public decimal OUT_AMOUNT { get; set; }
        public decimal ALL_AMOUNT { get; set; }
        public decimal IN_TOTAL_PRICE { get; set; }
        public decimal OUT_TOTAL_PRICE { get; set; }
        public decimal ALL_TOTAL_PRICE { get; set; }

        public string CATEGORY_CODE { get; set; }

        public string CATEGORY_NAME { get; set; }

        public string SERVICE_TYPE_CODE { get; set; }

        public string SERVICE_TYPE_NAME { get; set; }

        public string TRANSACTION_CODE { get; set; }

        public long? TIME { get; set; }

        //public decimal ALL_AMOUNT_1 { get; set; }
        //public decimal ALL_AMOUNT_2 { get; set; }
        //public decimal ALL_AMOUNT_3 { get; set; }
        //public decimal ALL_AMOUNT_4 { get; set; }
        //public decimal ALL_AMOUNT_5 { get; set; }
        //public decimal ALL_AMOUNT_6 { get; set; }
        //public decimal ALL_AMOUNT_7 { get; set; }
        //public decimal ALL_AMOUNT_8 { get; set; }
        //public decimal ALL_AMOUNT_9 { get; set; }
        //public decimal ALL_AMOUNT_10 { get; set; }
        //public decimal ALL_AMOUNT_11 { get; set; }
        //public decimal ALL_AMOUNT_12 { get; set; }

        //public decimal ALL_TOTAL_PRICE_1 { get; set; }
        //public decimal ALL_TOTAL_PRICE_2 { get; set; }
        //public decimal ALL_TOTAL_PRICE_3 { get; set; }
        //public decimal ALL_TOTAL_PRICE_4 { get; set; }
        //public decimal ALL_TOTAL_PRICE_5 { get; set; }
        //public decimal ALL_TOTAL_PRICE_6 { get; set; }
        //public decimal ALL_TOTAL_PRICE_7 { get; set; }
        //public decimal ALL_TOTAL_PRICE_8 { get; set; }
        //public decimal ALL_TOTAL_PRICE_9 { get; set; }
        //public decimal ALL_TOTAL_PRICE_10 { get; set; }
        //public decimal ALL_TOTAL_PRICE_11 { get; set; }
        //public decimal ALL_TOTAL_PRICE_12 { get; set; }


        public long TDL_TREATMENT_ID { get; set; }

        public long SERVICE_ID { get; set; }

        public decimal AMOUNT { get; set; }

        public string TDL_SERVICE_CODE { get; set; }

        public string TDL_SERVICE_NAME { get; set; }

        public decimal VIR_TOTAL_PRICE { get; set; }
    }
}
