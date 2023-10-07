using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00283
{
    public class Mrs00283Filter
    {
        public long? MEDI_STOCK_ID { get;  set;  }
        public List<long> MEDI_STOCK_IDs { get;  set;  } //kho nhập
        public List<long> EXP_MEDI_STOCK_IDs { get; set; } //kho xuất
        public List<long> IMP_SOURCE_IDs { get;  set;  } //nguồn nhập
        public long? IMP_SOURCE_ID { get;  set;  }

        public List<long> IMP_MEST_TYPE_IDs { get; set; } //loại nhập (chọn nhiều)
        public long? IMP_MEST_TYPE_ID { get; set; } //Loại nhập
        public List<long> DEPARTMENT_IDs { get; set; } //khoa
        public List<long> EXAM_ROOM_IDs { get; set; } //phòng khám

        public List<long> PATIENT_TYPE_IDs { get; set; } //đối tượng bệnh nhân

        public List<long> PATIENT_CLASSIFY_IDs { get; set; } //đối tượng chi tiết

        public long? SUPPLIER_ID { get;  set;  } //nhà cung cấp
        public List<long> SUPPLIER_IDs { set; get; }
        public long? TIME_FROM { get;  set;  } 
        public long? TIME_TO { get;  set;  }
        public List<string> REQ_LOGINNAMEs { get; set; } //Người yêu cầu
        public string DOCUMENT_NUMBER { get; set; } 
        public bool? IS_GROUP_BY_MEDICINE { get; set; }
        public bool? IS_GROUP_BY_IMP_MEST_CODE { get; set; }


        public long? MATERIAL_TYPE_ID { get; set; } //loại vật tư

        public long? MEDICINE_TYPE_ID { get; set; } //loại thuốc
        public List<long> MEDICINE_TYPE_IDs { get; set; } //loại thuốc

        public List<long> IMP_MEST_STT_IDs { get; set; } //tình trạng nhập

        public List<long> MATERIAL_TYPE_IDs { get; set; }
    }
}
