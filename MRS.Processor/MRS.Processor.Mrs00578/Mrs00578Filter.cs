using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00578
{
    public class Mrs00578Filter
    {
        public long TRANSACTION_TIME_FROM { get; set; }
        public long TRANSACTION_TIME_TO { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public bool? IS_TRANSACTION_TIME_OR_INTRUCTION_TIME { get; set; } //Lọc theo: null: thời gian giao dịch, true: thời gian giao dịch, false: thời gian chỉ định
    }
}