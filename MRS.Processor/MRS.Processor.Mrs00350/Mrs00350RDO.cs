using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00350
{
    public class Mrs00350RDO
    {
        public long FEE_LOCK_DATE { get;  set;  }
        public long FEE_LOCK_TIME { get;  set;  }
        public string FEE_LOCK_DATE_STR { get;  set;  }

        public decimal EXAM_AMOUNT_KB { get;  set;  }
        public decimal EXAM_AMOUNT_KSK { get;  set;  }
        public decimal TEST_AMOUNT_HH { get;  set;  }
        public decimal TEST_AMOUNT_HS { get;  set;  }
        public decimal TEST_AMOUNT_NT { get;  set;  }
        public decimal TEST_AMOUNT_HIV { get;  set;  }
        public decimal DIIM_AMOUNT_SA { get;  set;  }
        public decimal DIIM_AMOUNT_XQ { get;  set;  }
        public decimal DIIM_AMOUNT_REG { get;  set;  }
        public decimal DIIM_AMOUNT_ECG { get;  set;  }
        public decimal MISU_AMOUNT { get;  set;  }
        public decimal SURG_AMOUNT { get;  set;  }
        public decimal OTHER_AMOUNT { get;  set;  }

        public Mrs00350RDO() { }

        public Mrs00350RDO(V_HIS_TREATMENT treatment)
        {
            if (treatment != null)
            {
                this.FEE_LOCK_TIME = treatment.FEE_LOCK_TIME ?? 0; 
                this.FEE_LOCK_DATE = Convert.ToInt64(this.FEE_LOCK_TIME.ToString().Substring(0, 8)); 
                this.FEE_LOCK_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.FEE_LOCK_TIME); 
            }
        }
    }
}
