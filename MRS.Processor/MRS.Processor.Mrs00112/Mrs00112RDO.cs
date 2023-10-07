using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00112
{
    public class Mrs00112RDO : HIS_SERE_SERV
    {
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_TEXT { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public long? KSK_CONTRACT_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string DOB_YEAR { get; set; }
        public string INTRUCTION_TIME_STR { get; set; }

        public decimal VIR_TOTAL_PRICE_1 { get; set; }

        public decimal VIR_TOTAL_PRICE_2 { get; set; }

        public decimal VIR_TOTAL_PRICE_3 { get; set; }

        public decimal VIR_TOTAL_PRICE_4 { get; set; }

        public decimal VIR_TOTAL_PRICE_5 { get; set; }

        public decimal VIR_TOTAL_PRICE_6 { get; set; }

        public decimal VIR_TOTAL_PRICE_7 { get; set; }

        public decimal VIR_TOTAL_PRICE_8 { get; set; }

        public decimal VIR_TOTAL_PRICE_9 { get; set; }

        public decimal VIR_TOTAL_PRICE_10 { get; set; }

        public decimal VIR_TOTAL_PRICE_11 { get; set; }

        public decimal VIR_TOTAL_PRICE_12 { get; set; }

        public decimal VIR_TOTAL_PRICE_13 { get; set; }

        public decimal VIR_TOTAL_PRICE_14 { get; set; }

        public decimal VIR_TOTAL_PRICE_15 { get; set; }

        public decimal VIR_TOTAL_PRICE_16 { get; set; }

        public decimal VIR_TOTAL_PRICE_17 { get; set; }

        public decimal VIR_TOTAL_PRICE_18 { get; set; }

        public decimal VIR_TOTAL_PRICE_19 { get; set; }

        public decimal VIR_TOTAL_PRICE_20 { get; set; }

        public decimal VIR_TOTAL_PRICE_21 { get; set; }

        public decimal VIR_TOTAL_PRICE_22 { get; set; }

        public decimal VIR_TOTAL_PRICE_23 { get; set; }

        public decimal VIR_TOTAL_PRICE_24 { get; set; }

        public decimal VIR_TOTAL_PRICE_25 { get; set; }

        public decimal VIR_TOTAL_PRICE_26 { get; set; }

        public decimal VIR_TOTAL_PRICE_27 { get; set; }

        public decimal VIR_TOTAL_PRICE_28 { get; set; }

        public decimal VIR_TOTAL_PRICE_29 { get; set; }

        public decimal VIR_TOTAL_PRICE_30 { get; set; }

        public decimal VIR_TOTAL_PRICE_31 { get; set; }

        public decimal VIR_TOTAL_PRICE_32 { get; set; }

        public decimal VIR_TOTAL_PRICE_33 { get; set; }

        public decimal VIR_TOTAL_PRICE_34 { get; set; }

        public decimal VIR_TOTAL_PRICE_35 { get; set; }

        public decimal VIR_TOTAL_PRICE_36 { get; set; }

        public decimal VIR_TOTAL_PRICE_37 { get; set; }

        public decimal VIR_TOTAL_PRICE_38 { get; set; }

        public decimal VIR_TOTAL_PRICE_39 { get; set; }

        public decimal VIR_TOTAL_PRICE_40 { get; set; }

        public decimal VIR_TOTAL_PRICE_41 { get; set; }

        public decimal VIR_TOTAL_PRICE_42 { get; set; }

        public decimal VIR_TOTAL_PRICE_43 { get; set; }

        public decimal VIR_TOTAL_PRICE_44 { get; set; }

        public decimal VIR_TOTAL_PRICE_45 { get; set; }

        public decimal VIR_TOTAL_PRICE_46 { get; set; }

        public decimal VIR_TOTAL_PRICE_47 { get; set; }

        public decimal VIR_TOTAL_PRICE_48 { get; set; }

        public decimal VIR_TOTAL_PRICE_49 { get; set; }

        public decimal VIR_TOTAL_PRICE_50 { get; set; }

        public Mrs00112RDO(HIS_SERE_SERV r)
        {
            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00112RDO>(this, r);
        }

        public Mrs00112RDO()
        {
            // TODO: Complete member initialization
        }
    }
    public class SERVICE_NAME
    {
        public decimal SERVICE_NAME_1 { get; set; }

        public decimal SERVICE_NAME_2 { get; set; }

        public decimal SERVICE_NAME_3 { get; set; }

        public decimal SERVICE_NAME_4 { get; set; }

        public decimal SERVICE_NAME_5 { get; set; }

        public decimal SERVICE_NAME_6 { get; set; }

        public decimal SERVICE_NAME_7 { get; set; }

        public decimal SERVICE_NAME_8 { get; set; }

        public decimal SERVICE_NAME_9 { get; set; }

        public decimal SERVICE_NAME_10 { get; set; }

        public decimal SERVICE_NAME_11 { get; set; }

        public decimal SERVICE_NAME_12 { get; set; }

        public decimal SERVICE_NAME_13 { get; set; }

        public decimal SERVICE_NAME_14 { get; set; }

        public decimal SERVICE_NAME_15 { get; set; }

        public decimal SERVICE_NAME_16 { get; set; }

        public decimal SERVICE_NAME_17 { get; set; }

        public decimal SERVICE_NAME_18 { get; set; }

        public decimal SERVICE_NAME_19 { get; set; }

        public decimal SERVICE_NAME_20 { get; set; }

        public decimal SERVICE_NAME_21 { get; set; }

        public decimal SERVICE_NAME_22 { get; set; }

        public decimal SERVICE_NAME_23 { get; set; }

        public decimal SERVICE_NAME_24 { get; set; }

        public decimal SERVICE_NAME_25 { get; set; }

        public decimal SERVICE_NAME_26 { get; set; }

        public decimal SERVICE_NAME_27 { get; set; }

        public decimal SERVICE_NAME_28 { get; set; }

        public decimal SERVICE_NAME_29 { get; set; }

        public decimal SERVICE_NAME_30 { get; set; }

        public decimal SERVICE_NAME_31 { get; set; }

        public decimal SERVICE_NAME_32 { get; set; }

        public decimal SERVICE_NAME_33 { get; set; }

        public decimal SERVICE_NAME_34 { get; set; }

        public decimal SERVICE_NAME_35 { get; set; }

        public decimal SERVICE_NAME_36 { get; set; }

        public decimal SERVICE_NAME_37 { get; set; }

        public decimal SERVICE_NAME_38 { get; set; }

        public decimal SERVICE_NAME_39 { get; set; }

        public decimal SERVICE_NAME_40 { get; set; }

        public decimal SERVICE_NAME_41 { get; set; }

        public decimal SERVICE_NAME_42 { get; set; }

        public decimal SERVICE_NAME_43 { get; set; }

        public decimal SERVICE_NAME_44 { get; set; }

        public decimal SERVICE_NAME_45 { get; set; }

        public decimal SERVICE_NAME_46 { get; set; }

        public decimal SERVICE_NAME_47 { get; set; }

        public decimal SERVICE_NAME_48 { get; set; }

        public decimal SERVICE_NAME_49 { get; set; }

        public decimal SERVICE_NAME_50 { get; set; }
    }

}
