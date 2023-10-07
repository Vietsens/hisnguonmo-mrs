using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00616
{
    public class Mrs00616RDO
    {
        public HIS_SERVICE_REQ HisServiceReq { get; set; }
        public HIS_SERE_SERV HisSereServ { get; set; }
        public decimal AMOUNT { get; set; }
        public Mrs00616RDO(HIS_SERE_SERV r,HIS_SERVICE_REQ s)
        {
            this.HisSereServ = r;
            this.HisServiceReq = s;
        }

    }
}
