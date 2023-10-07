using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00312
{
    public class Mrs00312Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? MEDI_STOCK_ID { get; set; }

        public string IMS_LIMIT_CODE { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> REVERT_MEDI_STOCK_IDs { get; set; }
        public long? IMP_MEDI_STOCK_ID { get; set; }

        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }
        public bool? IS_CHEMICAL_SUBSTANCE { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { get; set; }
        public List<long> REQ_ROOM_IDs { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        public bool? MERGE_PRICE { get; set; }//1,2
        public bool? MERGE_PACKAGE { get; set; }//3
        public bool? MERGE_EXPIRED { get; set; }//4
        public bool? TRUE_FALSE { get; set; }//4
        public bool? TIME_TRUE_FALSE { get; set; }//4
        public List<long> EXACT_BED_ROOM_IDs { get; set; }//4
        //public bool? CABIN_IS_OUTSIDE { get; set; }//

        public string OMS_LIMIT_CODE { get; set; }
        //Kho ngoai vien: cac kho nay se khong tinh nhap xuat ton, khi xuat cho cac kho nay thi tinh la xuat ra khoi vien, khi nhap tu kho nay ve thi tinh la nhap vao vien
        public List<long> OUTSIDE_MEDI_STOCK_IDs { get; set; }

        //Khong lay xuat noi bo: khong lay loai xuat chuyen kho, bu co so, bu le tru truong hop xuat cho kho ngoai vien
        public bool? IS_NOT_LOCAL_EXP { get; set; }//
        public List<long> IMP_SOURCE_IDs { get; set; }//4

        public string TEST_PACKAGE_NUMBERs { get; set; }//tách thuốc, vật tư theo lô
        public List<long> PLACE_DEPARTMENT_IDs { get; set; }//Khoa sử dụng
        public List<long> PLACE_ROOM_IDs { get; set; }//Phòng sử dụng
        public string EXP_MEST_REASON_CODE__KN { get; set; }//lý do xuất kiểm nghiệm
        public string EXP_MEST_REASON_CODE__BGD { get; set; }//lý do xuất BGD
        public string EXP_MEST_REASON_CODE__TL { get; set; }//lý do xuất thanh lý
        public string EXP_MEST_REASON_CODE__TNCC { get; set; }//lý do xuất trả nhà cung cấp
        public string MEDI_STOCK_CODE__TTKHs { get; set; }//các tủ trực khoa
        public string MEDI_STOCK_CODE__TTPKs { get; set; }//các tủ trực phòng khám
        public string MEDI_STOCK_CODE__KHOLEs { get; set; }//kho lẻ
        public string DEPARTMENT_CODE__KKBs { get; set; }//Khoa khám bệnh
        public string DEPARTMENT_CODE__EXAMs { get; set; }//Khoa chứa các phòng khám
        public List<long> MEDICINE_TYPE_IDs { get; set; } //Loại thuốc

        public List<long> BRANCH_IDs { get; set; } // chi nhánh

        public List<long> EXP_MEST_REASON_IDs { get; set; }

    }
}
