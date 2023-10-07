using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00338
{
    public class Mrs00338RDO
    {
        //Tổng số bệnh nhân xuất viện
        public decimal TOTAL_TREAT_OUT_AMOUNT { get; set; }//Số lượt nội trú và ngoại trú
        public decimal TOTAL_TREAT_PRICE { get; set; }//Tổng chi phí
        public decimal AVEGARE_TREAT_PRICE { get; set; }//Bình quân phiếu
        public decimal AVEGARE_TREAT_MEDI_PRICE { get; set; }// Bình quân thuốc
        public decimal TREAT_MEDI_PRICE_RATIO { get; set; }//Tỷ lệ thuốc
        public decimal TOTAL_TREAT_MEDI_PRICE { get; set; }

        public decimal TOTAL_TREAT_IN_OUT_AMOUNT { get; set; }//Số lượt nội trú
        public decimal TOTAL_TREAT_IN_PRICE { get; set; }
        public decimal TOTAL_TREAT_IN_MEDI_PRICE { get; set; }
        public decimal AVEGARE_TREAT_IN_PRICE { get; set; }
        public decimal AVEGARE_TREAT_IN_MEDI_PRICE { get; set; }
        public decimal TREAT_IN_MEDI_PRICE_RATIO { get; set; }

        public decimal TOTAL_TREAT_OUT_OUT_AMOUNT { get; set; }//Số lượt ngoại trú
        public decimal TOTAL_TREAT_OUT_PRICE { get; set; }
        public decimal TOTAL_TREAT_OUT_MEDI_PRICE { get; set; }
        public decimal AVEGARE_TREAT_OUT_PRICE { get; set; }
        public decimal AVEGARE_TREAT_OUT_MEDI_PRICE { get; set; }
        public decimal TREAT_OUT_MEDI_PRICE_RATIO { get; set; }

        //Tổng số lượt bệnh nhân ngoại trú
        public decimal TOTAL_EXAM_OUT_AMOUNT { get; set; }// Số lượt 
        public decimal TOTAL_EXAM_PRICE { get; set; }//Tổng chi phí
        public decimal AVEGARE_EXAM_PRICE { get; set; }//Bình quân phiếu
        public decimal AVEGARE_EXAM_MEDI_PRICE { get; set; }// Bình quân thuốc
        public decimal EXAM_MEDI_PRICE_RATIO { get; set; }//Tỷ lệ thuốc
        public decimal TOTAL_EXAM_MEDI_PRICE { get; set; }

        //Tổng số bệnh nhân đái tháo đường
        public decimal TOTAL_E11_OUT_AMOUNT { get; set; }//Số lượt
        public decimal TOTAL_E11_PRICE { get; set; }//Tổng chi phí
        public decimal AVEGARE_E11_PRICE { get; set; }//Bình quân phiếu
        public decimal AVEGARE_E11_MEDI_PRICE { get; set; }// Bình quân thuốc
        public decimal E11_MEDI_PRICE_RATIO { get; set; }//Tỷ lệ thuốc
        public decimal TOTAL_E11_MEDI_PRICE { get; set; }
        //public decimal ICD_CODE { get; set; }//Tổng số bệnh nhân đái tháo đường

        public decimal TOTAL_PRICE_END_DATE { get; set; }
        public decimal TOTAL_HEIN_PRICE_END_DATE { get; set; }//Tổng thu viện phí BHYT ngày  <#DATE_TO_STR;>
        public decimal AVEGARE_TOTAL_HEIN_PRICE { get; set; }//Bình quân thực hiện /01 ngày
        public decimal TOTAL_PATIENT_PRICE_END_DATE { get; set; }// Tổng thu viện phí trực tiếp ngày  <#DATE_TO_STR;>
        public decimal TOTAL_PATIENT_PRICE_BHYT_END_DATE { get; set; }// Thu đồng chi trả của BN BHYT ngày <#DATE_TO_STR;>
        public decimal TOTAL_PATIENT_PRICE_FEE_OF_BHYT_END_DATE { get; set; } //Thu DVKT viện phí của đối tượng BHYT ngày <#DATE_TO_STR;> ( ko gồm DVKT Liên doanh liên kết)
        public decimal TOTAL_PATIENT_PRICE_KSK_END_DATE { get; set; }//Thu bệnh nhân KSK ngày  <#DATE_TO_STR;>
        public decimal TOTAL_PATIENT_PRICE_KHAC_END_DATE { get; set; }//Thu khác ngày <#DATE_TO_STR;>
        public decimal TOTAL_PATIENT_PRICE_FEE_END_DATE { get; set; }//Thu bệnh nhân Viện phí ngày  <#DATE_TO_STR;>

        public decimal TOTAL_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }//Tổng thu viện phí BHYT từ ngày  <#DATE_FROM_STR;> đến <#DATE_TO_STR;>
        public decimal TOTAL_PATIENT_PRICE { get; set; }//Tổng thu viện phí trực tiếp  từ ngày  <#DATE_FROM_STR;> đến <#DATE_TO_STR;>
        public decimal TOTAL_PATIENT_PRICE_BHYT { get; set; }//Thu đồng chi trả của BN BHYT từ ngày  <#DATE_FROM_STR;> đến <#DATE_TO_STR;>
        public decimal TOTAL_PATIENT_PRICE_FEE_OF_BHYT { get; set; }//Thu DVKT viện phí của đối tượng BHYT từ ngày  <#DATE_FROM_STR;> đến <#DATE_TO_STR;> ( ko gồn DVKT Liên doanh liên kết)
        public decimal TOTAL_PATIENT_PRICE_KSK { get; set; }// Thu bệnh nhân KSK từ ngày  <#DATE_FROM_STR;> đến <#DATE_TO_STR;>
        public decimal TOTAL_PATIENT_PRICE_KHAC { get; set; }//Thu khác từ ngày  <#DATE_FROM_STR;> đến <#DATE_TO_STR;>
        public decimal TOTAL_PATIENT_PRICE_FEE { get; set; }// Thu bệnh nhân Viện phí từ ngày  <#DATE_FROM_STR;> đến <#DATE_TO_STR;>

        public decimal TOTAL_PRICE_XHH_END_DATE { get; set; }//Thu viện phí hoạt động LDLK ngày <#DATE_TO_STR;>
        public decimal TOTAL_HEIN_PRICE_XHH_END_DATE { get; set; }//Thu từ cơ quan BHXH chi trả cho người bệnh có thẻ BHYT:
        public decimal TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH_END_DATE { get; set; }//Thu DVKT LDLK viện phí của đối tượng BHYT từ ngày
        public decimal TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE { get; set; }//Thu trực tiếp từ người bệnh có thẻ BHYT:
        public decimal TOTAL_PATIENT_PRICE_FEE_XHH_END_DATE { get; set; }//Thu trực tiếp từ người bệnh không có thẻ BHYT

        public decimal TOTAL_PRICE_XHH { get; set; }//Thu viện phí hoạt động LDLK từ ngày   <#DATE_FROM_STR;> đến <#DATE_TO_STR;>:
        public decimal TOTAL_HEIN_PRICE_XHH { get; set; }//Thu từ cơ quan BHXH chi trả cho người bệnh có thẻ BHYT từ ngày   <#DATE_FROM_STR;> đến <#DATE_TO_STR;>:
        public decimal TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH { get; set; }//Thu DVKT LDLK viện phí của đối tượng BHYT từ ngày  <#DATE_FROM_STR;> đến <#DATE_TO_STR;>:
        public decimal TOTAL_PATIENT_PRICE_BHYT_XHH { get; set; }//Thu trực tiếp từ người bệnh có thẻ BHYT từ ngày   <#DATE_FROM_STR;> đến <#DATE_TO_STR;>:
        public decimal TOTAL_PATIENT_PRICE_FEE_XHH { get; set; }//Thu trực tiếp từ người bệnh không có thẻ BHYT từ ngày   <#DATE_FROM_STR;> đến <#DATE_TO_STR;>:

        public decimal TOTAL_PATIENT_PRICE_BHYT_END_DATE_EXAM { get; set; }// Thu đồng chi trả của BN BHYT ngày <#DATE_TO_STR;> đối tượng khám
        public decimal TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_IN { get; set; }// Thu đồng chi trả của BN BHYT ngày <#DATE_TO_STR;> đối tượng nội trú
        public decimal TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_OUT { get; set; }// Thu đồng chi trả của BN BHYT ngày <#DATE_TO_STR;> đối tượng ngoại trú

        public decimal TOTAL_PATIENT_PRICE_BHYT_EXAM { get; set; }//Thu đồng chi trả của BN BHYT từ ngày  <#DATE_FROM_STR;> đến <#DATE_TO_STR;> đối tượng khám
        public decimal TOTAL_PATIENT_PRICE_BHYT_TREAT_IN { get; set; }//Thu đồng chi trả của BN BHYT từ ngày  <#DATE_FROM_STR;> đến <#DATE_TO_STR;> đối tượng nội trú
        public decimal TOTAL_PATIENT_PRICE_BHYT_TREAT_OUT { get; set; }//Thu đồng chi trả của BN BHYT từ ngày  <#DATE_FROM_STR;> đến <#DATE_TO_STR;> đối tượng ngoại trú

        public decimal TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_EXAM { get; set; }//Thu trực tiếp từ người bệnh có thẻ BHYT: đối tượng khám
        public decimal TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_IN { get; set; }//Thu trực tiếp từ người bệnh có thẻ BHYT: đối tượng nội trú
        public decimal TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_OUT { get; set; }//Thu trực tiếp từ người bệnh có thẻ BHYT: đối tượng ngoại trú

        public decimal TOTAL_PATIENT_PRICE_BHYT_XHH_EXAM { get; set; }//Thu trực tiếp từ người bệnh có thẻ BHYT từ ngày   <#DATE_FROM_STR;> đến <#DATE_TO_STR;>: đối tượng khám
        public decimal TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_IN { get; set; }//Thu trực tiếp từ người bệnh có thẻ BHYT từ ngày   <#DATE_FROM_STR;> đến <#DATE_TO_STR;>: đối tượng nội trú
        public decimal TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_OUT { get; set; }//Thu trực tiếp từ người bệnh có thẻ BHYT từ ngày   <#DATE_FROM_STR;> đến <#DATE_TO_STR;>: đối tượng ngoại trú

        public string END_ROOM_NAME { get; set; }//Phòng ra viện

        public long ROOM_TYPE_ID { get; set; }//Phòng ra viện
    }
}
