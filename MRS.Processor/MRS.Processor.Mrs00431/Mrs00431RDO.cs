using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00431
{
    public class Mrs00431RDO
    {
        public string TREATMENT_CODE { get;  set;  }//
        public string PATIENT_NAME { get;  set;  }//Lấy từ bảng V_HIS_TREATMENT_SUMMARY qua treatment_ID
        public int? AGE { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }////Lấy từ bảng HIS_PATY_ALTER_BHYT treatment_ID, qua bảng his_treatment_log treatment_log_id
        public string DEPARTMENT_NAME { get;  set;  }//Lấy từ bảng V_HIS_TREATMENT_SUMMARY
        public string ICD_NAME { get;  set;  }//Lấy từ bảng V_HIS_TREATMENT_SUMMARY
        public string MEDI_ORG_NAME { get;  set;  }//
        public string MEDI_ORG_CODE { get;  set;  }
        public string OUT_TIME_STR { get;  set;  }
        public long? OUT_TIME { get;  set;  }// thời gian kết thúc điều trị
        public long? OUT_DATE { get;  set;  }
        public int NUM_ORDER { get;  set;  } // số thứ tự tăng dần
        public string ADDRESS { get;  set;  }// địa chỉ
        public string END_USERNAME { get;  set;  }// người kết thúc điều trị
        public string ICD_CODE { get;  set;  }
        public string TREATMENT_TYPE_NOI_TRU { get;  set;  }
        public string TREATMENT_TYPE_NGOAI_TRU { get;  set;  }
        public string END_DEPARTMENT_NAME { get;  set;  }
    }
}
