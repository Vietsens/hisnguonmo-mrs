using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00155
{
    public class VSarReportMrs00155RDO
    {
        public long DEPARTMENT_ID { get;  set;  }
        public int NUMBER_ORDER { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;   set;  }
        public string SERVICE_UNIT_NAME { get;   set;  }
        public string NATIONAL_NAME { get;   set;  }
        public decimal? PRICE { get;   set;  }
        public decimal? AMOUNT { get;  set; }
        public decimal? TOTAL_PRICE { get;  set; }
        public string SERVICE_TYPE { get;  set; }
    }
    public class ImpMestTypeRDO
    {
        public string IMP_MEST_TYPE_CODE { get; set; }
        public string IMP_MEST_TYPE_NAME { get; set; }
        public string MEDI_STOCK_CODE { get;  set; }
        public string MEDI_STOCK_NAME { get; set; }
        public string CHMS_MEDI_STOCK_CODE { get; set; }
        public string CHMS_MEDI_STOCK_NAME { get; set; }
        //public decimal? PRICE { get;  set; }
        public int COUNT_IMP_MEST { get; set; }
        public decimal AMOUNT { get;  set; }
        public decimal TOTAL_PRICE { get;  set; }
    }
}
