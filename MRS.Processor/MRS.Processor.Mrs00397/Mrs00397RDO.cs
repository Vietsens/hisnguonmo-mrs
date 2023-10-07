using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00397
{
    public class Mrs00397RDO
    {
        
        public long MATERIAL_ID { get;  set;  }
       
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }

        public long MEDICINE_ID { get;  set;  }
        
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  } // SỐ LÔ THUỐC

        public string SERVICE_UNIT_NAME { get;  set;  } // ĐƠN VỊ TÍNH
       // public string CONCENTRA { get;  set;  }

        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal IMP_AMOUNT { get;  set;  }
        public decimal EXP_AMOUNT { get;  set;  }
        public decimal END_AMOUNT { get;  set;  }
        public decimal IMP_PRICE { get;  set;  }
        public decimal EXP_PRICE { get;  set;  }
        public decimal TOTAL_EXP_PRICE { get;  set;  }

      

       
        internal void SetMediConcentraAndEndAmount(List<V_HIS_MEDICINE_TYPE> ListMedicineType, ref CommonParam paramGet)
        {
            try
            {
                END_AMOUNT = BEGIN_AMOUNT + IMP_AMOUNT - EXP_AMOUNT; 
               
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        internal void SetMateConcentraAndEndAmount(List<V_HIS_MATERIAL_TYPE> ListMaterialType, ref CommonParam paramGet)
        {
            try
            {
                END_AMOUNT = BEGIN_AMOUNT + IMP_AMOUNT - EXP_AMOUNT; 
              
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
              
    }

 }

