using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00235
{
    public class Mrs00235Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? MEDI_STOCK_ID { get; set; } //kho xuất
        public bool? IS_DETAIL { get; set; } 
        public List<long> EXP_MEST_TYPE_IDs { get; set; } //lý do xuất
        public List<long> DEPARTMENT_IDs { get; set; } //xuat cho khoa
        public List<long> REQ_LOGINNAMEs { get; set; } //bac sy yeu cau
        public bool? IS_NOI_NGOAI_TRU { get; set; } //phan loai noi tru/ngoai tru
        public List<long> PATIENT_TYPE_IDs { get; set; } //doi tuong
        public bool? ORDER_BY { get; set; } //thu tu sap xep
        public string PATIENT_NAMEs { get; set; }
        public List<long> PATIENT_CLASSIFY_IDs { set; get; }// chi tiết đối tượng
        public bool? IS_HPKP { set; get; }
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }// đối tượng thanh toán
        public List<long> EXP_MEST_REASON_IDs { get; set; }
        public bool? IS_GROUP_IMP_PRICE { get; set; }

        public bool? IS_EXP_DATE { get; set; } // khi tích vào sẽ lọc theo ngày thực xuất. nếu không thì vẫn lọc theo ngày hoàn thành phiếu như cũ                                
    }
}
