using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00378
{
    public class Mrs00378Filter
    {
        public long? MEDI_STOCK_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public bool? IS_TREATTING { get; set; }
    }
}
