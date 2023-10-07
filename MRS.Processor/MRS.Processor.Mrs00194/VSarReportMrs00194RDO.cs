using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00194
{
    public class VSarReportMrs00194RDO
    {
        public string PATIENT_CODE { get; set; }//mã bệnh nhân

        public string PATIENT_NAME { get; set; }//tên bệnh nhân

        public string DATE_OF_BIRTH { get; set; }//ngày sinh

        public string GENDER_NAME { get; set; }//giới tính

        public string HEIN_CARD_NUMBER { get; set; }//số thẻ BHYT

        public string HEIN_MEDI_ORG_CODE { get; set; }//mã khám chữa bệnh ban đầu

        public string IN_TIME { get; set; }//thời gian vào viện

        public string OUT_TIME { get; set; }//thời gian ra viện

        public decimal PRICE_EXAM { get; set; }//tiền khám

        public decimal PRICE_BED { get; set; }//tiền giường

        public decimal PRICE_TEST { get; set; }//xét nghiệm

        public decimal PRICE_DIIM { get; set; }//chuẩn đoán hình ảnh

        public decimal PRICE_FUEX { get; set; }//Thăm dò chức năng

        public decimal PRICE_SURG_AND_MISU { get; set; }//phẫu thuật, thủ thuật

        public decimal PRICE_MEDICINE { get; set; }//tiền thuốc

        public decimal PRICE_MATERIAL { get; set; }//tiền vật tư y tế

        public decimal PRICE_BLOOD { get; set; }//máu và chế phẩm máu

        public decimal PRICE_TRANSPORT { get; set; }//vận chuyển

        public decimal TOTAL_ALL { get; set; }//tổng tiền

        public decimal PRICE_EXPEND_MATERIAL { get; set; }//hao phí thuốc

        public decimal PRICE_EXPEND_MEDICINE { get; set; }//hao phí vật tư

        public decimal TOTAL_PRICE_EXPEND { get; set; }//tổng cộng hao phí
    }
}
