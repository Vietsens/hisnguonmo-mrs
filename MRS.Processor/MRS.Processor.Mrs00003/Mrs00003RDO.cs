using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00003
{
    class Mrs00003RDO
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
        public string BID_NUMBER { get;  set;  }
        public string SUPPLIER_CODE { get;  set;  }
        public string SUPPLIER_NAME { get;  set;  }
        public string MANUFACTURER_CODE { get;  set;  }
        public string MANUFACTURER_NAME { get;  set;  }
        public string NATIONAL_NAME { get;  set;  }
        public long? NUM_ORDER { get;  set;  }
        public decimal? PRICE { get;  set;  }
        public string REGISTER_NUMBER { get;  set;  }
        public string SERVICE_UNIT_CODE { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal? VAT_RATIO { get;  set;  }

        public Mrs00003RDO(V_HIS_IMP_MEST_MEDICINE imp)
        {
            try
            {
                EXECUTE_TIME = imp.IMP_TIME; 
                PACKAGE_NUMBER = imp.PACKAGE_NUMBER; 
                IMP_MEST_CODE = imp.IMP_MEST_CODE; 
                EXPIRED_DATE = imp.EXPIRED_DATE; 
                IMP_AMOUNT = imp.AMOUNT; 
                BID_NUMBER = imp.BID_NUMBER; 
                SUPPLIER_CODE = imp.SUPPLIER_CODE; 
                SUPPLIER_NAME = imp.SUPPLIER_NAME; 

                NATIONAL_NAME = imp.NATIONAL_NAME; 
                NUM_ORDER = imp.NUM_ORDER; 
                PRICE = imp.PRICE; 
                REGISTER_NUMBER = imp.REGISTER_NUMBER; 
                SERVICE_UNIT_CODE = imp.SERVICE_UNIT_CODE; 
                SERVICE_UNIT_NAME = imp.SERVICE_UNIT_NAME; 
                VAT_RATIO = imp.VAT_RATIO; 

                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public Mrs00003RDO(V_HIS_EXP_MEST_MEDICINE exp)
        {
            try
            {
                EXECUTE_TIME = exp.EXP_TIME; 
                PACKAGE_NUMBER = exp.PACKAGE_NUMBER; 
                EXP_MEST_CODE = exp.EXP_MEST_CODE; 
                EXPIRED_DATE = exp.EXPIRED_DATE; 
                EXP_AMOUNT = exp.AMOUNT; 

                BID_NUMBER = exp.BID_NUMBER; 
                SUPPLIER_CODE = exp.SUPPLIER_CODE; 
                SUPPLIER_NAME = exp.SUPPLIER_NAME; 

                NATIONAL_NAME = exp.NATIONAL_NAME; 
                NUM_ORDER = exp.NUM_ORDER; 
                PRICE = exp.PRICE; 
                REGISTER_NUMBER = exp.REGISTER_NUMBER; 
                SERVICE_UNIT_CODE = exp.SERVICE_UNIT_CODE; 
                SERVICE_UNIT_NAME = exp.SERVICE_UNIT_NAME; 
                VAT_RATIO = exp.VAT_RATIO; 

                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void SetExtendField(Mrs00003RDO data)
        {
            EXECUTE_DATE_STR = data.EXECUTE_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.EXECUTE_TIME.Value) : ""; 
            EXPIRED_DATE_STR = data.EXPIRED_DATE.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.EXPIRED_DATE.Value) : ""; 
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
