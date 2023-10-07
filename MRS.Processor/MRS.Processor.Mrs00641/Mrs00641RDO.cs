using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00641
{
   
    public class DepartmentCountRDO
    {
        public long DEPARTMENT_ID { get; set; }

        public long SERVICE_TYPE_ID { get; set; }

        public long SERVICE_ID { get; set; }

        public long PATIENT_TYPE_ID { get; set; }

        public decimal AMOUNT { get; set; }
    }
}
