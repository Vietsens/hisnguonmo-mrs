using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Proccessor.Mrs00496
{
    public class Mrs00496RDO
    {
        public string ACTIVE_INGR_BHYT_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public string BYT_NUM_ORDER { get; set; }
        public string HEIN_ORDER { get; set; }
        public string HEIN_SERVICE_BHYT_CODE { get; set; }
        public string HEIN_SERVICE_BHYT_NAME { get; set; }
        public string MEDICINE_BYT_NUM_ORDER { get; set; }
        public string MEDICINE_REGISTER_NUMBER { get; set; }
        public string MEDICINE_TCY_NUM_ORDER { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string TCY_NUM_ORDER { get; set; }
        public string TDL_BID_GROUP_CODE { get; set; }
        public string TDL_BID_NUM_ORDER { get; set; }
        public string TDL_BID_NUMBER { get; set; }
        public string TDL_BID_PACKAGE_CODE { get; set; }
        public string TDL_BID_YEAR { get; set; }
        public decimal VIR_IMP_PRICE { get; set; }

        public string TYPE { get; set; }
        public long MEDI_MATE_ID { get; set; }
        public long MATERIAL_TYPE_ID { get; set; }
        public string MATERIAL_TYPE_CODE { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }

        public bool? IS_MEDICINE { get; set; }
        public long MEDICINE_TYPE_ID { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }

        public long BLOOD_TYPE_ID { get; set; }
        public string BLOOD_TYPE_CODE { get; set; }
        public string BLOOD_TYPE_NAME { get; set; }

        public string SERVICE_UNIT_NAME { get; set; }
        public string REGISTER_NUMBER { get; set; }
        public string CONCENTRA { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public string BID_NUMBER { get; set; }
        public string BID_GROUP_CODE { get; set; }
        public string BID_NAME { get; set; }
        public string BID_NUM_ORDER { get; set; }
        public string BID_YEAR { get; set; }

        public string PACKAGE_NUMBER { get; set; }

        public decimal TOTAL_PRICE { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal PRICE { get; set; }
        public decimal IMP_VAT { get; set; }
        public decimal PRICE_AFTER_VAT { get; set; }
        public string EXPIRED_DATE_STR { get; set; }
        public decimal ENABLE_AMOUNT { get; set; }
        public decimal BEAN_AMOUNT { get; set; }

        public decimal? SALE_PRICE { get; set; }
        public long? MEDICINE_LINE_ID { get; set; }
        public string MEDICINE_LINE_CODE { get; set; }
        public string MEDICINE_LINE_NAME { get; set; }
        public long? MEDICINE_GROUP_ID { get; set; }
        public string MEDICINE_GROUP_CODE { get; set; }
        public string MEDICINE_GROUP_NAME { get; set; }
        public long PARENT_MEDICINE_TYPE_ID { get; set; }
        public string PARENT_MEDICINE_TYPE_CODE { get; set; }
        public string PARENT_MEDICINE_TYPE_NAME { get; set; }

        public string RECORDING_TRANSACTION { get; set; }

        public string PACKING_TYPE_NAME { get; set; }
        public decimal? ALERT_MAX_IN_STOCK { get; set; }

        //cac truong sap xep nhom
        public long PARENT_NUM_ORDER { get; set; }
        public long LINE_NUM_ORDER { get; set; }

        public Mrs00496RDO()
        {

        }

        

        public decimal IMP_AMOUNT { get; set; }

        public decimal IMP_TOTAL_PRICE { get; set; }

        public decimal EXP_AMOUNT { get; set; }

        public decimal EXP_TOTAL_PRICE { get; set; }

        public decimal SALE_VAT_RATIO { get; set; }
    }
}
