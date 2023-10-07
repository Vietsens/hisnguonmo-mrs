using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00647
{
    class Mrs00647RDO : IMP_EXP_MEST_TYPE
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

        public string CONCENTRA { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }

        public long? NUM_ORDER { get; set; }

        public decimal EXP_PRICE { get; set; }
        public string EXPIRED_DATE_STR { get; set; }//Hạn dùng
        public string ACTIVE_INGR_BHYT_CODE { get; set; }//Mã Hoạt chất
        public string ACTIVE_INGR_BHYT_NAME { get; set; }//Tên hoạt chất
        public string REGISTER_NUMBER { get; set; }//Số đăng ký
        public string PACKAGE_NUMBER { get; set; }//Số lô

        public long? IMP_TIME { get; set; }
    }
}
