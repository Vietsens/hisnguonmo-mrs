using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00722
{
    class Mrs00722RDO
    {
        public string IN_TIME { get; set; }
        public string INTRUCTION_TIME { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_GENDER { get; set; }
        public string PATIENT_DOB { get; set; }
        public string PATIENT_ADDRESS { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string ICD_DIIM { get; set; }
        public string DIIM_RESULT { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal PRICE { get; set; }
    }
}
