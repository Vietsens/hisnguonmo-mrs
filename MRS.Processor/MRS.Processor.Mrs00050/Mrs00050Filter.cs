using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00050
{
    /// <summary>
    /// Bao cao chi tiet giao dich bi huy theo nguoi huy
    /// </summary>
    class Mrs00050Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? TRANSACTION_TYPE_ID { get; set; }
        public List<long> TRANSACTION_TYPE_IDs { get; set; }

        public Mrs00050Filter()
            : base()
        {
        }
    }
}
