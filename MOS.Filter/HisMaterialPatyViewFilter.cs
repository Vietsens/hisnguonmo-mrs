using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisMaterialPatyViewFilter : FilterBase
    {
        public long? PATIENT_TYPE_ID {get;set;}
        public long? MATERIAL_ID {get;set;}
        public List<long> MATERIAL_IDs { get; set; }
             
        public HisMaterialPatyViewFilter()
            : base()
        {
        }
    }
}
