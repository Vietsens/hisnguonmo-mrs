using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00661
{
    class Mrs00661RDO
    {
        public string ICD_NAME { get; set; }//tên bệnh
        public string ICD_CODE { get; set; }//mã bệnh
        public string ICD_CODE_SUB { get; set; }
        public int? TOTAL_EXAMINATION { get; set; }//tổng số ca tại khoa khám bệnh
        public int? FEMALE_EXAMINATION { get; set; }//nữ tại khoa khám bệnh
        public int? CHILDREN_UNDER_15_AGE_EXAMINATION { get; set; }//số trẻ em dưới 15 tuổi mắc bệnh
        public int? CHILDREN_UNDER_5_AGE_EXAMINATION { get; set; }//số trẻ em dưới 5 tuổi mắc bệnh
        public int? TOTAL_DEAD_EXAMINATION { get; set; }//số ca tử vong
        public int? CHILDREN_UNDER_15_AGE_DEAD_EXAMINATION { get; set; }//số trẻ em dưới 15 tuổi tử vong
        public int? CHILDREN_UNDER_5_AGE_DEAD_EXAMINATION { get; set; }//số trẻ em dưới 5 tuổi tử vong

        public int? TOTAL_SICK_BOARDING { get; set; }//tổng số bệnh nhân mắc bệnh điều trị nội trú
        public int? TOTAL_FEMALE_SICK_BOARDING { get; set; }//tổng số nữ mắc bệnh điều trị nội trú
        public int? TOTAL_DEAD_BOARDING { get; set; }//tổng số bệnh nhân tử vong điều trị nội trú
        public int? TOTAL_FEMALE_DEAD_BOARDING { get; set; }//tổng số nữ tử vong điều trị nội trú

        public int? TOTAL_CHILDREN_UNDER_15_AGE_SICK_BOARDING { get; set; }//trẻ em dưới 15 tuổi mắc bệnh điều trị nội trú
        public int? TOTAL_CHILDREN_UNDER_5_AGE_SICK_BOARDING { get; set; }//trẻ em dưới 5 tuổi mắc bệnh điều trị nội trú
        public int? TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING { get; set; }//tổng số trẻ em dưới 15 tuổi chết điều trị nội trú
        public int? TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING { get; set; }//tổng số trẻ em dưới 5 tuổi chết điều trị nội trú

        public string IS_CAUSE { get; set; }//Là nguyên nhân gây bệnh

        public decimal? TOTAL_BOARDING_DATE { get; set; } //tổng số ngày điều trị tử vong điều trị nội trú
        public decimal? TOTAL_FEMALE_BOARDING_DATE { get; set; } //tổng số ngày điều trị nữ tử vong điều trị nội trú
        public decimal? TOTAL_CHILDREN_UNDER_15_AGE_BOARDING_DATE { get; set; }	//tổng số ngày điều trị của trẻ em dưới 15 tuổi điều trị
        public decimal? TOTAL_CHILDREN_UNDER_5_AGE_BOARDING_DATE { get; set; }	//tổng số ngày điều trị của trẻ em dưới 5 tuổi điều trị

        public string ROOM_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
    }
    public class HIS_TREATMENT_D
    {
        public long id { get; set; }
        public string icd_code { get; set; }
        public string icd_cause_code { get; set; }
        public long tdl_patient_gender_id { get; set; }
        public long tdl_patient_type_id { get; set; }
        public long tdl_treatment_type_id { get; set; }
        public long? out_time { get; set; }
        public long in_time { get; set; }
        public short? is_pause { get; set; }
        public long? tdl_patient_dob { get; set; }
        public long? treatment_end_type_id { get; set; }
        public string icd_name { get; set; }
        public long? execute_room_id { get; set; }
        public decimal? treatment_day_count { get; set; }
        public int template_type { get; set; }
    }

    public class SUM_ICD_OUT_TREAT
    {
        public string ICD_CODE { get; set; }
        public string ICDVN_CODE { get; set; }
        public string ICD_CAUSE_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public int TOTAL { get; set; }
        public int TOTAL_FEMALE { get; set; }
        public int TOTAL_BHYT { get; set; }
        public int TOTAL_CHILD { get; set; }
        public int TOTAL_DEATH { get; set; }
    }

    public class SUM_ICD_IN_TREAT
    {
        public string ICD_CODE { get; set; }
        public string ICDVN_CODE { get; set; }
        public string ICD_CAUSE_CODE { get; set; }
        public string ICDVN_CAUSE_CODE { get; set; }
        public string ICD_NAME { get; set; }

        public int TOTAL { get; set; }
        public int TOTAL_DEATH { get; set; }
        public decimal? TOTAL_DAY { get; set; }

        public int TOTAL_CHILD { get; set; }
        public int TOTAL_CHILD_DEATH { get; set; }
        public decimal? TOTAL_CHILD_DAY { get; set; }

        public int TOTAL_CHILD4 { get; set; }
        public int TOTAL_CHILD4_DEATH { get; set; }
        public decimal? TOTAL_CHILD4_DAY { get; set; }

    }
}
