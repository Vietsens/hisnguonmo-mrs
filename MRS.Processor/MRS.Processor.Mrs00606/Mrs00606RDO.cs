using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00606
{
    public class Mrs00606RDO
    {
        public string DEPARTMENT_NAME { get; set; }

        public Dictionary<string,int> DIC_TYPE_TREAT { get; set; }
        public Dictionary<string, decimal> DIC_TYPE_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_TYPE_TOTAL_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_TYPE_EXPEND { get; set; }

        public Dictionary<string, int> DIC_TREAT { get; set; }
        public Dictionary<string, decimal> DIC_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_TOTAL_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_EXPEND { get; set; }

        public Mrs00606RDO()
        {
            DIC_TYPE_TREAT = new Dictionary<string, int>();
            DIC_TYPE_TOTAL_PRICE = new Dictionary<string, decimal>();
            DIC_TYPE_AMOUNT = new Dictionary<string, decimal>();
            DIC_TYPE_EXPEND = new Dictionary<string, decimal>();
            DIC_TREAT = new Dictionary<string, int>();
            DIC_TOTAL_PRICE = new Dictionary<string, decimal>();
            DIC_AMOUNT = new Dictionary<string, decimal>();
            DIC_EXPEND = new Dictionary<string, decimal>();
        }
    }
}
