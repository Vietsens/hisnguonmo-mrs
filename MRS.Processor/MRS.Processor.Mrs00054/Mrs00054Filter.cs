using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00054
{
    /// <summary>
    /// Bao cao chi tiet doanh thu cua thu ngan theo gia tri gia tang
    /// </summary>
    class Mrs00054Filter
    {
        /// <summary>
        /// Tai khoan thu ngan bat buoc
        /// </summary>
        public string CASHIER_LOGINNAME { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00054Filter()
            : base()
        {
        }
    }
}
