using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00202
{
    class Mrs00202RDO
    {
        public MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL HEIN_APPROVAL { get; set; }

        public long HEIN_APPROVAL_ID { get; set; }

        public long PATIENT_ID { get; set; }
        public long? DOB_MALE { get; set; }
        public long? DOB_FEMALE { get; set; }
        public long TOTAL_DATE { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }

        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string MEDIORG_NAME { get; set; }
        public string ICD_CODE_MAIN { get; set; }
        public string OPEN_TIME { get; set; }
        public string CLOSE_TIME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string HEIN_TREATMENT_TYPE_CODE { get; set; }

        public decimal TOTAL_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal SURGMISU_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal SERVICE_PRICE_RATIO { get; set; }
        public decimal MEDICINE_PRICE_RATIO { get; set; }
        public decimal MATERIAL_PRICE_RATIO { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }

        public int RIGHT_ROUTE_ID { get; set; }
        public string RIGHT_ROUTE_NAME { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00202RDO()
        {

        }

        public Mrs00202RDO(MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL heinApproval)
        {
            this.HEIN_APPROVAL = heinApproval;
            this.HEIN_APPROVAL_ID = heinApproval.ID;
            this.TREATMENT_CODE = heinApproval.TREATMENT_CODE;
            this.PATIENT_CODE = heinApproval.TDL_PATIENT_CODE;
        }
    }
}
