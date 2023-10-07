using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00131
{
    class Mrs00131ByTypeRDO
    {
        //So luong cap cuu doi tuong Quan (giay gioi thieu) + Chinh sach
        public int QuanChinhSach { get;  set;  }

        //So luong cap cuu doi tuong TQ (than nhan quan doi) + QD (quan doi)
        public int TqQd { get;  set;  }

        //So luong cap cuu doi tuong QH (quan huu) + BHYT khac
        public int QhBh { get;  set;  }

        //So luong cap cuu doi tuong Dich vu
        public int DichVu { get;  set;  }
    }
}
