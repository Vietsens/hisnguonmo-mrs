using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS .Processor .Mrs00303
{
		 public class Mrs00303RDO
		 {
					public string MEDICINE_TYPE_NAME {get;  set;  }
					public string SERVICE_UNIT_NAME {get;  set;  }
					public decimal AMOUNT_MOBA_SUM {get;  set;  }
					public Decimal AMOUNT_EXP_SUM { get;  set;  }
					public Decimal PRICE {get;  set;  }
					public Decimal VIR_TOTAL_PRICE { get;  set;  }
					public string AMOUNT_STR {get;  set;  }


                    public string MEDICINE_GROUP_CODE { get; set; }

                    public string MEDICINE_GROUP_NAME { get; set; }

                    public string PARENT_MEDICINE_TYPE_CODE { get; set; }

                    public string PARENT_MEDICINE_TYPE_NAME { get; set; }

                    public string PATIENT_NAME { get; set; }

                    public string EXP_MEST_CODE { get; set; }

                    public string NATIONAL_NAME { get; set; }

                    public string MANUFACTURER_NAME { get; set; }

                    public decimal AMOUNT { get; set; }

                    public Dictionary<string, decimal> DIC_DATE_AMOUNT { get; set; }

                    public Dictionary<string, decimal> DIC_DATE_MOBA_AMOUNT { get; set; }

                    public decimal? VAT_RATIO { get; set; }
         }

         public class DATE_STR
         {
             public string DATE_STRING { get; set; }
         }
}