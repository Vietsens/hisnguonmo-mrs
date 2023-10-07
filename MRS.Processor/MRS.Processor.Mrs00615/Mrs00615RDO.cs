using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00615
{
    public class Mrs00615RDO
    {
        public string INTRUCTION_TIME_STR { get; set; }
        public Dictionary<string, decimal> DIC_ROOM_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_DEPARTMENT_AMOUNT { get; set; }

        public long INTRUCTION_DATE { get; set; }
        public Mrs00615RDO()
        {
            DIC_ROOM_AMOUNT = new Dictionary<string, decimal>();
            DIC_DEPARTMENT_AMOUNT = new Dictionary<string, decimal>();
        }
    }
}
