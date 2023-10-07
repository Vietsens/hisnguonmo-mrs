using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00309
{
    public class Mrs00309RDO: V_HIS_SERE_SERV
    {
				 
				 public Decimal COUNT_TREATMENT { get;  set;  }	
				 public Decimal TOTAL_PRICE{get; set; }	
				 public Decimal MEDICINE_PRICE{get; set; }
                 public Decimal HEIN_TOTAL_PRICE { get;  set;  }
                 public Decimal HEIN_PATIENT_TOTAL_PRICE { get;  set;  }
                 public Decimal TOTAL_PRICE_FEE { get;  set;  }

    }
}
