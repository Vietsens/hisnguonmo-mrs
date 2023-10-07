
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MRS.Processor.Mrs00805
{
    public class Mrs00805RDO
    {
        public string DEPARTMENT_CODE { set; get; }
        public string DEPARTMENT_NAME { set; get; }
        public decimal THEORY_PATIENT_COUNT { set; get; }//giường kế hoạch
        public decimal REALITY_PATIENT_COUNT { set; get; }// giường thực kê
        public decimal COUNT_CBCA { get; set; }//1
        public decimal COUNT_CA_BHYT { get; set; }//2
        public decimal COUNT_BHYT_KHAC { get; set; }//3
        public decimal COUNT_VP_DAN { get; set; }//4
        public decimal COUNT_VP_CHINH_SACH { get; set; }//5
        public decimal COUNT_NGUOI_NUOC_NGOAI { get; set; }//6
        public decimal COUNT_PHAM_NHAN { get; set; }//7
        public decimal COUNT_CB_DUONG_CHUC { set; get; }//8
        public decimal COUNT_CB_HUU_TRI { set; get; }
        public decimal TREATMENT_DAY_COUNT{set;get;}
        public decimal COUNT_NOITRU { set; get; }
        public decimal COUNT_OLD { set; get; }
        public decimal COUNT_END { set; get; }
        public decimal COUNT_NEW { set; get; }
        public decimal COUNT_CV { set; get; }
        public long DAY { set; get; }
    }
}
