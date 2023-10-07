using Inventec.Common.Logging; 
using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00004
{
    class Mrs00004RDO
    {
        public long? EXECUTE_TIME { get;  set;  }
        public string EXECUTE_DATE_STR { get;  set;  }
        public string IMP_MEST_CODE { get;  set;  }
        public string EXP_MEST_CODE { get;  set;  }
        public string BID_NUMBER { get;  set;  }
        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal? IMP_AMOUNT { get;  set;  }
        public decimal? EXP_AMOUNT { get;  set;  }
        public decimal END_AMOUNT { get;  set;  }

        public Mrs00004RDO(V_HIS_IMP_MEST_MATERIAL imp)
        {
            try
            {
                EXECUTE_TIME = imp.IMP_TIME; 
                BID_NUMBER = imp.BID_NUMBER; 
                IMP_MEST_CODE = imp.IMP_MEST_CODE; 
                IMP_AMOUNT = imp.AMOUNT; 

                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }

        public Mrs00004RDO(V_HIS_EXP_MEST_MATERIAL exp)
        {
            try
            {
                EXECUTE_TIME = exp.EXP_TIME; 
                BID_NUMBER = exp.BID_NUMBER; 
                EXP_MEST_CODE = exp.EXP_MEST_CODE; 
                EXP_AMOUNT = exp.AMOUNT; 

                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }

        private void SetExtendField(Mrs00004RDO data)
        {
            EXECUTE_DATE_STR = data.EXECUTE_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.EXECUTE_TIME.Value) : ""; 
        }

        internal void CalculateAmount(decimal previousEndAmount)
        {
            try
            {
                BEGIN_AMOUNT = previousEndAmount; 
                END_AMOUNT = BEGIN_AMOUNT + (IMP_AMOUNT.HasValue ? IMP_AMOUNT.Value : 0) - (EXP_AMOUNT.HasValue ? EXP_AMOUNT.Value : 0); 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }
    }
}
