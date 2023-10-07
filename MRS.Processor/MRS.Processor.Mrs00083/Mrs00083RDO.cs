using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00083
{
    public class Mrs00083RDO
    {
        public decimal? PRICE { get; set; }//1
        public string REQUEST_USERNAME { get; set; }//2
        public string REQUEST_LOGINNAME { get; set; }//2
        public string INTRUCTION_TIME { get; set; } //3
        public long? TDL_INTRUCTION_DATE { get; set; } //3
        public long? TDL_INTRUCTION_TIME { get; set; } //3
        public string FINISH_TIME_STR { get; set; }//4
        public long? finish_Time { get; set; }//4
        public string START_TIME { get; set; }
        public long? TDL_START_TIME { get; set; }
        public long intruction_Time { get; set; }
        public string TREATMENT_CODE { get;  set;  }//5
        public string STORE_CODE { get;  set;  }//so luu tru//6
        public string IN_CODE { get;  set; }//so vo vien//7
        public string HEIN_CARD_NUMBER { get; set; }//8
        public string TDL_HEIN_CARD_NUMBER { get; set; }//8
        public string EXECUTE_ROOM { get; set; }//9
        public long? EXECUTE_ROOM_ID { get; set; }//9
        public long? PATIENT_TYPE_ID { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string PATIENT_TYPE_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string ICD_FUEX { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public string REQUEST_ROOM { get; set; }
        public string SERVICE_NAME { get;  set;  }
        public string FUEX_RESULT { get; set; }
        public string CONCLUDE { get; set; }
        public string DESCRIPTION { get; set; }
        public string EXECUTE_USERNAME { get;  set;  }
        public long? EXECUTE_TIME { get; set; }
        public string EXECUTE_DATE_STR { get; set; }

        public string TDL_PATIENT_TYPE_NAME { get; set; }//đối tượng bệnh nhân

        public string NOTE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string MALE_YEAR { get;  set;  }
        public decimal? MALE_AGE { get;  set;  }
        public decimal? FEMALE_AGE { get;  set;  }
        public string PATIENT_TYPE_NAME_01 { get; set; }
        public string PATIENT_TYPE_NAME_1 { get; set; }

        public string MEDI_ORG_CODE { get; set; }

        public long? IN_TIME { get; set; }

        public long? OUT_TIME { get; set; }

        public long? BEGIN_TIME { get; set; }

        public long? END_TIME { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public string ICD_TEXT { get; set; }

        public string ICD_NAME { get; set; }

        public long? TDL_PATIENT_DOB { get; set; }

        public long? TDL_PATIENT_GENDER_ID { get; set; }

        public long? TDL_TREATMENT_ID { get; set; }

        public string PATIENT_TYPE_CODE_1 { get; set; }

        public string EXECUTE_ROOM_NAME { get; set; }

        public string EXECUTE_DEPARTMENT_NAME { get; set; }

        public string REQUEST_ROOM_NAME { get; set; }

        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public string MACHINE_CODE { get; set; }

        public string MACHINE_NAME { get; set; }
        public string EXECUTE_MACHINE_NAME { get; set; }
        public string EXECUTE_MACHINE_CODE { get; set; }

        public class SSE
        {
            public long? ID { get; set; }
            public long? SERE_SERV_ID { get; set; }
            public long? TEST_INDEX_ID { get; set; }
            public long? MACHINE_ID { get; set; }
            public string VALUE { get; set; }
            public string DESCRIPTION { get; set; }
            public string CONCLUDE { get; set; }
            public string NOTE { get; set; }
            public string LEAVEN { get; set; }
        }
    }
}
