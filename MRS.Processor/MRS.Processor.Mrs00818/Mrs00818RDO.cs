using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00818
{
    public class Mrs00818RDO
    {
        public long? TREATMENT_KH { get; set; }//khám
        public long? TREATMENT_KH_BHYT { set; get; }
        public long? TREATMENT_KH_YHCT { set; get; }
        public long? TREATMENT_KH_15 { set; get; }
        public long? TREATMENT_KH_FEMALE { set; get; }
        public long? TREATMENT_KH_VP { set; get; }

        public long? TREATMENT_NOITRU { get; set; }
        public long? TREATMENT_NOITRU_BHYT { set; get; }
        public long? TREATMENT_NOITRU_HYCT { set; get; }
        public long? TREATMENT_NOITRU_VP { set; get; }
        public long? TREATMENT_NOITRU_15 { set; get; }
        public long? TREATMENT_NOITRU_FEMALE { set; get; }
        public decimal? TREATMET_NOITRU_DAY_COUNT { get; set; }

        public long? TREATMENT_NGOAITRU { get; set; }
        public long? TREATMENT_NGOAITRU_BHYT { set; get; }
        public long? TREATMENT_NGOAITRU_YHCT { set; get; }
        public long? TREATMENT_NGOAITRU_VP { set; get; }

        public long? COUNT_NT { get; set; }
        public long?  COUNT_XN { get; set; }//Xét nghiệm
        public long? COUNT_SA { get; set; }//siêu âm
        public long? COUNT_XQ { set; get; }//x quang
        public long? COUNT_DT { get; set; }//điện tim
        public long? COUNT_DN { get; set; }//điện não
        public long? COUNT_NS { set; get; }// nội soi
        public long? COUNT_PTTT_3 { set; get; }//pttt loại 3 trở lên
        public long? COUNT_TT { set; get; }// thủ thuật
        public long? COUNT_DE { set; get; }//đẻ
        public long? COUNT_DE_THUONG { set; get; }//đẻ thường
        public long? COUNT_DE_MO{set;get;}//đẻ mổ
        public long? COUNT_DE_KHO{set;get;}//đẻ khó
        public long? TOTAL_MAU{set;get;}//tổng số máu 
        public long? CHUYENTUYEN{set;get;}// tỉ lệ chuyển tuyến
    }
}
