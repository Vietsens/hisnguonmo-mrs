using System;
using System.Collections.Generic;
using System.Text;

namespace MOS.Filter
{
    public class HisAccidentHurtFilter : FilterBase
    {
        public List<long> TREATMENT_IDs { get; set; }
        public List<long> ACCIDENT_CARE_IDs { get; set; }
        public List<long> ACCIDENT_HELMET_IDs { get; set; }
        public long? TREATMENT_ID { get; set; }        
        public long? ACCIDENT_HURT_TYPE_ID { get; set; }
        public long? ACCIDENT_CARE_ID { get; set; }
        public long? ACCIDENT_HELMET_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public List<long> EXECUTE_ROOM_IDs { get; set; }

        public HisAccidentHurtFilter()
            : base()
        {
        }
    }
}
