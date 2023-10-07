using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00440
{
    class Mrs00440RDO
    {
        public long TREATMENT_ID { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public long IN_TIME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long TDL_PATIENT_GENDER_ID { get; set; }
        public long? CAREER_ID { get; set; }
        public long? ACCIDENT_LOCATION_ID { get; set; }
        public long? ACCIDENT_BODY_PART_ID { get; set; }
        public long? ACCIDENT_HURT_TYPE_ID { get; set; }
        public long? ACCIDENT_CARE_ID { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_CAUSE_CODE { get; set; }

        public long SERVICE_GROUP_ID { get; set; }
        public long GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public string SERVICE_GROUP_CODE { get; set; }
        public string SERVICE_GROUP_NAME { get; set; }
        public decimal TOTAL_STICKY { get; set; }
        public decimal TOTAL_DIE { get; set; }
        public decimal TOTAL_STICKY_FEMALE { get; set; }
        public decimal TOTAL_DIE_FEMALE { get; set; }
        public decimal TOTAL_STICKY_04 { get; set; }
        public decimal TOTAL_DIE_04 { get; set; }
        public decimal TOTAL_STICKY_FEMALE_04 { get; set; }
        public decimal TOTAL_DIE_FEMALE_04 { get; set; }
        public decimal TOTAL_STICKY_14 { get; set; }
        public decimal TOTAL_DIE_14 { get; set; }
        public decimal TOTAL_STICKY_FEMALE_14 { get; set; }
        public decimal TOTAL_DIE_FEMALE_14 { get; set; }
        public decimal TOTAL_STICKY_19 { get; set; }
        public decimal TOTAL_DIE_19 { get; set; }
        public decimal TOTAL_STICKY_FEMALE_19 { get; set; }
        public decimal TOTAL_DIE_FEMALE_19 { get; set; }
        public decimal TOTAL_STICKY_60 { get; set; }
        public decimal TOTAL_DIE_60 { get; set; }
        public decimal TOTAL_STICKY_FEMALE_60 { get; set; }
        public decimal TOTAL_DIE_FEMALE_60 { get; set; }
        public decimal TOTAL_STICKY_100 { get; set; }
        public decimal TOTAL_DIE_100 { get; set; }
        public decimal TOTAL_STICKY_FEMALE_100 { get; set; }
        public decimal TOTAL_DIE_FEMALE_100 { get; set; }
        public Mrs00440RDO() { }

    }
}
