using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00422
{
    public class Mrs00422Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public List<string> EXECUTE_LOGINNAME_DOCTORs { get; set; }
        public List<string> REQUEST_LOGINNAME_DOCTORs { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }
        public List<long> SERVICE_REQ_STT_IDs { get; set; }
        public long? SERVICE_REQ_STT_ID { get; set; }
        public bool? IS_FINISH_TIME { get; set; }
        public bool? IS_PAYED { get; set; }
        public bool? IS_DEPO_SERVICE { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
    }
}
