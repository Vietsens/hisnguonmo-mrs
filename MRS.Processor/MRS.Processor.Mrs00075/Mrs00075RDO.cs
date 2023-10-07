using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00075
{
    class Mrs00075RDO
    {
        public long? EXECUTE_TIME { get;  set;  }
        public string EXECUTE_DATE_STR { get;  set;  }
        public string IMP_MEST_CODE { get;  set;  }
        public string EXP_MEST_CODE { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  }
        public long? EXPIRED_DATE { get;  set;  }
        public string EXPIRED_DATE_STR { get;  set;  }
        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal? IMP_AMOUNT { get;  set;  }
        public decimal? EXP_AMOUNT { get;  set;  }
        public decimal END_AMOUNT { get;  set;  }

        public Mrs00075RDO() {}

        public Mrs00075RDO(V_HIS_IMP_MEST_MEDICINE  imp)
        {
            try
            {
                EXECUTE_TIME = imp.IMP_TIME; 
                PACKAGE_NUMBER = imp.PACKAGE_NUMBER; 
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

        public Mrs00075RDO(V_HIS_EXP_MEST_MEDICINE exp)
        {
            try
            {
                EXECUTE_TIME = exp.EXP_TIME; 
                PACKAGE_NUMBER = exp.PACKAGE_NUMBER; 
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

        private void SetExtendField(Mrs00075RDO data)
        {
            EXECUTE_DATE_STR = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.ConvertTimeToDateString(data.EXECUTE_TIME); 
            EXPIRED_DATE_STR = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.ConvertTimeToDateString(data.EXPIRED_DATE); 
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
