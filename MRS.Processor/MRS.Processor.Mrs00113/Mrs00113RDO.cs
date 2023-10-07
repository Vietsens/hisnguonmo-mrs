using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00113
{
    class Mrs00113RDO
    {
        public long TREATMENT_ID { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }

        public long PATIENT_ID { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }

        public long SERVICE_TYPE_ID { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        public long SERVICE_ID { get;  set;  }
        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }
        public decimal? TOTAL_DISCOUNT { get;  set;  }

        public Mrs00113RDO() { }

        public Mrs00113RDO(List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> hisSereServs, MOS.EFMODEL.DataModels.HIS_SERVICE_REQ serviceReq)
        {
            try
            {
                if (hisSereServs != null && hisSereServs.Count > 0)
                {
                    TREATMENT_ID = hisSereServs.First().TDL_TREATMENT_ID ?? 0; 
                    TREATMENT_CODE = hisSereServs.First().TDL_TREATMENT_CODE; 
                    PATIENT_ID = hisSereServs.First().TDL_PATIENT_ID ?? 0; 
                    PATIENT_CODE = serviceReq.TDL_PATIENT_CODE; 
                    VIR_PATIENT_NAME = serviceReq.TDL_PATIENT_NAME; 
                    SERVICE_TYPE_ID = hisSereServs.First().TDL_SERVICE_TYPE_ID; 
                    SERVICE_TYPE_CODE = hisSereServs.First().SERVICE_TYPE_CODE; 
                    SERVICE_TYPE_NAME = hisSereServs.First().SERVICE_TYPE_NAME; 
                    SERVICE_UNIT_NAME = hisSereServs.First().SERVICE_UNIT_NAME; 
                    SERVICE_ID = hisSereServs.First().SERVICE_ID; 
                    SERVICE_CODE = hisSereServs.First().TDL_SERVICE_CODE; 
                    SERVICE_NAME = hisSereServs.First().TDL_SERVICE_NAME; 
                    AMOUNT = hisSereServs.Sum(s => s.AMOUNT); 
                    TOTAL_PRICE = hisSereServs.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0)); 
                    TOTAL_DISCOUNT = hisSereServs.Sum(s => s.DISCOUNT); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
