using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00709
{
    
    public class Mrs00709Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }
        public long? MEDICINE_ID { get; set; }
        public long? MATERIAL_ID { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> MATERIAL_IDs { get; set; }
    }
}
