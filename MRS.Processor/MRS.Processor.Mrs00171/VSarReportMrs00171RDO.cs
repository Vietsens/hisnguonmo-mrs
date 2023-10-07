using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00171
{
    public class VSarReportMrs00171RDO
    {
        public string SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal TOTAL_AMOUNT { get; set; }
    }
}
