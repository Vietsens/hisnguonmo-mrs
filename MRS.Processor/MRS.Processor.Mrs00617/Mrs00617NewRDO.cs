using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00617
{
    class Mrs00617NewRDO
    {
        public long SERVICE_ID { get; set; }
        public long? PARENT_SERVICE_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal? NUMBER_OF_FILM { get; set; }       
        public decimal? PRICE { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public long IN_TIME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        //public long REPORT_TYPE_CAT_ID { get; set; }

        public string TDL_SERVICE_TYPE_NAME { get; set; }

        public decimal AMOUNT_NT { get; set; }
        public decimal AMOUNT_NGT { get; set; }

        public long TDL_REQUEST_ROOM_ID { get; set; }
        public string TDL_REQUEST_ROOM_CODE { get; set; }
        public string TDL_REQUEST_ROOM_NAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public decimal AMOUNT_KHAM { get; set; }

        public decimal AMOUNT_KHAM_BHYT { get; set; }

        public decimal AMOUNT_KHAM_VP { get; set; }

        public decimal AMOUNT_CHUYENVIEN { get; set; }

        public decimal AMOUNT_SERVICE { get; set; }
        public decimal AMOUNT_XUYEN_SO { get; set; }

        public long? TDL_TREATMENT_END_TYPE_ID { get; set; }

        public Dictionary<string, decimal> DIC_SERVICE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_SERVICE_AMOUNT { get; set; }

        public Dictionary<string, decimal> DIC_PARENT_AMOUNT { get; set; }

        public long PATIENT_TYPE_ID { get; set; }

        public string TDL_PATIENT_TYPE_NAME { get; set; }

        public string PATIENT_TYPE_NAME { get; set; }

        public int AMOUNT_CHUYEN { get; set; }

        public long COUNT_TREA_NGT { get; set; }

        public long COUNT_TREA_NT { get; set; }

        public string GRPR_SERVICE_CODE { get; set; }

        public string GRPR_SERVICE_NAME { get; set; }
    }
}
