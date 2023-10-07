using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00803
{
    public class Mrs00803RDO
    {
        public string MATERIAL_TYPE_NAME { set; get; }
        public string MATERIAL_TYPE_CODE { set; get; }
        public long SUPPLIER_ID { set; get; }
        public string SUPPLIER_CODE { set; get; }
        public string SUPPLIER_NAME { set; get; }
        public long MANUFACTURER_ID { set; get; }
        public string MANUFACTURER_NAME { set; get; }// tên nhà sản xuất
        public string MANUFACTURER_CODE { set; get; }
        public string SERVICE_UNIT_CODE { set; get; }
        public string SERVICE_UNIT_NAME { set; get; }//tên đơn vị
        public string PACKING_TYPE_NAME { set; get; }//quy cách đóng gói
        public string NATIONAL_NAME { set; get; }//tên quốc gia sản xuất
        public decimal IMP_PRICE { set; get; }//giá nhập
        public decimal PRICE { set; get; }//Giá
        public decimal TOTAL_PRICE { set; get; }
        public decimal AMOUNT { set; get; }//Số lượng
        public decimal? IN_AMOUNT { set; get; }//Số lượng nhập
        public string BID_NUMBER { get; set; }// Mã đấu thầu số
        public short? IS_REUSABLE { set; get; }//tái sử dung?1:0
        public string BID_NUM_ORDER { set; get; }//số thứ tự thầu
        public string REGISTER_NUMBER { get; set; }//số đăng ký(bộ y tế)
        public string DOCUMENT_DATE { get; set; }//NGÀY CẤP chứng chỉ
        public string DOCUMENT_NUMBER { get; set; }//SỐ VĂN BẢN
        public long DOCUMENT_TIME { get; set; }
        public string MEDICINE_TYPE_NAME { set; get; }
        public string MEDICINE_TYPE_CODE { set; get; }
        public long? MAX_REUSE_COUNT { set; get; }//số lần tối đa tái sư dụng
        public string PACKAGE_NUMBER { get; set; }//lô sản suất
        public string MATERIAL_GROUP_BHYT { set; get; }//tên nhóm vật tư
        public string HEIN_SERVICE_BHYT_CODE { set; get; }//mã danh ục dùng chung
        public string HEIN_SERVICE_BHYT_NAME { set; get; }//tên vật tư theo y tế
        public long? HEIN_SERVICE_TYPE_ID { set; get; }
        public string BID_GROUP_CODE { set; get; }//Mã nhóm thầu
        public string BID_PACKAGE_CODE { set; get; }//Mã gói thầu
        public string BID_MATERIAL_TYPE_CODE { set; get; }//mã trúng thầu
        public string BID_MATERIAL_TYPE_NAME { set; get; }//tên trúng thầu
        public string JOIN_BID_MATERIAL_TYPE_CODE { set; get; }//mã dự thầu
        public long MATERIAL_TYPE_MAP_ID { set; get; }//vật tư ánh xạ
        public string CONCENTRA { set; get; }//hàm lượng
        public decimal? TDL_CONTRACT_AMOUNT { set; get; }

    }
}
