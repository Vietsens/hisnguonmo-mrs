using Inventec.Common.Repository;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using MRS.Processor.Mrs00105;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00105
{
    public class Mrs00105RDO : MRS.Processor.Mrs00105.IMP_EXP_MEST_TYPE
    {
        const long THUOC = 1;
        const long VATTU = 2;
        const long CHMS_TYPE_BSCS = 1;
        const long CHMS_TYPE_HTCS = 2;
        public long MEDI_STOCK_ID { get; set; }
        public string MEDI_STOCK_CODE { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public long MEDI_MATE_TYPE_ID { get; set; }
        public long TYPE { get; set; }
        public long MEDI_MATE_ID { get; set; }
        public string PACKAGE_NUMBER { get; set; }//Số lô
        public long? EXPIRED_DATE { get; set; }//Hạn dùng
        public string EXPIRED_DATE_STR { get; set; }//Hạn dùng
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string HEIN_SERVICE_TYPE_CODE { get; set; }
        public string HEIN_SERVICE_TYPE_NAME { get; set; }
        public string HEIN_SERVICE_CODE { get; set; }
        public string HEIN_SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public decimal IMP_TOTAL_AMOUNT { get; set; }
        public decimal IMP_NCC_AMOUNT { get; set; }
        public decimal IMP_HT_AMOUNT { get; set; }
        public decimal EXP_TOTAL_AMOUNT { get; set; }
        public decimal LOCAL_EXP_TOTAL_AMOUNT { get; set; } // so luong nhap xuat noi bo cac kho trong thoi gian bao cao
        public decimal BHYT_EXP_TOTAL_AMOUNT { get; set; } //so luong xuat bao hiem
        public decimal VP_EXP_TOTAL_AMOUNT { get; set; } //so luong xuat dich vu
        public decimal BHYT_IMP_TOTAL_AMOUNT { get; set; } //so luong nhap bao hiem
        public decimal VP_IMP_TOTAL_AMOUNT { get; set; } //so luong nhap dich vu
        public decimal BHYT_DTTTL_TOTAL_AMOUNT { get; set; } //so luong don tu truc tra lai bao hiem
        public decimal VP_DTTTL_TOTAL_AMOUNT { get; set; } //so luong don tu truc tra lai dich vu
        public decimal KNT_EXP_TOTAL_AMOUNT { get; set; } //so luong xuat kho nội trú
        public decimal BSCS_AMOUNT { get; set; }//số lượng xuất bổ sung cơ số tủ trực (kho chính bổ sung cho tủ trực)
        public decimal HTCS_AMOUNT { get; set; }//số lượng nhập hoàn trả cơ số tủ trực (kho chính nhận hoàn trả từ tủ trực)
        public decimal CABIN_BSCS_AMOUNT { get; set; }//số lượng nhập bổ sung cơ số tủ trực (tủ trực nhập bổ sung cơ số từ kho chính)
        public decimal CABIN_HTCS_AMOUNT { get; set; }//số lượng xuất hoàn trả cơ số tủ trực (tủ trực xuất hoàn trả cơ số về kho chính)
        public decimal BEGIN_AMOUNT { get; set; }
        public decimal PRICE { get; set; }
        public decimal? VAT_RATIO { get; set; }
        public string VAT_RATIO_STR { get; set; }
        public decimal END_AMOUNT { get; set; }
        public decimal END_PERIOD_AMOUNT { get; set; }
        public decimal IMP_PRICE { get; set; }
        public string CONCENTRA { get; set; }//Hàm lượng
        public string MEDICINE_TYPE_PROPRIETARY_NAME { get; set; }//Biệt dược
        public string NATIONAL_NAME { get; set; }//Quốc gia
        public string MANUFACTURER_NAME { get; set; }//Hãng sx
        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }//Nhà cung cấp
        public string SUPPLIER_ADDRESS { get; set; }//d/c Nhà cung cấp
        public string ACTIVE_INGR_BHYT_CODE { get; set; }//Mã Hoạt chất
        public string ACTIVE_INGR_BHYT_NAME { get; set; }//Tên hoạt chất
        public string PACKING_TYPE_NAME { get; set; }//Quy cách đóng gói
        public string REGISTER_NUMBER { get; set; }//Số đăng ký
        public string BID_GROUP_CODE { get; set; }//Mã nhóm thầu
        public string BID_NUM_ORDER { get; set; }//Số thứ tự thầu
        public string MEDICINE_USE_FORM_CODE { get; set; }//Mã đường dùng
        public string MEDICINE_USE_FORM_NAME { get; set; }//tên đường dùng

        public decimal BEGIN_TOTAL_PRICE { get; set; }
        public decimal EXP_TOTAL_PRICE { get; set; }
        public decimal IMP_TOTAL_PRICE { get; set; }
        public decimal END_TOTAL_PRICE { get; set; }

        public string EXP_MEDI_STOCK_CODE { get; set; }//Mã kho xuất
        public string IMP_MEDI_STOCK_CODE { get; set; }//Mã kho nhập
        public decimal INPUT_END_AMOUNT { get; set; }//Số lượng thực tế khi kiểm kê

        public string BYT_NUM_ORDER { get; set; }//STT theo TT40 (Bo y te)
        public string TCY_NUM_ORDER { get; set; }//STT theo TT31-12
        public string QUALITY_STANDARDS { get; set; }//TCCL
        public short SOURCE_MEDICINE { get; set; }//Nguồn gốc

        public string TDL_BID_NUMBER { get; set; }

        public long BID_ID { get; set; }

        public short? IS_CABINET { get; set; }
        public string RECORDING_TRANSACTION { get; set; }

        public string DEPARTMENT_NAME { get; set; }

        public string PREVIOUS_MEDI_STOCK_CODE { get; set; }//Mã kho trước chuyển sang

        public string PREVIOUS_MEDI_STOCK_NAME { get; set; }//Tên kho trước chuyển sang

        public Dictionary<string, decimal> DIC_EXP_REQ_DEPARTMENT { get; set; }

        public Dictionary<string, decimal> DIC_EXP_MEST_REASON { get; set; }

        public List<PreviousMediStockAmount> PreviousMediStockImpAmounts { get; set; }//số lượng đã nhập từ các kho cha

        public List<PreviousMediStockAmount> PreviousMediStockExpAmounts { get; set; }//số lượng đã xuất trả kho cha

        //bổ sung tiền bán và giá bán trong kỳ báo cáo

        public decimal SALE_TOTAL_PRICE { get; set; }

        public decimal SALE_PRICE { get; set; }

        public decimal VACCIN_PRICE { get; set; }

        public decimal VACCIN_VAT_RATIO { get; set; }

        public decimal SALE_VAT_RATIO { get; set; }

        public long? EXP_MEST_TYPE_ID { get; set; }

        public long? REQ_DEPARTMENT_ID { get; set; }

        public long? EXP_MEST_REASON_ID { get; set; }

        public long? IMP_MEST_TYPE_ID { get; set; }
        public string SCIENTIFIC_NAME { get; set; } // tên khoa học
        public string PREPROCESSING { get; set; } // sơ chế
        public string PROCESSING { get; set; } //phức chế
        public string USED_PART { get; set; }// bộ phận dùng điều chế
        public string DOSAGE_FORM { get; set; } // dạng bào chế
        public string DISTRIBUTED_SL { get; set; } // số lượng

        public Dictionary<string, decimal> DIC_CHMS_MEDI_STOCK { get; set; }

        public string JSON_CHMS_MEDI_STOCK { get; set; }

        public Dictionary<string, decimal> DIC_IMP_MEDI_STOCK { get; set; }

        public string JSON_IMP_MEDI_STOCK { get; set; }

        public Dictionary<string, decimal> DIC_CHMS_REQ_DEPARTMENT { get; set; }

        public string JSON_CHMS_REQ_DEPARTMENT { get; set; }

        public Dictionary<string, decimal> DIC_IMP_REQ_DEPARTMENT { get; set; }

        public string JSON_IMP_REQ_DEPARTMENT { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? IMP_TIME { get; set; }

        public Dictionary<string, decimal> DIC_MEDI_STOCK { get; set; }

        public Mrs00105RDO()
        {
            // TODO: Complete member initialization
        }

        public long? MEDICINE_LINE_ID { get; set; }

        public string MEDICINE_LINE_CODE { get; set; }

        public string MEDICINE_LINE_NAME { get; set; }

        public long? MEDICINE_GROUP_ID { get; set; }

        public long PARENT_MEDICINE_TYPE_NUM { get; set; }

        public long PARENT_MEDICINE_TYPE_ID { get; set; }

        public string PARENT_MEDICINE_TYPE_CODE { get; set; }

        public string PARENT_MEDICINE_TYPE_NAME { get; set; }

        public string MEDICINE_GROUP_CODE { get; set; }

        public string MEDICINE_GROUP_NAME { get; set; }

        public long? PREVIOUS_MEDI_STOCK_ID { get; set; }

        public decimal ANTICIPATE_AMOUNT { get; set; }

        public string SUPPLIER_PHONE { get; set; }

        public long? CHMS_IMP_TIME { get; set; }

        public string BID_NAME { get; set; }

        public string IMP_SOURCE_NAME { get; set; }

        public string IMP_SOURCE_CODE { get; set; }

        public long? IMP_SOURCE_ID { get; set; }

        public long? VACCINE_TYPE_ID { get; set; }

        public short? IS_CHEMICAL_SUBSTANCE { get; set; }

        //public short? IS_NT_NGT_CLS { get; set; }

        public string ATC_CODES { get; set; }

        public string BID_YEAR { get; set; }

        public string BID_PACKAGE_CODE { get; set; }

        public short? IS_SALE_EQUAL_IMP_PRICE { get; set; }
        public string BID_AM { get;  set; }

        public long? TREATMENT_TYPE_ID { get; set; }

        public string REQ_DEPARTMENT_CODE { get; set; }

        public string REQ_DEPARTMENT_NAME { get; set; }

        public long IS_NT_NGT_CLS { get; set; }

        public decimal IMP_PRICE_NEW { get; set; }
        public Dictionary<string,decimal> DIC_MEDI_STOCK_BEGIN_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_MEDI_STOCK_END_AMOUNT { get; set; }
        public decimal BHYT_EXP_TOTAL_AMOUNT_NGT { get; set; }
        public decimal BHYT_EXP_TOTAL_AMOUNT_NT { get; set; }
        public decimal BHYT_EXP_TOTAL_AMOUNT_NGOAI_TRU { get; set; }
        public decimal BHYT_EXP_TOTAL_AMOUNT_NOI_TRU { get; set; }
        public decimal VP_EXP_TOTAL_AMOUNT_NGOAI_TRU { get; set; }
        public decimal VP_EXP_TOTAL_AMOUNT_NOI_TRU { get; set; }
        public string MEDICINE_TYPE_NATIONAL { get; set; }
        public string BID_TYPE { get; set; }
        public decimal EXP_NGOAI_TRU_AMOUNT { get; set; }
        public decimal EXP_NOI_TRU_AMOUNT { get; set; }

        public long EXP_MEDI_STOCK_ID { get; set; }

        public string EXP_MEDI_STOCK_NAME { get; set; }

        public string BLOOD_ABO_CODE { get; set; }

        public string LAST_EXP_TIME { get; set; }

        public long? LAST_EXP_TIME_NUM { get; set; }

        public long? DOCUMENT_DATE { get; set; }

        public string DOCUMENT_NUMBER { get; set; }

        public string PATIENT_TYPE_NAME { get; set; }

        public decimal IMP_NCC_NEW_AMOUNT { get; set; }

        public string PACKAGE_NUMBER_1 { get; set; }

        public long? VALID_FROM_TIME { get; set; }

        public long? VALID_TO_TIME { get; set; }

        public decimal BID_INCREATE_AMOUNT { get; set; }

        public decimal BID_ADJUST_AMOUNT { get; set; }

        public decimal BID_DECREATE_AMOUNT { get; set; }

        public string BID_SUPPLIER_PHONE { get; set; }

        public string BID_SUPPLIER_ADDRESS { get; set; }

        public string BID_SUPPLIER_NAME { get; set; }

        public string BID_SUPPLIER_CODE { get; set; }

        public decimal DIFF_DOCUMENT_TOTAL_PRICE { get; set; }

        public short? IS_REUSABLING { get; set; }

        public string KEY_ORDER { get; set; }

        public string KEY_GROUP_BID_DETAIL { get; set; }

        public string KEY_GROUP_BID { get; set; }

        public short? IS_VACCINE { get; set; }
    }

    public class DETAIL
    {
        public string EXP_MEST_CODE { get; set; }
        public string IMP_MEST_CODE { get; set; }
        public string EXP_MEDI_STOCK_CODE { get; set; }
        public string IMP_MEDI_STOCK_CODE { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string CONCENTRA { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal IMP_PRICE_AFTER { get; set; }
        public decimal AMOUNT { get; set; }
    }

    public class ExpMestIdReason
    {
        public long EXP_MEST_ID { get; set; }
        public long? CHMS_TYPE_ID { get; set; }
        public string EXP_MEST_REASON_CODE { get; set; }
    }


    public class ImpMestIdChmsMediStockId
    {
        public long IMP_MEST_ID { get; set; }
        public long? CHMS_MEDI_STOCK_ID { get; set; }
    }


    public class MediMateIdChmsMediStockId
    {
        public short? IS_ON { get; set; }
        public long MAX_IMP_TIME { get; set; }
        public long TYPE_ID { get; set; }
        public long MEDI_MATE_ID { get; set; }
        public long MEDI_STOCK_ID { get; set; }
        public long? CHMS_MEDI_STOCK_ID { get; set; }
    }

    public class AnticipateMety
    {
        public long MEDICINE_TYPE_ID { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public decimal? AMOUNT { get; set; }
        public decimal? IMP_PRICE { get; set; }
        public decimal? IMP_VAT_RATIO { get; set; }
    }

    public class AnticipateMaty
    {
        public long MATERIAL_TYPE_ID { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public decimal? AMOUNT { get; set; }
        public decimal? IMP_PRICE { get; set; }
        public decimal? IMP_VAT_RATIO { get; set; }
    }

    public class PreviousMediStockAmount
    {
        public long PREVIOUS_MEDI_STOCK_ID { get; set; }
        public decimal AMOUNT { get; set; }
    }


    public class MediStockMetyMaty
    {
        public long TYPE { get; set; }
        public long MEDI_MATE_TYPE_ID { get; set; }
        public long MEDI_STOCK_ID { get; set; }
        public long? EXP_MEDI_STOCK_ID { get; set; }
    }

    public class ServiceMetyMaty
    {
        public string SERVICE_NAME { get; set; }
        public long TYPE { get; set; }
        public long MEDI_MATE_TYPE_ID { get; set; }
        public decimal QUOTA_AMOUNT { get; set; }
    }

    public class IMP_MEST
    {
        public string IMP_MEST_CODE { get; set; }
        public long? DOCUMENT_DATE { get; set; }
        public string DOCUMENT_NUMBER { get; set; }
    }

    public class BID_IMP
    {
        public string KEY_BID_IMP { get; set; }
        public decimal? BID_AMOUNT { get; set; }
        public decimal? BID_IMP_AMOUNT { get; set; }
        public decimal? BID_ADJUST_AMOUNT { get; set; }
    }


    public class MEDI_STOCK_CODE_IMPs
    {
        public string MEDI_STOCK_CODE { get; set; }
    }

    public class MATERIAL_REUSABLING
    {
        public long MATERIAL_ID { get; set; }
        public short IS_REUSABLING { get; set; }
    }
}
