using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00138
{
    class Mrs00138RDO
    {
        public long SERVICE_ID { get;  set;  }

        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }

        public decimal EXAM_AMOUNT { get;  set;  }
        public decimal TREAT_AMOUNT { get;  set;  }
        public decimal VIR_PRICE { get;  set;  }

        public Mrs00138RDO() { }

        public Mrs00138RDO(V_HIS_SERE_SERV sereServ, V_HIS_MEDICINE_TYPE medicine, bool IsTreat)
        {
            try
            {
                if (sereServ != null && medicine != null)
                {
                    SERVICE_ID = sereServ.SERVICE_ID; 
                    MEDICINE_TYPE_NAME = medicine.HEIN_SERVICE_BHYT_NAME; 
                    SERVICE_UNIT_NAME = sereServ.SERVICE_UNIT_NAME; 
                    if (IsTreat)
                    {
                        TREAT_AMOUNT = sereServ.AMOUNT; 
                    }
                    else
                    {
                        EXAM_AMOUNT = sereServ.AMOUNT; 
                    }
                    VIR_PRICE = sereServ.VIR_PRICE.Value; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
