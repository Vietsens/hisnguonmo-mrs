using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;

namespace MRS.Processor.Mrs00565
{
    class Mrs00565RDO:IMP_EXP_MEST_TYPE
    {
        public string MEDICINE_TYPE_NAME { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string CONCENTRA { get; set; }
        public string UNIT { get; set; }
        public decimal PRICE { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal TT_PRICE { get; set; }
        public string TYPE { get; set; }
        public decimal AMOUTN_TH { set; get; }
        public decimal AMOUNT_TH { set; get; }
        public decimal AMOUNT_TL { set; get; }
        public decimal AMOUNT_BH { set; get; }
        public decimal AMOUNT_VP { set; get; }
        public decimal AMOUNT_HP { set; get; }
        public decimal AMOUNT_TRALAI_HP { set; get; }
        public decimal AMOUNT_TRALAI_VP { set; get; }
        public decimal AMOUNT_TRALAI_BH { set; get; }

        public string PATIENT_TYPE_ID { get; set; }


    }
}
