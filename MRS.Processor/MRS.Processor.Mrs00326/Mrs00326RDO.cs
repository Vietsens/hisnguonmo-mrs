using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00326
{
    class Mrs00326RDO : IMP_EXP_MEST_TYPE
    {
        public long MEDI_STOCK_ID { get; set; }
        public string MEDI_STOCK_NAME { get; set; }

        public int SERVICE_TYPE_ID { get; set; }

        public long MEDI_MATE_ID { get; set; }
        public long SERVICE_ID { get; set; }

        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }

        public decimal BEGIN_AMOUNT { get; set; }
        public decimal END_AMOUNT { get; set; }
        public decimal IMP_PRICE { get; set; }
        public decimal IMP_VAT { get; set; }

        public decimal IMP_CHMS_IS_BUSINESS_AMOUNT { get; set; }

        public decimal EXP_CHMS_IS_BUSINESS_AMOUNT { get; set; }

        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string SUPPLIER_PHONE { get; set; }
        public string SUPPLIER_TAX_CODE { get; set; }
        public string SUPPLIER_FAX { get; set; }
        public decimal? SUPPLIER_BANK_ACCOUNT { get; set; }
        public string SUPPLIER_BANK_INFO { get; set; }
        public string SUPPLIER_POSITION { get; set; }

        public long? NUM_ORDER { get; set; }
    }
}
