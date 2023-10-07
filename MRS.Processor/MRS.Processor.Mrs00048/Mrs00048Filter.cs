using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00048
{
    /// <summary>
    /// Bao cao tong hop xu ly dich vu theo phong va trang thai thuc hien
    /// </summary>
    class Mrs00048Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00048Filter()
            : base()
        {
        }
    }
}
