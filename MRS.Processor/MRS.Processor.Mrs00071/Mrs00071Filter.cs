using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00071
{
    /// <summary>
    /// Báo cáo chi tiết sử dụng quyển sổ thu chi
    /// </summary>
    class Mrs00071Filter
    {

        public long? TRANSACTION_TIME_FROM { get; set; }
        public long? TRANSACTION_TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }


        public long? ACCOUNT_BOOK_ID { get; set; }
        public string LOGINNAME { get; set; }
        public string CASHIER_LOGINNAME { get; set; }

        public Mrs00071Filter()
            : base()
        {
        }
    }
}
