using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00618
{
    public class Mrs00618Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get; set; }
        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get; set; }
        /// </summary>
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public List<string> LOGINNAME_DOCTORs { get; set; }

        public List<long> BRANCH_IDs { get; set; }
        public long? BRANCH_ID { get; set; }

        public short? INPUT_DATA_ID_TIME_TYPE { get; set; } // Loại thời gian: 1 - Khóa viện phí; 2 - Ra viện; 3 - Vào viện;
        public Mrs00618Filter()
            : base()
        {

        }
    }
}
