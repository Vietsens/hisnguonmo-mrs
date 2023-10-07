using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00361
{
    public class Mrs00361RDO
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

        public decimal EXAM_PRICE_KB { get;  set;  }
        public decimal EXAM_PRICE_KSK { get;  set;  }
        public decimal TEST_PRICE_HH { get;  set;  }
        public decimal TEST_PRICE_HS { get;  set;  }
        public decimal TEST_PRICE_NT { get;  set;  }
        public decimal TEST_PRICE_HIV { get;  set;  }
        public decimal DIIM_PRICE_SA { get;  set;  }
        public decimal DIIM_PRICE_XQ { get;  set;  }
        public decimal DIIM_PRICE_REG { get;  set;  }
        public decimal DIIM_PRICE_ECG { get;  set;  }
        public decimal MISU_PRICE { get;  set;  }
        public decimal SURG_PRICE { get;  set;  }
        public decimal OTHER_PRICE { get;  set;  }


        public decimal DEPOSIT_AMOUNT { get;  set;  }

        public Mrs00361RDO() { }

        public Mrs00361RDO(V_HIS_TRANSACTION bill)
        {
            if (bill != null)
            {
                this.FEE_LOCK_TIME = bill.TRANSACTION_TIME; 
                this.FEE_LOCK_DATE = Convert.ToInt64(this.FEE_LOCK_TIME.ToString().Substring(0, 8)); 
                this.FEE_LOCK_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.FEE_LOCK_TIME); 
                if (bill.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU) this.DEPOSIT_AMOUNT = bill.AMOUNT; 
            }

        }
    }
}
