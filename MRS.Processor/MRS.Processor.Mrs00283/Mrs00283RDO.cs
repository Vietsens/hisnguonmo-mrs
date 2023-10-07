using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00283
{
    public class Mrs00283RDO
    {
        public string SUPPLIER { get; set; }
        public long SUPPLIER_ID { get; set; }
        public long IMP_TIME_NUMBER { get; set; }
        public string IMP_TIME { get; set; }
        public string IMP_TIME_STR { get; set; }
        public string DOCUMENT_DATE { get; set; }
        public string DOCUMENT_NUMBER { get; set; } //document_price
        public long IMP_MEST_ID { get; set; }
        public string IMP_MEST_CODE { get; set; }
        public string IMP_SOURCE_NAME { get; set; }
        public Decimal DOCUMENT_PRICE { get; set; }
        public Decimal TOTAL_PRICE { get; set; }//+VAT

        public string EXP_MEST_CODE { get; set; }
        public long MEDICINE_EXP_MEST_ID { get; set; }
        public long MATERIAL_EXP_MEST_ID { get; set; }

        public long MEDICAL_CONTRACT_ID { get; set; }
        public string MEDICAL_CONTRACT_CODE { get; set; }
        public string MEDICAL_CONTRACT_NAME { get; set; }

        public long DOCUMENT_SUPPLIER_ID { get; set; }
        public string DOCUMENT_SUPPLIER_CODE { get; set; }
        public string DOCUMENT_SUPPLIER_NAME { get; set; }

        public decimal AMOUNT { get; set; }

        public decimal CHMS_AMOUNT { get; set; }

        public decimal CABIN_TO_MST_AMOUNT { get; set; }

        public long IMP_STT_ID { get; set; }
        public decimal TOTAL_PRICE_IMP { get; set; }


        public decimal PRICE { get; set; }

        public decimal VAT { get; set; }

        public string IMP_MEST_TYPE_NAME { get; set; }

        public string MEDI_STOCK { get; set; }

        public string MEDI_MATE_CODE { get; set; }

        public string MEDI_MATE_NAME { get; set; }

        public string UNIT { get; set; }

        public long IMP_MEST_TYPE_ID { get; set; }

        public string EXPIRED_DATE_STR { get; set; }

        public string PACKAGE_NUMBER { get; set; }

        public string SERVICE_UNIT_NAME { get; set; }

        public string MANUFACTURER_NAME { get; set; }

        public string ACTIVE_INGR_BHYT_NAME { get; set; }

        public string CONCENTRA { get; set; }

        public string MEDI_MATE_TYPE_NAME { get; set; }

        public string MEDI_MATE_TYPE_CODE { get; set; }

        public decimal MEDI_MATE_AMOUNT { get; set; }

        public decimal MEDI_MATE_PRICE { get; set; }

        public decimal MEDI_MATE_TOTAL_PRICE { get; set; }

        public string PARENT_NAME { get; set; }

        public string PARENT_CODE { get; set; }

        public string PATIENT_NAME { get; set; }

        public string PATIENT_CODE { get; set; }

        public string EXP_MEST_STOCK_CODE { get; set; }
        public string EXP_MEST_STOCK_NAME { get; set; }

        public string IMP_MEST_TYPE_CODE { get; set; }


        public string AGGR_IMP_MEST_CODE { get; set; }

        public string AGGR_IMP_MEST_TYPE_NAME { get; set; }

        public Dictionary<string, decimal> DIC_PATIENT_CLASSIFY_AMOUNT { get; set; }

        public long PATIENT_CLASSIFY_ID { get; set; }

        public string PATIENT_CLASSIFY_CODE { get; set; }

        public string PATIENT_CLASSIFY_NAME { get; set; }

        public string TYPE { get; set; }

        public string STORAGE_CONDITION_NAME { get; set; }
    }
}
