using MOS.MANAGER.HisImpMest;
using Inventec.Common.Logging; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Core; 
using MOS.MANAGER.HisImpMestType; 
using MRS.MANAGER.Core.MrsReport.RDO; 
namespace MRS.Processor.Mrs00076
{
    class Mrs00076RDO
    {
        public long? EXECUTE_TIME { get;  set;  }
        public string EXECUTE_DATE_STR { get;  set;  }
        public string IMP_MEST_CODE { get;  set;  }
        public string EXP_MEST_CODE { get;  set;  }
        public string BID_NUMBER { get;  set;  }
        public long? EXPIRED_DATE { get;  set;  }
        public string EXPIRED_DATE_STR { get;  set;  }
        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal? IMP_AMOUNT { get;  set;  }
        public decimal? EXP_AMOUNT { get;  set;  }
        public decimal END_AMOUNT { get;  set;  }
        public Mrs00076RDO() { }
        public Mrs00076RDO(V_HIS_IMP_MEST_MATERIAL imp)
        {
            try
            {
                EXECUTE_TIME = imp.IMP_TIME; 
                BID_NUMBER = imp.BID_NUMBER; 
                IMP_MEST_CODE = imp.IMP_MEST_CODE; 
                EXPIRED_DATE = imp.EXPIRED_DATE; 
                IMP_AMOUNT = imp.AMOUNT; 

                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public Mrs00076RDO(V_HIS_EXP_MEST_MATERIAL exp)
        {
            try
            {
                EXECUTE_TIME = exp.EXP_TIME; 
                BID_NUMBER = exp.BID_NUMBER; 
                EXP_MEST_CODE = exp.EXP_MEST_CODE; 
                EXPIRED_DATE = exp.EXPIRED_DATE; 
                EXP_AMOUNT = exp.AMOUNT; 

                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void SetExtendField(Mrs00076RDO data)
        {
            EXECUTE_DATE_STR = RDOCommon.ConvertTimeToDateString(data.EXECUTE_TIME); 
            EXPIRED_DATE_STR = RDOCommon.ConvertTimeToDateString(data.EXPIRED_DATE); 
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
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
