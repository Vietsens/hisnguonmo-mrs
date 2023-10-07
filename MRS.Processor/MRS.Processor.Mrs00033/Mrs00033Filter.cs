using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00033
{
    /// <summary>
    /// Bao cao tong hop xu ly dich vu theo khoa thuc hien
    /// </summary>
    class Mrs00033Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public bool? HAS_NOT_BILL { get; set; }
        public bool? HAS_NOT_DONE { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }

        public Mrs00033Filter()
            : base()
        {
        }
    }
}
