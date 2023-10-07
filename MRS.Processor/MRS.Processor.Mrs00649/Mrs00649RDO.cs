using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00649
{
    class Mrs00649RDO
    {
        public long GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }

        public long GROUP_6_ID { get; set; }
        public string GROUP_6_NAME { get; set; }

        public long SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }

        public decimal IMP_PRICE { get; set; }
        public long? IMP_TIME { get; set; }
        public decimal IMP_VAT_RATIO { get; set; }
        public decimal IMP_VAT_100 { get; set; }
        
        public string REGISTER_NUMBER { get; set; }

        public string SERVICE_UNIT_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string MANUFACTURER_NAME { get; set; }       // hãng sản xuất
        public string SUPPLIER_NAME { get; set; }       // NCC

        public long? DOCUMENT_DATE { get; set; }
        public string DOCUMENT_NUMBER { get; set; }         // số chứng từ
        public string BID_NUMBER { get; set; }              // số quyết định(số thầu)

        public decimal AMOUNT { get; set; }
        public decimal PRICE { get; set; }

        public string PACKAGE_NUMBER { get; set; }          // số lô
        public long EXPIRED_DATE { get; set; }              // hạn sử dụng
    }
}
