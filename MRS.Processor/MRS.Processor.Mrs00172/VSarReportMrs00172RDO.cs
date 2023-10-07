using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00172
{
    public class VSarReportMrs00172RDO
    {
        public string PATIENT_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string TREATMENT_CODE { get; set; }
        public decimal? VIR_PATIENT_PRICE { get; set; }
        public string DV { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
    }
}
