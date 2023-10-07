using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00694
{
    public class Mrs00694RDO
    {
        public string OUT_DATE_STR { get; set; }
        public string PATIENT_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public long DOB { get; set; }

        public decimal EXAM_ROOM_COUNT { get; set; }
        public string IN_TIME_STR { get; set; }
        public string START_EXAM_TIME_STR { get; set; }
        public string RESULT_SUBCLINICAL_TIME_STR { get; set; }
        public string PRES_TIME_STR { get; set; }
        public string EXP_TIME_STR { get; set; }

        public long? OUT_TIME { get; set; }
        public long IN_TIME { get; set; }
        public long? START_EXAM_TIME { get; set; }
        public long? RESULT_SUBCLINICAL_TIME { get; set; }
        public long? PRES_TIME { get; set; }
        public long? EXP_TIME { get; set; }
        public decimal? TOTAL_TIME { get; set; }
        public decimal? AVG_TIME { get; set; }

        public string HAS_CLS { get; set; }
        public string NOTE { get; set; }
    }
}
