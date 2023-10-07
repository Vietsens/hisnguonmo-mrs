using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00726
{
    class Mrs00726RDO
    {
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string ROOM_CODE { get; set; }
        public string ROOM_NAME { get; set; }
        public int SO_LUOT_KHAM { get; set; }
        public int SO_LUOT_DA_KHAM { get; set; }
        public int KHAM_CHINH_BHYT { get; set; }
        public int KHAM_PHU_BHYT { get; set; }
        public int KHAM_CHINH_DV { get; set; }
        public int KHAM_PHU_DV { get; set; }
        public int NHAP_VIEN { get; set; }
        public int BN_DIA_PHUONG { get; set; } 
        public int BN_CAC_TINH { get; set; }
        public int DANGKY_BHYT { get; set; }
        public int DANGKY_DV { get; set; }
    }
}
