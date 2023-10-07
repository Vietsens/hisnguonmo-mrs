using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00053
{
    /// <summary>
    /// Bao cao chi tiet doanh thu cua thu ngan theo phieu thu thanh toan
    /// </summary>
    class Mrs00053Filter
    {
        /// <summary>
        /// Tai khoan thu ngan bat buoc
        /// </summary>
        public string CASHIER_LOGINNAME { get; set; }
        public string LOGINNAME { get; set; }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
    }
}
