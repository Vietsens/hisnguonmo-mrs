using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00183
{
    public class Mrs00183RDO 
    {
         public long TREATMENT_ID { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }

        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string DATE_IN_STR { get;  set;  }
        public string DATE_OUT_STR { get;  set;  }

        public long IN_TIME { get;  set;  }
        public long? OUT_TIME { get;  set;  }

        public decimal TOTAL_HEIN_PRICE { get;  set;  }
        public decimal TOTAL_HEIN_LIMIT_PRICE { get;  set;  }
        public decimal TOTAL_HEIN_PATIENT_PRICE { get;  set;  }
        public decimal TOTAL_FEE_PATIENT_PRICE { get;  set;  }
        public decimal TOTAL_DIFFERENCE_PRICE { get;  set;  }
        public decimal VIR_TOTAL_PATIENT_PRICE { get;  set;  }// = TOTAL_HEIN_PATIENT_PRICE+TOTAL_FEE_PATIENT_PRICE+TOTAL_DIFFERENCE_PRICE
        public decimal TOTAL_BILL_EXAM_AMOUNT { get;  set;  }
        public decimal TOTAL_DEPOSIT_AMOUNT { get;  set;  }
        public decimal TOTAL_DEPOSIT_SERE_AMOUNT { get;  set;  }
        public decimal TOTAL_PATIENT_AMOUNT { get;  set;  }
        public decimal TOTAL_WITHDRAW_AMOUNT { get;  set;  }
        public decimal TOTAL_BILL_FUND { get;  set;  }

        public long? LOG_TIME_EXAM { get;  set;  }

        public Mrs00183RDO() { }

        public Mrs00183RDO(V_HIS_TREATMENT treatment)
        {
            try
            {
                if (treatment != null)
                {
                    TREATMENT_ID = treatment.ID; 
                    TREATMENT_CODE = treatment.TREATMENT_CODE; 
                    PATIENT_CODE = treatment.TDL_PATIENT_CODE; 
                    VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME; 
                    IN_TIME = treatment.IN_TIME; 
                    OUT_TIME = treatment.OUT_TIME; 
                    DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME); 
                    DATE_OUT_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

    }
}
