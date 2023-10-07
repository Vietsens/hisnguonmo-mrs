using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00477
{
    public class Mrs00477RDO
    {
        public Decimal? PRICE { get; set; }//1
        public string REQUEST_USERNAME { get; set; }//2
        public long INTRUCTION_TIME { get; set; } //3
        public string INTRUCTION_TIME_STR { get; set; } //3
        public long FINISH_TIME { get; set; }//4
        public string FINISH_TIME_STR { get; set; }//4
        public long START_TIME { get; set; }//4
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

        public long? BEGIN_TIME { get; set; }

        public string BEGIN_TIME_STR { get; set; }

        public string END_TIME_STR { get; set; }

        public long? END_TIME { get; set; }
    }
}
