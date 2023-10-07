using System;
using System.Collections.Generic;
using System.Text;

namespace MRS.Processor.Mrs00883
{
    class Mrs00883RDO
    {
        public long MONTH { get; set; }
        public string MONTH_STR { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long AGE { get; set; }
        public long TREATMENT_END_TYPE_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public long SERVICE_REQ_STT_ID { get; set; }
        public long TREATMENT_ID { get; set; }
        public decimal AMOUNT_TEST { get; set; }
        public decimal AMOUNT_XQUANG { get; set; }
        public decimal AMOUNT_SIEUAM { get; set; }
        public decimal AMOUNT_CT { get; set; }
        public decimal AMOUNT_MRI { get; set; }
        public decimal AMOUNT_CHUYEN_TUYEN { get; set; }
    }
}
