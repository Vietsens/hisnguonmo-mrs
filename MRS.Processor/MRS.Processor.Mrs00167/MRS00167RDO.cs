using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00167
{
    public class MRS00167RDO
    {
        public long PATIENT_TYPE_ID { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public long TREATMENT_ID { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string  TREATMENT_CODE { get; set; }
        public long PATIENT_ID { get; set; }
        public short? IS_EXPEND { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_CODE { get; set; }
        public long IN_TIME { get; set; }
        public long OUT_TIME { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public long ROOM_ID { get; set; }
        public string ROOM_CODE { get; set; }
        public string ROOM_NAME { get; set; }
        public long EXECUTTE_DEPARTMENT_ID { get; set; }
        public string EXECUTE_DEPARTMENT_CODE { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public decimal KH_PRICE { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal XN_PRICE { get; set; }
        public decimal DIMM_PRICE { get; set; }
        public decimal PTTT_PRICE { set; get; }
        public decimal KHAC_PRICE { get; set; }
        public decimal DISCOUNT_PRICE { set; get; }
        public decimal COUNT_SS { get; set; }
        public decimal AMOUNT { get; set; }

        public decimal MEDICINE_PRICE_HP { get; set; }
        public decimal MATERIAL_PRICE_HP { get; set; }
        public decimal XN_PRICE_HP { get; set; }
        public decimal DIMM_PRICE_HP { get; set; }
        public decimal DEPARTMENT_PRICE_HP { get; set; }
        public decimal BLOOD_PRICE_HP { get; set; }
        public decimal PTTT_PRICE_HP { get; set; }
        public decimal KHAC_PRICE_HP { get; set; }
        public decimal EXAM_PRICE_HP { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_CODE { get; set; }

        public decimal MATERIAL_PRICE_OUT_PACK { get; set; }

        public decimal MATERIAL_PRICE_IN_PACK { get; set; }
    }
    public class S_HIS_SERE_SERV
    {

        public long IS_DELETE { get; set; }

        public long PATIENT_TYPE_ID { get; set; }

        public short? IS_EXPEND { get; set; }

        public long? PARENT_ID { get; set; }

        public long ID { get; set; }

        public string TDL_SERVICE_NAME { get; set; }

        public string TDL_SERVICE_CODE { get; set; }

        public long? TDL_REQUEST_ROOM_ID { get; set; }

        public long? TDL_EXECUTE_ROOM_ID { get; set; }

        public long? TDL_EXECUTE_DEPARTMENT_ID { get; set; }

        public long? TDL_REQUEST_DEPARTMENT_ID { get; set; }

        public long? TDL_SERVICE_TYPE_ID { get; set; }

        public long TDL_TREATMENT_ID { get; set; }

        public decimal? AMOUNT { get; set; }

        public decimal? VIR_PRICE { get; set; }

        public decimal? VIR_PRICE_NO_EXPEND { get; set; }

        public long? TDL_HEIN_SERVICE_TYPE_ID { get; set; }

        public long? SERVICE_ID { get; set; }

        public string TDL_PATIENT_NAME { get; set; }

        public long PATIENT_ID { get; set; }

        public string TDL_PATIENT_CODE { get; set; }

        public string TREATMENT_CODE { get; set; }

        public long IN_TIME { get; set; }

        public long? OUT_TIME { get; set; }
    }
}
