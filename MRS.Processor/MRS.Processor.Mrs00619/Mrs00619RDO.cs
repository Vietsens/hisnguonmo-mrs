using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00619
{
    public class Mrs00619RDO
    {
        public long ID { get; set; }
        public Decimal? VIR_PRICE { get; set; }//1
        public string REQUEST_USERNAME { get; set; }//2
        public long? INTRUCTION_TIME { get; set; } //3
        public string INTRUCTION_TIME_STR { get; set; } //3
        public long? FINISH_TIME { get; set; }//4
        public string FINISH_TIME_STR { get; set; }//4
        public long? START_TIME { get; set; }//4
        public string START_TIME_STR { get; set; }//4
        public string TREATMENT_CODE { get; set; }//5
        public string STORE_CODE { get; set; }//so luu tru//6
        public string IN_CODE { get; set; }//so vo vien//7
        public string HEIN_CARD_NUMBER { get; set; }//8
        public string EXECUTE_ROOM_NAME { get; set; }//9
        public string PATIENT_NAME { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string IS_BHYT { get; set; }
        public string ICD_NAME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string SERVICE_NAME { get; set; }
        public string RESULT { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string PATIENT_CODE { get; set; }

        public decimal? MALE_AGE { get; set; }
        public decimal? FEMALE_AGE { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal? VIR_TOTAL_PRICE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public long REQUEST_ROOM_ID { get; set; }

        public long EXECUTE_ROOM_ID { get; set; }

        public long REQUEST_DEPARTMENT_ID { get; set; }

        public long PATIENT_TYPE_ID { get; set; }

        public string STATUS_TREATMENT { get; set; }
    }
}
