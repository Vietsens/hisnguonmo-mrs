using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00344
{
    public class Mrs00344RDO : V_HIS_TREATMENT
    {
        public long TOTAL_PATIENT_NUMBER { get;  set;  }
        public long TOTAL_TREATMENT_DATE { get;  set;  }
        public decimal? VIR_TOTAL_HEIN_PRICE { get;  set; }
        public decimal? VIR_TOTAL_PATIENT_PRICE { get; set; }
        public decimal? VIR_TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public decimal? VIR_TOTAL_PRICE_TEST { get;  set;  }
        public decimal? VIR_TOTAL_PRICE_DIIM_FUEX { get;  set;  }
        public decimal? VIR_TOTAL_PRICE_EXAM { get;  set;  }
        public decimal? VIR_TOTAL_PRICE_SURG_MISU { get;  set;  }
        public decimal? VIR_TOTAL_PRICE_BLOOD { get;  set;  }
        public decimal? VIR_TOTAL_PRICE_MEDICINE { get;  set;  }
        public decimal? VIR_TOTAL_PRICE_MATERIAL { get;  set;  }
        public decimal? VIR_TOTAL_PRICE_TRAN { get;  set;  }
        public decimal? VIR_TOTAL_PRICE_OTHER { get;  set;  }
        public decimal? VIR_TOTAL_PRICE_BED { get;  set;  }
        public decimal? VIR_TOTAL_PRICE_HIGHTECH { get;  set;  }
        public decimal? VIR_TOTAL_PRICE { get; set; }
        public string GROUP_DEPARTMENT_NAME { get; set; }
        public Dictionary<string, decimal> DIC_TOTAL_HEIN_PRICE { get; set; }
        public Dictionary<string,decimal> DIC_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public Mrs00344RDO()
        {
        }

        public Mrs00344RDO(V_HIS_TREATMENT treatment)
        {
            if (treatment != null)
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00344RDO>(this, treatment); 
            }
        }
    }
}
