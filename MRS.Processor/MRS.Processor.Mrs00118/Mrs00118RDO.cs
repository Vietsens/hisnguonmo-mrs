using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00118
{
    class Mrs00118RDO
    {
        public string TEN_DANH_MUC { get;  set;  }
        public string BENH_NHAN_CU { get;  set;  }
        public int? BENH_NHAN_VAO { get;  set;  }
        public int? CHUYEN_BAN_DEN { get;  set;  }
        public int? CHUYEN_BAN_DI { get;  set;  }
        public int? BENH_NHAN_RA { get;  set;  }
        public int? CHUYEN_VIEN { get;  set;  }
        public string CON_LAI { get;  set;  }

        //public Mrs00118RDO(List<int?> list, string name)
        //{
        //    TEN_DANH_MUC = name; 
        //    BENH_NHAN_CU = list[0].ToString(); // != 0 ? list[0] : null; 
        //    BENH_NHAN_VAO = list[1] != 0 ? list[1] : null; 
        //    CHUYEN_BAN_DEN = list[2] != 0 ? list[2] : null; 
        //    BENH_NHAN_RA = list[3] != 0 ? list[3] : null; 
        //    CHUYEN_BAN_DI = list[4] != 0 ? list[4] : null; 
        //    CHUYEN_VIEN = list[5] != 0 ? list[5] : null; 
        //    CON_LAI = list[6].ToString(); // != 0 ? list[6] : null; 
        //}

        public Mrs00118RDO() { }
    }
}
