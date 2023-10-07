using Inventec.Common.Repository;
using MOS.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Inventec.Common.Logging;

namespace MRS.Processor.Mrs00207
{
    public class Mrs00207RDO : V_HIS_EXP_MEST_MEDICINE
    {
        public long ROW_POS { get; set; }
        public Decimal AMOUNT_TRUST { get; set; }
        public long? PATIENT_ID { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string TDL_TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_PHONE { get; set; }
        
        public string TDL_PATIENT_CODE { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_GENDER_CODE { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_TEXT { get; set; }
        public string EXP_TIME_STR { get; set; }
        public Decimal TOTAL_PRICE { get; set; }
        public decimal? TREATMENT_DAY_COUNT { get; set; }
        public long? INTRUCTION_TIME { get; set; }
        public long? INTRUCTION_DATE { get; set; }
        public long? EXP_DATE { get; set; }
        public long? IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TREATMENT_TYPE_CODE { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
        public string TDL_PATIENT_TYPE_CODE { get; set; }
        public string TDL_PATIENT_TYPE_NAME { get; set; }
        public string REQUEST_DEPARTMENT_BHYT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_ROOM_CODE { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string BED_CODE { get; set; }
        public string BED_NAME { get; set; }
        public string MEDI_STOCK_CODE { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public string TREATMENT_END_TYPE_CODE { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
        public string EXP_MEST_TYPE_CODE { get; set; }
        public string EXP_MEST_TYPE_NAME { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string HEAD_CARD { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public decimal VIR_TOTAL_HEIN_PRICE { get; set; }
        public string TDL_BID_PACKAGE_CODE { get; set; }
        public string AGGR_EXP_MEST_CODE { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public long? EXPIRED_DATE { get; set; }
        


        public string SERVICE_CODE { set; get; }
        public string SERVICE_NAME { set; get; }
        public string SS_SERVICE_NAME { set; get; }
        public string GROUP_PARENT_MEDICINE_CODE { set; get; }
        public string GROUP_PARENT_MEDICINE_NAME { set; get; }

        public string PARENT_MEDICINE_TYPE_NAME { get; set; }
        public string PARENT_MEDICINE_TYPE_CODE { get; set; }

        public string MEDICINE_TYPE_NAME_KS { set; get; }
        public string MEDICINE_TYPE_NAME_DT { set; get; }
        public string MEDICINE_TYPE_NAME_CO { set; get; }
        public string MEDICINE_TYPE_NAME_VTM { set; get; }
        public string MEDICINE_TYPE_NAME_TIEM { set; get; }
        public decimal COUNT_MEDICINE_TYPE_KS { set; get; }
        public decimal COUNT_MEDICINE_TYPE_DT { set; get; }
        public decimal COUNT_MEDICINE_TYPE_CO { set; get; }
        public decimal COUNT_MEDICINE_TYPE_VTM { set; get; }
        public decimal COUNT_MEDICINE_TYPE_TIEM { set; get; }
        public decimal COUNT_MEDICINE_TYPE_NAME_G1 { set; get; }
        public decimal COUNT_MEDICINE_TYPE_NAME_G2 { set; get; }
        public decimal COUNT_MEDICINE_TYPE_NAME_G3 { set; get; }
        public decimal COUNT_TOTAL_MEDICINE_TYPE { set; get; }
        public decimal AMOUNT_TOTAL_MEDICINE_TYPE { set; get; }
        public decimal COUNT_TOTAL_EXP_MEST { set; get; }

        public Dictionary<string, string> DIC_CATE_METY_NAME { get; set; }
        public Dictionary<string, decimal> DIC_CATE_METY_COUNT { get; set; }
        public Dictionary<string, decimal> DIC_CATE_METY_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_CATE_METY_PRICE { get; set; }

        public Dictionary<string, string> DIC_GR_METY_NAME { get; set; }
        public Dictionary<string, decimal> DIC_GR_METY_COUNT { get; set; }
        public Dictionary<string, decimal> DIC_GR_METY_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_GR_METY_PRICE { get; set; }

        public Dictionary<string, string> DIC_BID_PK_METY_NAME { get; set; }
        public Dictionary<string, decimal> DIC_BID_PK_METY_COUNT { get; set; }
        public Dictionary<string, decimal> DIC_BID_PK_METY_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_BID_PK_METY_PRICE { get; set; }

        public Dictionary<string, string> DIC_UF_METY_NAME { get; set; }
        public Dictionary<string, decimal> DIC_UF_METY_COUNT { get; set; }
        public Dictionary<string, decimal> DIC_UF_METY_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_UF_METY_PRICE { get; set; }

        public Dictionary<string, string> DIC_PAR_METY_NAME { get; set; }
        public Dictionary<string, decimal> DIC_PAR_METY_COUNT { get; set; }
        public Dictionary<string, decimal> DIC_PAR_METY_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_PAR_METY_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_MONTH_DAY_AMOUNT { get; set; }

        public string TYPE { get; set; }

        public Dictionary<string, decimal> DIC_CATE_METY_PRES { get; set; }

        public Dictionary<string, decimal> DIC_GR_METY_PRES { get; set; }

        public Dictionary<string, decimal> DIC_BID_PK_METY_PRES { get; set; }

        public Dictionary<string, decimal> DIC_UF_METY_PRES { get; set; }

        public Dictionary<string, decimal> DIC_PAR_METY_PRES { get; set; }

        public long MEDICINE_LINE_ID { get; set; }

        public string MEDICINE_LINE_CODE { get; set; }

        public string MEDICINE_LINE_NAME { get; set; }

        public Dictionary<string, decimal> DIC_SV_AMOUNT { get; set; }
    }
    public class AMOUNT_OF_EXP
    {
        public string EXP_MEST_CODE { get; set; }
        public long COUNT_MEDICINE_TYPE { get; set; }
        public Dictionary<string, decimal> DIC_BID_PACKAGE_CODE { get; set; }
        public Dictionary<string, decimal> DIC_MEDICINE_GROUP_CODE { get; set; }
        public Dictionary<string, decimal> DIC_PARENT_METY_CODE { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_CODE { get; set; }
        public Dictionary<string, decimal> DIC_PARENT_MEDICINE { set; get; }
        
    }
}
