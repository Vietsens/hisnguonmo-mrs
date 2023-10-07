using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00187
{
    public class Mrs00187RDO
    {
        public long STT { get;  set;  } //STT
        public string MEDICINE_TYPE_NAME { get;  set;  } //Ten thuoc
        public string SERVICE_UNIT_NAME { get;  set;  } //don vi
        public decimal? PRICE { get;  set;  }  // don gia
        public decimal? AMOUNT { get;  set;  } // so luong
        public decimal? AMOUNT_X { get;  set;  } // so luong        
        public decimal? AMOUNT_N { get;  set;  } // so luong
        public decimal? TT { get;  set;  }  // tong tien

        public string PATIENT_CODE { get; set; }

        public string PATIENT_NAME { get; set; }

        public string EXP_MEST_TYPE_NAME { get; set; }

        public string EXP_MEST_CODE { get; set; }

        public long? NUM_ORDER { get; set; }

        public string EXP_TIME_STR { get; set; }

        public string MEDI_STOCK_CODE { get; set; }

        public string MEDI_STOCK_NAME { get; set; }

        public decimal? TOTAL_PRICE { get; set; }

        public decimal? EXP_PRICE { get; set; }

        public string TRANSACTION_CODE { get; set; }

        public string REGISTER_NUMBER { get; set; }

        public string TREATMENT_CODE { get; set; }

        public string TDL_AGGR_EXP_MEST_CODE { get; set; }
    }
}
