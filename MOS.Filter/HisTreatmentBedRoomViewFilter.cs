
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisTreatmentBedRoomViewFilter : FilterBase
    {
        public long? TREATMENT_ID { get; set; }
        public long? BED_ROOM_ID { get; set; }
        public long? ADD_TIME_TO { get; set; }
        public long? REMOVE_TIME_FROM { get; set; }
        public bool? IS_IN_ROOM { get; set; }
        public string KEYWORD__PATIENT_NAME__TREATMENT_CODE__BED_NAME { get; set; }
        public List<long> TREATMENT_IDs { get; set; }

        public HisTreatmentBedRoomViewFilter()
            : base()
        {
        }
    }
}
