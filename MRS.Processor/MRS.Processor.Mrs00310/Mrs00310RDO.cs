using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00310
{
    public class Mrs00310RDO
    {
        public string CREATOR { get; set; }
        public string CREATE_USERNAME { get; set; }
        public long TREATMENT_ID { get; set; }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string IS_BHYT { get;  set;  }// có phải là bhyt 
        public string IS_DUOI_12THANG { get;  set;  }// dưới 12 tháng
        public string IS_1DEN15TUOI { get;  set;  }// 1 đến 15 tuổi
        public string VIR_ADDRESS { get;  set;  }
        public string GIOITHIEU { get;  set;  }
        public string DATE_IN_STR { get;  set;  }// ngày vào 
        public string TIME_IN_STR { get;  set;  }
        public string DIAGNOSE_TUYENDUOI { get;  set;  }// chuẩn đoán tuyến dưới
        public string DIAGNOSE_KKB { get;  set;  }//chẩn đoán khoa khám bệnh
        public string DIAGNOSE_KDT { get;  set;  }// chẩn đoán khoa điều trị
        public string HEIN_CARD_NUMBER { get;  set;  }
        public long? ICD_ID { get; set; }
        public string ICD_NAME { get; set; }
        public string IN_LOGINNAME { get; set; }
        public string IN_USERNAME { get; set; }

        public string MALE_YEAR { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }


        public int? MALE_AGE { get;  set;  }

        public int? FEMALE_AGE { get;  set;  }

        public long? CLINICAL_IN_TIME { get; set; }

        public long? GIVE_DEPARTMENT_ID { get; set; }

        public string GIVE_DEPARTMENT_NAME { get; set; }// khoa vào

        public long? IN_DEPARTMENT_ID { get; set; }

        public string IN_DEPARTMENT_NAME { get; set; }// khoa vào

        public string DEPARTMENT_IN_TIME_STR { get; set; }//thời gian vào khoa

        public string DEPARTMENT_OUT_TIME_STR { get; set; }// thời gian ra khoa

        public string OUT_TIME_STR { get; set; }// thời gian ra viện

        public string IN_CODE { get; set; }// số vào viện
        public string STORE_CODE { get; set; }// số lưu trữ hồ sơ bệnh án
    }
}
