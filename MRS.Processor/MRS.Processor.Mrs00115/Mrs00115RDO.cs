using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00115
{
    class Mrs00115RDO
    {
        public long NUMBER_ORDER { get;  set;  }
        public string PATIENTS_CODE { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENTS_NAME { get;  set;  }
        public string DATE_OF_BIRTH { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public decimal SERVICE_NUMBER { get;  set;  }
        public decimal SERVICE_PRICE { get;  set;  }
        public decimal? TOTAL_PRICE { get;  set;  }

        public Mrs00115RDO(MOS.EFMODEL.DataModels.V_HIS_SERE_SERV listSereServ, MOS.EFMODEL.DataModels.HIS_SERVICE_REQ serviceReq)
        {
            try
            {
                if (listSereServ != null)
                {
                    PATIENTS_CODE = serviceReq.TDL_PATIENT_CODE; 
                    TREATMENT_CODE = serviceReq.TDL_TREATMENT_CODE; 
                    PATIENTS_NAME = serviceReq.TDL_PATIENT_NAME; 
                    DATE_OF_BIRTH = Inventec.Common.DateTime.Convert.TimeNumberToDateString(serviceReq.TDL_PATIENT_DOB); 
                    SERVICE_NAME = listSereServ.TDL_SERVICE_NAME; 
                    SERVICE_NUMBER = listSereServ.AMOUNT; 
                    SERVICE_PRICE = listSereServ.PRICE; 
                    TOTAL_PRICE = listSereServ.VIR_TOTAL_PATIENT_PRICE; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public Mrs00115RDO() { }
    }
}
