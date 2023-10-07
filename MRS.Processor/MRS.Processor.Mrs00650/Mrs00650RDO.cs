using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00650
{
    class Mrs00650RDO
    {
        public string IMP_MEST_CODE { get; set; }

        public long? IMP_TIME { get; set; }
        public decimal IMP_PRICE { get; set; }
        public decimal IMP_VAT_RATIO { get; set; }

        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }

        public long MEDI_MATE_ID { get; set; }
        public string MEDI_MATE_CODE { get; set; }//MÃ
        public string MEDI_MATE_NAME { get; set; }//Tên

        public string EXPIRED_DATE_STR { get; set; }//Hạn dùng
        public string ACTIVE_INGR_BHYT_CODE { get; set; }//Mã Hoạt chất
        public string ACTIVE_INGR_BHYT_NAME { get; set; }//Tên hoạt chất
        public string REGISTER_NUMBER { get; set; }//Số đăng ký
        public string PACKAGE_NUMBER { get; set; }//Số lô

        public long? DOCUMENT_DATE { get; set; }
        public string DOCUMENT_NUMBER { get; set; }//Số chứng từ
        public decimal? DOCUMENT_PRICE { get; set; }

        public decimal? IMP_AMOUNT { get; set; }
        public decimal? EXP_AMOUNT { get; set; }
    }
}
