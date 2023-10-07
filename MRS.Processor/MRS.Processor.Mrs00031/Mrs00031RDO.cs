using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00031
{
    class Mrs00031RDO:HIS_SERE_SERV
    {
        
        public string EXECUTE_LOGINNAME { get;  set;  }
        public string EXECUTE_USERNAME { get;  set;  }
        public Mrs00031RDO() { }

        public Mrs00031RDO(HIS_SERE_SERV r, List<HIS_SERVICE_REQ> listHisServiceReq)
        {
            PropertyInfo[] p = typeof(HIS_SERE_SERV).GetProperties();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
            }
            var servcieReq = listHisServiceReq.FirstOrDefault(o=>o.ID==r.SERVICE_REQ_ID)??new HIS_SERVICE_REQ();
            this.EXECUTE_LOGINNAME = servcieReq.EXECUTE_LOGINNAME;
            this.EXECUTE_USERNAME = servcieReq.EXECUTE_USERNAME;
           
        }
    }
}
