using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00256
{
    public class Mrs00256RDO
    {
        public string NAME {get; set; }	
        public string  UNIT {get; set; }		
        public Decimal  BEGIN_AMOUNT {get; set; }
        public Decimal BID_AMOUNT { get;  set;  }
        public Decimal TOTAL_IMP { get;  set;  }
        public Decimal PRICE { get;  set;  }
        public Decimal TOTAL_PRICE_IMP { get;  set;  }
        public Decimal USERED_AMOUNT { get;  set;  }
        public Decimal BID_AMOUNT_TO { get;  set;  }
        public Decimal AMOUNT_TO { get;  set;  }
        public Decimal TOTAL_AMOUNT_TO { get;  set;  }			
        public string  ANTICIPATE_NAME {get; set; }						
        public string  DESCRIPTION {get; set; }			
        public string  MANUFACTURER {get; set; }			
        public string  NATIONAL_NAME {get; set; }


        public string ACTIVE_INGR_BHYT_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public string CONCENTTRA { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public Decimal? IM_PRICE { get; set; }



        public string BID_GROUP_CODE { get; set; }
        public string BID_PACKAGE_CODE { get; set; }

        public string KEY_GROUP_BID_DETAIL { get; set; }

        public string KEY_GROUP_BID { get; set; }







        public string BID_NUMBER { get; set; }

        public string BID_NAME { get; set; }

        public string CODE { get; set; }

        public string BID_YEAR { get; set; }

        public string SUPPLIER_CODE { get; set; }

        public string PRIO_BID_SUPPLIER_NAME { get; set; }

        public string PRIO_BID_SUPPLIER_CODE { get; set; }

        public decimal COUNT_METY { get; set; }
    }
}
