using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00813
{
    public class Mrs00813RDO
    {
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_DOB { get; set; }
        public string PATIENT_ADDRESS { get; set; }
        public string IN_DATE { set; get; }
        public string OUT_DATE { set; get; }
        public long SERI_NUMBER { get; set; }
        public string BOOK_NUMBER { get; set; }
        public decimal COUNT_PRINT { set; get; }
        public string REASON_PRINT_AGAIN { set; get; }
    }
    public class PrintLogUnique
    {
        public string UNIQUE_CODE { get; set; }
        public decimal NUM_ORDER { set; get; }
        public string TREATMENT_CODE { get; set; }
        public string BIRTH_CERT_BOOK_ID { set; get; }
        public long PRINT_TIME { set; get; }
    }
}
