using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00131
{
    class Mrs00131ByTimeRDO
    {
        public long EmergencyWtimeId { get;  set;  }

        //Thoi gian dau (thoi gian dau truoc khi den vien cap cuu)
        public string EmergencyWtimeName { get;  set;  }

        //So luong cap cuu doi tuong Quan (giay gioi thieu) + Chinh sach
        public int QuanChinhSach_Noi { get;  set;  }//o khoa Noi
        public int QuanChinhSach_Ngoai { get;  set;  }//o khoa ngoai
        public int QuanChinhSach_CK { get;  set;  }//o cac chuyen khoa

        //So luong cap cuu doi tuong TQ (than nhan quan doi) + QD (quan doi)
        public int TqQd_Noi { get;  set;  }//o khoa Noi
        public int TqQd_Ngoai { get;  set;  }//o khoa ngoai
        public int TqQd_CK { get;  set;  }//o cac chuyen khoa

        //So luong cap cuu doi tuong QH (quan huu) + BHYT khac
        public int QhBh_Noi { get;  set;  }//o khoa Noi
        public int QhBh_Ngoai { get;  set;  }//o khoa ngoai
        public int QhBh_CK { get;  set;  }//o cac chuyen khoa

        //So luong cap cuu doi tuong Dich vu
        public int DichVu_Noi { get;  set;  }//o khoa Noi
        public int DichVu_Ngoai { get;  set;  }//o khoa ngoai
        public int DichVu_CK { get;  set;  }//o cac chuyen khoa
    }
}
