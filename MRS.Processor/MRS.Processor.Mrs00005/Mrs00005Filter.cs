using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00005
{
    /// <summary>
    
    /// </summary>
    public class Mrs00005Filter
    {
        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }
        public bool? IS_CHEMICAL_SUBSTANCE { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public long? IMP_TIME_FROM { get; set; }
        public long? IMP_TIME_TO { get; set; }
        public long? EXPIRED_TIME_FROM { get; set; }
        public long? EXPIRED_TIME_TO { get; set; }
        public long? IMP_MEST_TYPE_ID { get; set; }
        public string KEY_GROUP_IMP { get; set; }

        public Mrs00005Filter()
            : base()
        {
        }
    }
}
