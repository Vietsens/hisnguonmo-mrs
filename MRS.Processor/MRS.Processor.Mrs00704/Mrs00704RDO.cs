using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00704
{
    public class Mrs00704RDO
    {
        public List<Loai_PTTT_Model> List_Loai_PTTT_Model { get; set; }
        public List<DT_PCD_Model> List_DT_PCD_Model { get; set; }
        public List<DT_KCD_Model> List_DT_KCD_Model { get; set; }
    }

    public class Loai_PTTT_Model
    {
        public string LoaiPTTT { get; set; }
        public List<PTTT_Model> List_PTTT_Model { get; set; }
    }

    public class PTTT_Model
    {
        public string TenPTTT { get; set; }
        public decimal DoanhThu { get; set; }
        public decimal ChiPhi { get; set; }
        public decimal LoiNhuan { get; set; }
    }

    public class DT_PCD_Model
    {
        public string TenPhong { get; set; }
        public decimal TongCong { get; set; }
        public decimal BHYT { get; set; }
        public decimal ThuPhi { get; set; }
    }

    public class DT_KCD_Model
    {
        public string LoaiBaoCao { get; set; }
        public List<Khoa_DT_Model> List_Khoa_DT_Model { get; set; }
    }

    public class Khoa_DT_Model
    {
        public string TenKhoa { get; set; }
        public decimal DoanhThu { get; set; }
        public decimal ChiPhi { get; set; }
        public decimal LoiNhuan { get; set; }
    }
}
