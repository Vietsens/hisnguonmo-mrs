using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00139
{
    class Mrs00139RDO
    {
        public long SERVICE_ID { get;  set;  }

        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }

        public decimal EXAM_AMOUNT { get;  set;  }
        public decimal TREAT_AMOUNT { get;  set;  }
        public decimal VIR_PRICE { get;  set;  }

        public Mrs00139RDO() { }

        public Mrs00139RDO(V_HIS_SERE_SERV sereServ, bool IsTreat)
        {
            try
            {
                if (sereServ != null)
                {
                    SERVICE_ID = sereServ.SERVICE_ID; 
                    MEDICINE_TYPE_NAME = sereServ.TDL_SERVICE_NAME; 
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
