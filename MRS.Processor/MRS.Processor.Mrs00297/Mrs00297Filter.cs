using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00297
{
    public class Mrs00297Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? BRANCH_ID { get;  set; }
        public long? MEDI_STOCK_ID { get; set; }
        public long? MEDI_STOCK_BUSINESS_ID { get; set; }

        public List<long> MEDI_STOCK_BUSINESS_IDs { get; set; }

        public List<long> MEDICINE_TYPE_IDs { get; set; }

        public List<long> MATERIAL_TYPE_IDs { get; set; }

        public string KEY_GROUP_EXP { get; set; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
    }
}
