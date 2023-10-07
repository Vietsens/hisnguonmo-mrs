using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisMedicinePatyViewFilter : FilterBase
    {
        public long? PATIENT_TYPE_ID {get;set;}
        public long? MEDICINE_ID {get;set;}
        public List<long> MEDICINE_IDs { get; set; }
             
        public HisMedicinePatyViewFilter()
            : base()
        {
        }
    }
}
