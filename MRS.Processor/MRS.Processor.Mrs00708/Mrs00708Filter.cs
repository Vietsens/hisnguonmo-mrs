using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00708
{
    
    public class Mrs00708Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public List<long> FORM_TYPE_IDs { get; set; }

    }
}
