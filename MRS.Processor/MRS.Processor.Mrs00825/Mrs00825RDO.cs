using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00825
{
    public class Mrs00825RDO
    {
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_GENDER_NAME { get; set; }
        public string PATIENT_DOB_STR { get; set; }
        public string PATIENT_AGE { get; set; }
        public long PATIENT_DOB { get; set; }
        public string PATIENT_ADDRESS { get; set; }
        public string CCCD_NUMBER { get; set; }
        public string CMND_NUMBER { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string DEPARTMENT_NAMEs { get; set; }
        public string DEPARTMENT_NAME_CODEs { get; set; }
        public string ROOM_NAMEs { get; set; }
        public string ROOM_CODEs { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
    }
}
