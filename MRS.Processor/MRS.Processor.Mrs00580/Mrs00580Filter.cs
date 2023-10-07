using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00580
{
    public class Mrs00580Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public long? REQ_DEPARTMENT_ID { get; set; }

        public long? MEDI_STOCK_ID { get; set; }

        public List<long> EXP_MEST_TYPE_IDs { get; set; }
    }
}