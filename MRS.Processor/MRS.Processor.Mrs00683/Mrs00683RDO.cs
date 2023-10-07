using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00683
{
    class Mrs00683RDO
    {
        public string PROVINCE_CODE { get; set; }
        public string PROVINCE_NAME { get; set; }

        public string DISTRICT_CODE { get; set; }
        public string DISTRICT_NAME { get; set; }

        public long? TOTAL_UNDER_6_AGE { get; set; }//số bệnh nhân dưới 6 tuổi
        public long? TOTAL_UPPER_6_AGE { get; set; }//số bệnh nhân từ 6 tuổi

        public long? TOTAL_UPPER_6_AGE_MALE { get; set; }//số bệnh nhân trên 6 tuổi
        public long? TOTAL_UPPER_6_AGE_FEMALE { get; set; }//số bệnh nhân trên 6 tuổi

        public long? TOTAL_UNDER_1_AGE_MALE { get; set; }//dưới 1
        public long? TOTAL_UNDER_3_AGE_MALE { get; set; }//1 đến dưới 3
        public long? TOTAL_UNDER_5_AGE_MALE { get; set; }//3 đến dưới 5
        public long? TOTAL_6_AGE_MALE { get; set; }//5 đến 6
        public long? TOTAL_UNDER_1_AGE_FEMALE { get; set; }//dưới 1
        public long? TOTAL_UNDER_3_AGE_FEMALE { get; set; }//1 đến dưới 3
        public long? TOTAL_UNDER_5_AGE_FEMALE { get; set; }//3 đến dưới 5
        public long? TOTAL_6_AGE_FEMALE { get; set; }//5 đến 6
    }
}
