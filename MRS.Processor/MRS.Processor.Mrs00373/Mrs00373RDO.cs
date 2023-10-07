using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00373
{
    public class Mrs00373RDO
    {
        public string SERVICE_REQUEST_CODE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string DOB { get; set; }
        public string ADDRESS { get; set; }
        public long? CLINICAL_IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? TRANSACTION_TIME { get; set; }
        public decimal? TOTAL_DATE { get; set; }
        public string NOTE { get; set; }
        public long? FEE_LOCK_TIME { get; set; }
        public decimal? TOTAL_PRICE { get; set; }
    }
}
