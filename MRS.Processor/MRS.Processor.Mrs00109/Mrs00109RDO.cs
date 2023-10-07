using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00109
{
    class Mrs00109RDO
    {
        public long PATIENT_TYPE_ID { get;  set;  }
        public string PATIENT_TYPE_NAME { get;  set;  }

        public long SERVICE_TYPE_ID { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        public long SERVICE_ID { get;  set;  }
        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }

        public decimal COST_PRICE { get;  set;  }
        public decimal FEE_PRICE { get;  set;  }
        public decimal AMOUNT { get;  set;  }

        public decimal TOTAL_COST_PRICE { get;  set;  }
        public decimal TOTAL_FEE_PRICE { get;  set;  }

        public Mrs00109RDO() { }

        public Mrs00109RDO(List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> ListData, MOS.EFMODEL.DataModels.V_HIS_SERVICE service)
        {
            try
            {
                PATIENT_TYPE_ID = ListData.First().PATIENT_TYPE_ID; 
                PATIENT_TYPE_NAME = ListData.First().PATIENT_TYPE_NAME; 
                SERVICE_TYPE_ID = service.SERVICE_TYPE_ID; 
                SERVICE_TYPE_NAME = ListData.First().SERVICE_TYPE_NAME; 
                SERVICE_ID = ListData.First().SERVICE_ID; 
                SERVICE_CODE = service.SERVICE_CODE; 
                SERVICE_NAME = ListData.First().TDL_SERVICE_NAME; 
                AMOUNT = ListData.Sum(s => s.AMOUNT); 
                COST_PRICE = service.COGS ?? 0; 
                FEE_PRICE = (ListData.First().VIR_PRICE ?? 0) - (service.COGS ?? 0); 
                TOTAL_COST_PRICE = COST_PRICE * AMOUNT; 
                TOTAL_FEE_PRICE = FEE_PRICE * AMOUNT; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
