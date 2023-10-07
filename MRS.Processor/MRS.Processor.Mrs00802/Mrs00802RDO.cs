using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00802
{
    public class Mrs00802RDO
    {
        public string ACTIVE_INGR_BHYT_CODE { set; get; }
        public string ACTIVE_INGR_BHYT_NAME { set; get; }
        public string BYT_NUM_ORDER { get; set; }
        public decimal AMOUNT { get; set; }//5 số lượng
        public string CONCENTRA { get; set; }
        public long? MEDICINE_GROUP_ID { get; set; }
        public short? IS_STAR_MARK { get; set; }
        public long? MANUFACTURER_ID { get; set; }
        public long? MEDICINE_LINE_ID { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public long? NUM_ORDER { get; set; }
        public string PACKING_TYPE_NAME { get; set; }
        public string REGISTER_NUMBER { get; set; }
        public long SERVICE_ID { get; set; }
        public string MANUFACTURER_CODE { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public string SERVICE_UNIT_CODE { get; set; }
        public long SERVICE_UNIT_ID { set; get; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string TCY_NUM_ORDER { get; set; }
        public string TOTAL_PRICE { set; get; }//7 tổng tiền
        public string SUPPLIER_CODE { set; get; }
        public string SUPPLIER_NAME { set; get; }// tên công ty
        public string IMP_TIME { get; set; }
        public decimal? PRICE { get; set; }//Giá
        public decimal IMP_VAT { get; set; }
        public decimal VOLUME { get; set; }//dung tích
        public string MEDI_CONTRACT_NAME { get; set; }//tên hợp đồng
        public long ANTICIPATE_ID { get; set; }
        public long MEDICINE_TYPE_ID { get; set; }
        public long? SUPPLIER_ID { get; set; }
        List<HIS_SUPPLIER> listSupplier { set; get; }
        public string MEDICINE_GROUP_NAME { set; get; }
        public string MEDICINE_GROUP_CODE { set; get; }
    }
}
