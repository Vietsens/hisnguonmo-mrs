using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00719
{
    public class Mrs00719RDO
    {
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string MACHINE_CODE { get; set; }
        public string MACHINE_NAME { get; set; }
        public int MACHINE_AMOUNT { get; set; }
        public long MACHINE_PRICE { get; set; }
        public long? START_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public string PATIENT_TYPE { get; set; }
        public Dictionary<string, int> DIC_PATIENT_TYPE { get; set; }
    }
}
