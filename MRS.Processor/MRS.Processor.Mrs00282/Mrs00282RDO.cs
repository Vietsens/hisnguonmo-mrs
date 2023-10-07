using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00282
{
    public class Mrs00282RDO
    {
        public string MEDICINE_TYPE_CODE { get; set; }//MÃ LOẠI THUỐC
        public string MATERIAL_TYPE_CODE { get; set; }//MÃ LOẠI VẬT LIỆU
        public string BLOOD_TYPE_CODE { get; set; }//Mã loại máu
        
        public string MEDICINE_TYPE_NAME { get; set; }//tên loại thuốc
        public string MATERIAL_TYPE_NAME { get; set; }//tên loại vật liệu
        public string BLOOD_TYPE_NAME { get; set; }//Tên loại máu
        public string SERVICE_UNIT_NAME { get; set; }//TÊN ĐƠN VỊ DỊCH VỤ 
        public string PACKAGE_NUMBER { get; set; }//lô sản suất
        public string BID_NUMBER { get; set; }// Mã đấu thầu số
        public string EXPIRED_DATE { get; set; }//NGÀY HẾT HẠN
        public string MANUFACTURER_NAME { get; set; }//TÊN NHÀ SẢN XUẤT
        public string PACKING_TYPE_NAME { get; set; }//TÊN LOẠI ĐÓNG GÓI 
        public string NATIONAL_NAME { get; set; }//TÊN QUỐC GIA SX
        public string IMP_MEST_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { set; get; }/// <summary>
        /// tên hoạt chất
        /// </summary>
        public string IMP_TIME { get; set; }
        public string PRICE { get; set; }//Giá
        public Decimal IMP_PRICE { get; set; }
        public Decimal IMP_VAT { get; set; }
        public Decimal AMOUNT { get; set; }//số lượng
        public Decimal VOLUME { get; set; }//dung tích
        public string EXP_PRICE { get; set; }
        public Decimal TOTAL_PRICE { get; set; }//Tổng tiền
        public Decimal VIR_TOTAL_PRICE { get; set; }
        public string DELIVERER { get; set; }//NGƯỜI GIAO HÀNG 
        public string IMP_SOURCE_NAME { get; set; }//TÊN NGUỒN
        public Decimal VAT { get; set; }
        public string DOCUMENT_DATE { get; set; }//NGÀY CẤP chứng chỉ
        public string DOCUMENT_NUMBER { get; set; }//SỐ VĂN BẢN
        public long DOCUMENT_TIME { get; set; }
        public string SUPPLIER_NAME { get; set; }//Tên nhà cung cấp
        public string RECORDING_TRANSACTION { get; set; }//Bản ghi giao dịch
        public string REGISTER_NUMBER { get; set; }//số đăng ký
        public string CONCENTRA { get; set; }// nồng độ
        public long? TIME_IMP { get; set; }

        public decimal IMP_PRICE_BEFORE_VAT { get; set; }//giá đã tính thuế

        public long? DOCUMENT_PRICE { get; set; }//số tiền ghi trên giấy chứng từ nhập

        public Decimal? VIR_PRICE { get; set; }

        public string SUPPLIER_CODE { get; set; }//mã nhà cung cấp

        public string MEDI_CONTRACT_CODE { get; set; }
        public string MEDI_CONTRACT_NAME { get; set; }//tên hợp đồng
        public string CONTRAINDICATION { get; set; }//CHỐNG CHỈ ĐỊNH 
        public long PARENT_ID { get; set; }
        public string PARENT_CODE { get; set; }//Mã người giám hộ
        public string PARENT_NAME { get; set; }// tên người giám hộ
        public short IS_CHEMICAL_SUBSTANCE { get; set; }
        public short? IS_VACCINE { get; set; }
        public short? IS_FUNCTIONAL_FOOD { get; set; }

        public V_HIS_IMP_MEST_MEDICINE impMestMedicine { get; set; }

        public V_HIS_IMP_MEST_MATERIAL impMestMaterial { get; set; }

        public V_HIS_IMP_MEST_BLOOD impMestBlood { get; set; }
    }

}
