using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00039
{
    public class Mrs00039RDO : HIS_SERE_SERV
    {
        public string PATIENT_NAME { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public long? TDL_FINISH_TIME { get; set; }
        public long? START_TIME { get; set; }
        public string VIR_ADDRESS { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public string BEFORE_SURG { get; set; }
        public string AFTER_SURG { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string SURG_PPPT { get; set; }
        public string SURG_PPPT_2 { get; set; }
        public string SURG_PPVC { get; set; }
        public string SURG_PPVC_2 { get; set; }
        public string TIME_SURG_STR { get;  set;  }
        public string SURG_TYPE_NAME { get;  set;  }
        public string DEFAULT_SURG_TYPE_NAME { get;  set;  }
        public string NOTE { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string MALE_YEAR { get;  set;  }
        public string DESCRIPTION { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public decimal? MALE_AGE { get;  set;  }
        public decimal? FEMALE_AGE { get;  set;  }
        public Dictionary<string, string> DICR_EXECUTE_USERNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string FINISH_TIME { get; set; }
        public long finish_Time { get; set; }
       
        public string PTTT_PRIORITY_NAME { get; set; }
        public string PTTT_PRIORITY_CC { get; set; }
        public string PTTT_PRIORITY_CT { get; set; }

        public string PTTT_TABLE_NAME { get; set; }

        public string EMOTIONLESS_RESULT_NAME { get; set; }

        public string END_TIME_STR { get; set; }
        public string TDL_EXECUTE_DEPARTMENT_NAME { get; set; }
        public string TDL_REQUEST_DEPARTMENT_NAME { get; set; }

        public string INSURANCE_CARD_NUMBER { get; set; }
        public string MEDI_ORG_CODE { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string CATEGORY_NAME { get; set; }

        public Mrs00039RDO(HIS_SERE_SERV data)
        {
           
        }
        public Mrs00039RDO() { }

        public string IS_FEE { get; set; }

        public long SERVICE_ID { get; set; }

        public string SERVICE_CODE { get; set; }

        public string PTTT_GROUP_DB { get; set; }

        public string PTTT_GROUP_1 { get; set; }

        public string PTTT_GROUP_2 { get; set; }

        public string PTTT_GROUP_3 { get; set; }

        public string DESCRIPTION_SURGERY { get; set; }

        public string BEGIN_TIME_STR { get; set; }

        public string PATIENT_TYPE_CODE { get; set; }//
        public string PATIENT_TYPE_NAME { get; set; }//đối tượng thanh toán

        public string TDL_PATIENT_TYPE_CODE { get; set; }//
        public string TDL_PATIENT_TYPE_NAME { get; set; }//đối tượng bệnh nhân

        public string MACHINE_CODE { get; set; }//
        public string MACHINE_NAME { get; set; }//máy dịch vụ

        public string EXECUTE_MACHINE_CODE { get; set; }//
        public string EXECUTE_MACHINE_NAME { get; set; }//máy thực hiện

        public string PATIENT_TYPE_NAME_01 { get; set; }


        public long END_TIME_NUM { get; set; }

        public long BEGIN_TIME_NUM { get; set; }

        public string EXECUTE_ROLE_ID { get; set; }

        public string EXECUTE_ROLE_NAME { get; set; }

        public long? FEE_LOCK_TIME { get; set; }

        public string FEE_LOCK_TIME_STR { get; set; }

        public string IN_TIME_STR { get; set; }

        public string OUT_TIME_STR { get; set; }

        public string MEDICINE_TYPE_CODE { get; set; }

        public string MEDICINE_TYPE_NAME { get; set; }

        public decimal MEDICINE_AMOUNT { get; set; }

        public decimal? MEDICINE_PRICE { get; set; }

        public decimal? MEDICINE_VAT_RATIO { get; set; }

        public string HEIN_SERVICE_BHYT_CODE { get; set; }

        public long? PTTT_GROUP_ID { get; set; }

        public long? EMOTIONLESS_METHOD_ID { get; set; }

        public long? PTTT_METHOD_ID { get; set; }

        

        public Dictionary<string, decimal> DIC_REQ_DEPARTMENT_AMOUNT { get; set; }



        public decimal SERVICE_AMOUNT { get; set; }

        public string INTRUCTION_NOTE { get; set; }
    }
}
