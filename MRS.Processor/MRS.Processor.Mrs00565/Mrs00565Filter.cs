using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00565
{
    public class Mrs00565Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        public bool? IS_EXPEND { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
    }
}
