using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00682
{
    class Mrs00682RDO
    {
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }

        public long? TOTAL { get; set; }

        public long? TOTAL_MALE { get; set; }

        public long? TOTAL_MALE_UNDER_6_AGE_IN { get; set; }//số bệnh nhân nam dưới 6 tuổi cùng tỉnh
        public long? TOTAL_MALE_UPPER_6_AGE_IN { get; set; }//số bệnh nhân nam trên 6 tuổi cùng tỉnh

        public long? TOTAL_MALE_UNDER_6_AGE_OUT { get; set; }//số bệnh nhân nam dưới 6 tuổi khác tỉnh
        public long? TOTAL_MALE_UPPER_6_AGE_OUT { get; set; }//số bệnh nhân nam trên 6 tuổi khác tỉnh

        public long? TOTAL_MALE_UNDER_6_AGE_OTHER { get; set; }//số bệnh nhân nam dưới 6 tuổi không xác định tỉnh
        public long? TOTAL_MALE_UPPER_6_AGE_OHTER { get; set; }//số bệnh nhân nam trên 6 tuổi không xác định tỉnh

        public long? TOTAL_FEMALE { get; set; }

        public long? TOTAL_FEMALE_UNDER_6_AGE_IN { get; set; }//số bệnh nhân nữ dưới 6 tuổi cùng tỉnh
        public long? TOTAL_FEMALE_UPPER_6_AGE_IN { get; set; }//số bệnh nhân nữ trên 6 tuổi cùng tỉnh

        public long? TOTAL_FEMALE_UNDER_6_AGE_OUT { get; set; }//số bệnh nhân nữ dưới 6 tuổi khác tỉnh
        public long? TOTAL_FEMALE_UPPER_6_AGE_OUT { get; set; }//số bệnh nhân nữ trên 6 tuổi khác tỉnh

        public long? TOTAL_FEMALE_UNDER_6_AGE_OTHER { get; set; }//số bệnh nhân nữ dưới 6 tuổi không xác định tỉnh
        public long? TOTAL_FEMALE_UPPER_6_AGE_OHTER { get; set; }//số bệnh nhân nữ trên 6 tuổi không xác định tỉnh
    }
}
