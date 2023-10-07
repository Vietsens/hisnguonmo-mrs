using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00012
{
    /// <summary>
    /// Bao cao tong hop doanh thu theo doi tuong thanh toan doi voi ho so da duyet khoa
    /// </summary>
    class Mrs00012Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set; }

        public List<long> ACCOUNT_BOOK_IDs { get; set; }

        public List<long> PAY_FORM_IDs { get; set; }
        
        public string CASHIER_LOGINNAME { get; set; }

        public Mrs00012Filter()
            : base()
        {
        }
    }
}
