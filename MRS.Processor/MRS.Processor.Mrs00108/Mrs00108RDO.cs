using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00108
{
    class Mrs00108RDO
    {
        public long FEE_LOCK_DATE { get;  set;  }
        public string FEE_LOCK_DATE_STR { get;  set;  }

        public decimal TOTAL_BILL_AMOUNT { get;  set;  }
        public decimal TOTAL_DEPOSIT_AMOUNT { get;  set;  }
        public decimal TOTAL_REPAY_AMOUNT { get;  set;  }

        public Mrs00108RDO() { }

        public Mrs00108RDO(List<MOS.EFMODEL.DataModels.V_HIS_TRANSACTION> hisTransactions, long? Fee_Lock_Time)
        {
            try
            {
                TOTAL_BILL_AMOUNT = hisTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(s => s.AMOUNT); 
                TOTAL_DEPOSIT_AMOUNT = hisTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Sum(s => s.AMOUNT); 
                TOTAL_REPAY_AMOUNT = hisTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Sum(s => s.AMOUNT); 

                if (Fee_Lock_Time.HasValue && Fee_Lock_Time.Value > 0)
                {
                    FEE_LOCK_DATE = long.Parse(Fee_Lock_Time.Value.ToString().Substring(0, 8)); 
                    FEE_LOCK_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Fee_Lock_Time.Value); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
