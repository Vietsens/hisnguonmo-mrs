using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00396
{
    public class V_HIS_SERE_SERV
    {

        public decimal AMOUNT { get; set; }
        public long ID { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public decimal PRICE { get; set; }
        public long SERVICE_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        
        public long TDL_REQUEST_DEPARTMENT_ID { get; set; }
       
        public long? TDL_TREATMENT_ID { get; set; }
        
    }
}
