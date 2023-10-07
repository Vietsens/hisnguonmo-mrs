using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00058
{
    public class Mrs00058RDO
    {
        public string SERVICE_REQ_CODE { get; set; }
        public Decimal? PRICE { get; set; }//1
        public string REQUEST_USERNAME { get; set; }//2
        public long? INTRUCTION_TIME_NUM { get; set; } //3
        public string INTRUCTION_TIME { get; set; } //3
        public string FINISH_TIME_STR { get; set; }//4
        public long? START_TIME { get; set; }//4
        //public long? finish_Time { get; set; }//4
        public long? FINISH_TIME { get; internal set; }
        public long? TDL_PATIENT_DOB { get; set; }//4
        public string TREATMENT_CODE { get; set; }//5
        public string STORE_CODE { get; set; }//so luu tru//6
        public string IN_CODE { get; set; }//so vo vien//7
        public string HEIN_CARD_NUMBER { get; set; }//8
        public string EXECUTE_ROOM { get; set; }//9
        public string PATIENT_NAME { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string TDL_PATIENT_TYPE_NAME { get; set; }//đối tượng bệnh nhân

        public string VIR_ADDRESS { get; set; }
        public string IS_BHYT { get; set; }
        public string ICD_SUIM { get; set; }
        public string REQUEST_ROOM { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SUIM_RESULT { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string NOTE { get; set; }
        public string PATIENT_CODE { get; set; }

        public decimal? MALE_AGE { get; set; }
        public decimal? FEMALE_AGE { get; set; }

        public long? END_TIME { get; set; }
        public long? BEGIN_TIME { get; set; }

        public decimal? AMOUNT { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }

        public string PATIENT_TYPE_NAME_1 { get; set; }

        public long IN_TIME { get; set; }

        public string EXECUTE_DATE_STR { get; set; }
        public long? EXECUTE_TIME { get; set; }

        public long? OUT_TIME { get; set; }
        public string TDL_HEIN_SERVICE_BHYT_NAME { get;  set; }
        public string TDL_HEIN_SERVICE_BHYT_CODE { get;  set; }
        public long? TDL_PATIENT_ID { get;  set; }
        public long? PATIENT_TYPE_ID { get;  set; }
        public long? TDL_PATIENT_TYPE_ID { get;  set; }
        public string ICD_NAME { get;  set; }
        public string ICD_TEXT { get;  set; }
        public long? REQUEST_ROOM_ID { get;  set; }
        public long? EXECUTE_ROOM_ID { get;  set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public long? MACHINE_ID { get; set; }
        public string MACHINE_NAME { get; set; }
        public string MACHINE_CODE { get; set; }
        public string EXECUTE_MACHINE_NAME { get; set; }
        public string EXECUTE_MACHINE_CODE { get; set; }

        public long SERVICE_ID { get; set; }
    }
}
