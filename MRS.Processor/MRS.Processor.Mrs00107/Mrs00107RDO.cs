using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00107
{
    class Mrs00107RDO
    {
        public long SERVICE_ID { get;  set;  }
        public long DEPARTMENT_ID { get;  set;  }

        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }

        public decimal AMOUNT { get;  set;  }

        public Mrs00107RDO() { }

        public Mrs00107RDO(List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> listSereServ)
        {
            try
            {
                SERVICE_ID = listSereServ.First().SERVICE_ID; 
                SERVICE_CODE = listSereServ.First().TDL_SERVICE_CODE; 
                SERVICE_NAME = listSereServ.First().TDL_SERVICE_NAME; 
                AMOUNT = listSereServ.Sum(s => s.AMOUNT); 
                DEPARTMENT_ID = listSereServ.First().TDL_REQUEST_DEPARTMENT_ID; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
