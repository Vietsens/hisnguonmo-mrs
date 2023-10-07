using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00069
{
    class Mrs00069RDO
    {
        public long MEDICINE_TYPE_ID { get;  set;  }
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }

        public long MATERIAL_TYPE_ID { get;  set;  }
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }

        public decimal OUT_AMOUNT { get;  set;  }
        public decimal IN_AMOUNT { get;  set;  }
        public decimal VIR_PRICE { get;  set;  }

        public Mrs00069RDO() { }

        public Mrs00069RDO(List<V_HIS_SERE_SERV> hisSereServs, bool isMedicine, bool isIn)
        {
            try
            {
                if (isMedicine)
                {
                    // là thuốc
                    MEDICINE_TYPE_ID = hisSereServs.First().SERVICE_ID; 
                    MEDICINE_TYPE_CODE = hisSereServs.First().TDL_SERVICE_CODE; 
                    MEDICINE_TYPE_NAME = hisSereServs.First().TDL_SERVICE_NAME; 

                }
                else
                {
                    //là vật tư
                    MATERIAL_TYPE_ID = hisSereServs.First().SERVICE_ID; 
                    MATERIAL_TYPE_CODE = hisSereServs.First().TDL_SERVICE_CODE; 
                    MATERIAL_TYPE_NAME = hisSereServs.First().TDL_SERVICE_NAME; 
                }

                VIR_PRICE = hisSereServs.First().VIR_PRICE ?? 0; 
                SERVICE_UNIT_NAME = hisSereServs.First().SERVICE_UNIT_NAME; 

                if (isIn)
                {
                    //Là Nội trú
                    IN_AMOUNT = hisSereServs.Sum(s => s.AMOUNT); 
                }
                else
                {
                    //là ngoại trú
                    OUT_AMOUNT = hisSereServs.Sum(s => s.AMOUNT); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
