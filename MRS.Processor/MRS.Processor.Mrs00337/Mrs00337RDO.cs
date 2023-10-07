using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00337
{
    public class Mrs00337RDO:HIS_SERVICE_REQ
    {
        public long ICD_GROUP_ID { get; set; }
        public string ICD_GROUP_CODE { get; set; }
        public string ICD_GROUP_NAME { get; set; }
        public int? TOTAL_EXAMINATION { get; set; }//tổng số ca tại khoa khám bệnh
        public int? FEMALE_EXAMINATION { get;  set;  }//nữ tại khoa khám bệnh
        public int? CHILDREN_UNDER_15_AGE_EXAMINATION { get;  set;  }//số trẻ em dưới 15 tuổi mắc bệnh
        public int? TOTAL_DEAD_EXAMINATION { get;  set;  }//số ca tử vong


        public int? TOTAL_SICK_BOARDING { get;  set;  }//tổng số bệnh nhân mắc bệnh điều trị nội trú
        public int? TOTAL_FEMALE_SICK_BOARDING { get;  set;  }//tổng số nữ mắc bệnh điều trị nội trú
        public int? TOTAL_DEAD_BOARDING { get;  set;  }//tổng số bệnh nhân tử vong điều trị nội trú
        public int? TOTAL_FEMALE_DEAD_BOARDING { get;  set;  }//tổng số nữ tử vong điều trị nội trú

        public int? TOTAL_CHILDREN_UNDER_15_AGE_SICK_BOARDING { get;  set;  }//trẻ em dưới 15 tuổi mắc bệnh điều trị nội trú
        public int? TOTAL_CHILDREN_UNDER_5_AGE_SICK_BOARDING { get;  set;  }//trẻ em dưới 5 tuổi mắc bệnh điều trị nội trú
        public int? TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING { get;  set;  }//tổng số trẻ em dưới 15 tuổi chết điều trị nội trú
        public int? TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING { get;  set;  }//tổng số trẻ em dưới 5 tuổi chết điều trị nội trú


        public int? TOTAL_DEAD_BEFORE { get; set; }

        public int? TOTAL_NANG_XINVE { get; set; }
    }

}
