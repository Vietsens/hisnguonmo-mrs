using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00807
{
    public class Mrs00807RDO
    {
        public string DOCTOR_LOGINNAME { set; get; }
        public string DOCTOR_USERNAME { set; get; }
        public long SERVICE_TYPE_ID { set; get; }
        public decimal AMOUNT { set; get; }
        public decimal PRICE { set; get; }
        public decimal TOTAL_PRICE { set; get; }
        public long SERVICE_ID { set; get; }
        public long TREATMENT_ID { set; get; }
        
    }
    public class Mrs00807RDOGroup: Mrs00807RDO
    {
        public decimal AMOUNT_KH { set; get; }
        public decimal AMOUNT_XN { set; get; }
        public decimal AMOUNT_CDHA { set; get; }
        public decimal TOTAL_PRICE_CDHA { set; get; }
        public decimal TOTAL_PRICE_XN { set; get; }
        public decimal TOTAL_AMOUNT { set; get; }
        public decimal? TOTAL_PRICE_XN_TB { set; get; }
        public decimal? TOTAL_PRICE_CDHA_TB { set; get; }
        public decimal TOTAL_SERVICE { set; get; }

        public decimal TOTAL_PRICE_ALL { set; get; }
    }
}
