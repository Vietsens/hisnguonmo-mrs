using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00034
{
    public class Mrs00034Filter : HisServiceReqViewFilterQuery
    {
       public List<long> REQUEST_ROOM_IDs { get;  set;  }
        public Mrs00034Filter()
            : base()
        {

        }
    }
}
