using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00230
{
    public class Mrs00230RDO
    {
        public long? PARENT_ID { get;  set;  }

        public string ICD_GROUP_NAME { get; set; }//tên nhóm
        public string ICD_GROUP_CODE { get; set; }//mã nhóm
        public string ICD_NAME { get; set; }//tên bệnh
        public string ICD_CODE { get; set; }//mã bệnh
        public string ICD_CODE_SUB { get;  set;  }
        public int? TOTAL_EXAMINATION { get;  set;  }//tổng số ca tại khoa khám bệnh
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

        public decimal? TOTAL_DEAD_BOARDING_DATE { get; set; } //tổng số ngày điều trị tử vong điều trị nội trú
        public decimal? TOTAL_FEMALE_DEAD_BOARDING_DATE { get; set; } //tổng số ngày điều trị nữ tử vong điều trị nội trú
        public decimal? TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING_DATE { get; set; }	//tổng số ngày điều trị của trẻ em dưới 15 tuổi điều trị tử vong
        public decimal? TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING_DATE { get; set; }	//tổng số ngày điều trị của trẻ em dưới 5 tuổi điều trị tử vong
        public int? TOTAL_SICK_BOARDING_MORE_60_AGE { get; set; }		//tổng số ngày điều trị của người già trên 60 tuổi điều trị mắc bệnh
        public int? TOTAL_FEMALE_SICK_BOARDING_MORE_60_AGE { get; set; }	//tổng số ngày điều trị của người già nữ trên 60 tuổi điều trị mắc bệnh
        public int? TOTAL_DEAD_BOARDING_MORE_60_AGE { get; set; }//tổng số ngày điều trị của người già trên 60 tuổi điều trị tử vong	
        public int? TOTAL_FEMALE_DEAD_BOARDING_MORE_60_AGE { get; set; }//tổng số ngày điều trị của người già nữ trên 60 tuổi điều trị tử vong
        public int? TOTAL_POLICE { get; set; }//tổng số cán bộ cơ sở

        public string IS_CAUSE { get; set; }//Là nguyên nhân gây bệnh


        public int? HEAVY_EXAMINATION { get; set; }//khám bệnh nặng xin về

        public int? DEATH_BEFORE_EXAMINATION { get; set; }//khám tử vong trước khi vào viện

        public int? DEATH_ON_EXAMINATION { get; set; }//khám tử vong tại viện

        public int? TOTAL_HEAVY_SICK_BOARDING { get; set; }// điều trị nặng xin về

        public int? TOTAL_HEAVY_FEMALE_SICK_BOARDING { get; set; }// điều trị nặng xin về nữ

        public int? TOTAL_DEAD_DOCUMENT_BOARDING { get; set; }
    }
   
    public class Ms00230RDO_DEAD_488
	{
        public string ICD_NAME { get; set; }//tên bệnh
        public string ICD_CODE { get; set; }//mã bệnh
        public string ICD_CODE_SUB { get; set; }

        public decimal? TOTAL_0_TO_28_DAY { get; set; }
        public decimal? TOTAL_28_DAY_TO_1_AGE { get; set; }
        public decimal? TOTAL_1_AGE_TO_5_AGE { get; set; }
        public decimal? TOTAL_5_AGE_TO_10_AGE { get; set; }
        public decimal? TOTAL_10_AGE_TO_15_AGE { get; set; }
        public decimal? TOTAL_15_AGE_TO_20_AGE { get; set; }
        public decimal? TOTAL_20_AGE_TO_30_AGE { get; set; }
        public decimal? TOTAL_30_AGE_TO_40_AGE { get; set; }
        public decimal? TOTAL_40_AGE_TO_50_AGE { get; set; }
        public decimal? TOTAL_50_AGE_TO_60_AGE { get; set; }
        public decimal? TOTAL_60_AGE_TO_70_AGE { get; set; }
        public decimal? TOTAL_MORE_70_AGE { get; set; }
        public decimal? TOTAL_FEMALE_0_TO_28_DAY { get; set; }
        public decimal? TOTAL_FEMALE_28_DAY_TO_1_AGE { get; set; }
        public decimal? TOTAL_FEMALE_1_AGE_TO_5_AGE { get; set; }
        public decimal? TOTAL_FEMALE_5_AGE_TO_10_AGE { get; set; }
        public decimal? TOTAL_FEMALE_10_AGE_TO_15_AGE { get; set; }
        public decimal? TOTAL_FEMALE_15_AGE_TO_20_AGE { get; set; }
        public decimal? TOTAL_FEMALE_20_AGE_TO_30_AGE { get; set; }
        public decimal? TOTAL_FEMALE_30_AGE_TO_40_AGE { get; set; }
        public decimal? TOTAL_FEMALE_40_AGE_TO_50_AGE { get; set; }
        public decimal? TOTAL_FEMALE_50_AGE_TO_60_AGE { get; set; }
        public decimal? TOTAL_FEMALE_60_AGE_TO_70_AGE { get; set; }
        public decimal? TOTAL_FEMALE_MORE_70_AGE { get; set; }
        public decimal? TOTAL_TV_ME { get; set; }

        public decimal? TOTAL_DEAD { get; set; }
            public decimal? TOTAL_DEAD_FEMALE { get; set; }

            public string ICD_GROUP_CODE { get; set; }

            public string ICD_GROUP_NAME { get; set; }
    }
    public class Mrs00230RDO_NEW
    {
        public string ICD_NAME { get; set; }//tên bệnh
        public string ICD_CODE { get; set; }//mã bệnh
        public string ICD_CODE_SUB { get; set; }
        public string ICD_GROUP_CODE { get; set; }
        public string ICD_GROUP_NAME { get; set; }

        public decimal? TOTAL_LESS_THAN_1_AGE { get; set; }
        public decimal? TOTAL_FEMALE_LESS_THAN_1_AGE { get; set; }
        public decimal? TOTAL_TREATMENT_DAY_LESS_THAN_1_AGE { get; set; }
        public decimal? TOTAL_TREATMENT_DAY_FEMALE_LESS_THAN_1_AGE { get; set; }
        public decimal? TOTAL_DEATH_LESS_THAN_1_AGE { get; set; }

        public decimal? TOTAL_LESS_THAN_5_AGE { get; set; }
        public decimal? TOTAL_FEMALE_LESS_THAN_5_AGE { get; set; }
        public decimal? TOTAL_TREATMENT_DAY_LESS_THAN_5_AGE { get; set; }
        public decimal? TOTAL_TREATMENT_DAY_FEMALE_LESS_THAN_5_AGE { get; set; }
        public decimal? TOTAL_DEATH_LESS_THAN_5_AGE { get; set; }

        public decimal? TOTAL_LESS_THAN_15_AGE { get; set; }
        public decimal? TOTAL_FEMALE_LESS_THAN_15_AGE { get; set; }
        public decimal? TOTAL_TREATMENT_DAY_LESS_THAN_15_AGE { get; set; }
        public decimal? TOTAL_TREATMENT_DAY_FEMALE_LESS_THAN_15_AGE { get; set; }
        public decimal? TOTAL_DEATH_LESS_THAN_15_AGE { get; set; }

        public decimal? TOTAL_MORE_THAN_60_AGE { get; set; }
        public decimal? TOTAL_FEMALE_MORE_THAN_60_AGE { get; set; }
        public decimal? TOTAL_TREATMENT_DAY_MORE_THAN_60_AGE { get; set; }
        public decimal? TOTAL_TREATMENT_DAY_FEMALE_MORE_THAN_60_AGE { get; set; }
        public decimal? TOTAL_DEATH_MORE_THAN_60_AGE { get; set; }
        public decimal? TOTAL_FEMALE_DEATH_MORE_THAN_60_AGE { get; set; }

        public decimal? TOTAL_RA_VIEN { get; set; }
        public decimal? TOTAL_FEMALE_RA_VIEN { get; set; }
        public decimal? TOTAL_TREATMENT_DAY_RA_VIEN { get; set; }
        public decimal? TOTAL_TREATMENT_DAY_FEMALE_RA_VIEN { get; set; }
        public decimal? TOTAL_DEAD_RA_VIEN { get; set; }

        public Dictionary<string,int> DIC_DEATH_24H_AMOUNT { get; set; }
        public Dictionary<string, int> DIC_DEATH_FEMALE_24H_AMOUNT { get; set; }
        public decimal TOTAL_VAO_VIEN { get; set; }
    }
    public class Mrs00230RDO_PARENT
    {
        public HIS_ICD_GROUP ICD_GROUP { get;  set;  }
    }
    public class COUNT_IN
    {
        public string ICD_CODE { get; set; }
        public decimal TOTAL_VAO_VIEN { get; set; }
    }

    public class ICD_GROUP_DETAIL
    {
        public string ICD_CODE { get; set; }
        public string ICD_GROUP { get; set; }
        public string ICD_PARENT_GROUP { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? LAST_DEPARTMENT_ID { get; set; }
        public string LAST_DEPARTMENT_NAME { get; set; }
    }
}
