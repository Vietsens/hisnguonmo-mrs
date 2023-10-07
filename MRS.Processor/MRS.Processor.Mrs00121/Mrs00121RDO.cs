using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00121
{
    class Mrs00121RDO
    {
        public string TEN_DOI_TUONG { get;  set;  }
        public int? SO_CU { get;  set;  }
        public int? DON_VI_DEN { get;  set;  }
        public int? GIA_DINH_DEN { get;  set;  }
        public int? CHUYEN_VIEN_DEN { get;  set;  }
        public int? CHUYEN_KHOA_DEN { get;  set;  }
        public int? TONG_BENH_NHAN_TANG { get;  set;  }
        public int? KHOI_BENH { get;  set;  }
        public int? CHUYEN_VIEN { get;  set;  }
        public int? CHUYEN_KHOA { get;  set;  }
        public int? NANG_XIN_VE { get;  set;  }
        public int? TU_VONG { get;  set;  }
        public int? LY_DO_KHAC { get;  set;  }
        public int? TONG_BENH_NHAN_GIAM { get;  set;  }
        public int? BENH_NHAN_CON_LAI { get;  set;  }

        public Mrs00121RDO() { }

        public Mrs00121RDO(List<int?> listTotal, string tenDoiDuong)
        {
            TEN_DOI_TUONG = tenDoiDuong; 
            //SO_CU = listTotal[0] != 0 ? listTotal[0] : null; 
            //DON_VI_DEN = listTotal[1] != 0 ? listTotal[1] : null; 
            //GIA_DINH_DEN = listTotal[2] != 0 ? listTotal[2] : null; 
            //CHUYEN_VIEN_DEN = listTotal[3] != 0 ? listTotal[3] : null; 
            //CHUYEN_KHOA_DEN = listTotal[4] != 0 ? listTotal[4] : null; 
            //TONG_BENH_NHAN_TANG = listTotal[5] != 0 ? listTotal[5] : null; 
            //KHOI_BENH = listTotal[6] != 0 ? listTotal[6] : null; 
            //CHUYEN_VIEN = listTotal[7] != 0 ? listTotal[7] : null; 
            //CHUYEN_KHOA = listTotal[8] != 0 ? listTotal[8] : null; 
            //NANG_XIN_VE = listTotal[9] != 0 ? listTotal[9] : null; 
            //TU_VONG = listTotal[10] != 0 ? listTotal[10] : null; 
            //LY_DO_KHAC = listTotal[11] != 0 ? listTotal[11] : null; 
            //TONG_BENH_NHAN_GIAM = listTotal[12] != 0 ? listTotal[12] : null; 
            //BENH_NHAN_CON_LAI = listTotal[13] != 0 ? listTotal[13] : null; 
            SO_CU = listTotal[0]; 
            DON_VI_DEN = listTotal[1]; 
            GIA_DINH_DEN = listTotal[2]; 
            CHUYEN_VIEN_DEN = listTotal[3]; 
            CHUYEN_KHOA_DEN = listTotal[4]; 
            TONG_BENH_NHAN_TANG = listTotal[5]; 
            KHOI_BENH = listTotal[6]; 
            CHUYEN_VIEN = listTotal[7]; 
            CHUYEN_KHOA = listTotal[8]; 
            NANG_XIN_VE = listTotal[9]; 
            TU_VONG = listTotal[10]; 
            LY_DO_KHAC = listTotal[11]; 
            TONG_BENH_NHAN_GIAM = listTotal[12]; 
            BENH_NHAN_CON_LAI = listTotal[13]; 
        }
    }
}
