using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00206
{
    class Mrs00206RDO
    {
        public long MEDI_STOCK_ID { get; set; }
        public string MEDI_STOCK_NAME { get; set; }

        public long REPORT_TYPE_CAT_ID { get; set; }

        public int SERVICE_TYPE_ID { get; set; }

        public long MEDI_MATE_ID { get; set; }
        public long MEDI_MATE_IM_ID { get; set; }

        public long SERVICE_ID { get; set; }

        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }

        public decimal BEGIN_AMOUNT { get; set; }
        public decimal END_AMOUNT { get; set; }
        public decimal IMP_PRICE { get; set; }

        public decimal IMP_MANU_AMOUNT { get; set; }
        public decimal IMP_CHMS_AMOUNT { get; set; }
        public decimal IMP_MOBA_AMOUNT { get; set; }

        public decimal EXP_PRES_AMOUNT { get; set; }
        public decimal EXP_DEPA_AMOUNT { get; set; }
        public decimal EXP_CHMS_AMOUNT { get; set; }
        public decimal EXP_MANU_AMOUNT { get; set; }
        public decimal EXP_EXPE_AMOUNT { get; set; }
        public decimal EXP_LOST_AMOUNT { get; set; }
        public decimal EXP_SALE_AMOUNT { get; set; }
        public decimal EXP_LIQU_AMOUNT { get; set; }
        public decimal EXP_OTHER_AMOUNT { get; set; }

        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }


        public long? NUM_ORDER { get; set; }

    }
}
