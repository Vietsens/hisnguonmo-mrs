using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00700
{
    class Mrs00700RDO : MOS.EFMODEL.DataModels.V_HIS_TEST_INDEX
    {
        public Mrs00700RDO()
        {

        }

        public Mrs00700RDO(MOS.EFMODEL.DataModels.V_HIS_TEST_INDEX data)
        {
            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00700RDO>(this, data);
        }

        public long IS_PARENT { get; set; }
        public long HAS_ONE_CHILD { get; set; }
        public string VALUE_RANGE { get; set; }
        public long? LEVEL { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SAMPLE_SERVICE_STT_CODE { get; set; }
        public long? SAMPLE_SERVICE_STT_ID { get; set; }
        public string SAMPLE_SERVICE_STT_NAME { get; set; }
        public long? SAMPLE_ID { get; set; }
        public long? SAMPLE_STT_ID { get; set; }
        public long? LIS_RESULT_ID { get; set; }
        public long? SAMPLE_SERVICE_ID { get; set; }
        public short? Item_Edit_Value { get; set; }
        public long? MACHINE_ID { get; set; }
        public long? MACHINE_ID_OLD { get; set; }
        public string PARENT_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public string CHILD_ID { get; set; }
        public decimal? MIN_VALUE { get; set; }
        public decimal? MAX_VALUE { get; set; }
        public string NOTE { get; set; }
        public short? IS_RETURN_RESULT { get; set; }
        public decimal? VALUE { get; set; }
        public bool? IS_NORMAL { get; set; }
        public bool? IS_LOWER { get; set; }
        public bool? IS_HIGHER { get; set; }
        public short? IS_NO_EXECUTE { get; set; }
        public long? SERVICE_NUM_ORDER { get; set; }
        public decimal? APPOINTMENT_TIME { get; set; }
        public string OLD_VALUE { get; set; }
        public short? IS_RUN_AGAIN { get; set; }
        public short? IS_RUNNING { get; set; }
        public bool? RERUN { get; set; }
        public long HIS_SERVICE_NUM_ORDER { get; set; }
        public string HIS_SERVICE_CODE { get; set; }
        public string HIS_SERVICE_NAME { get; set; }

        public string BARCODE { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public string TDL_HEIN_MEDI_ORG_CODE { get; set; }
        public string TDL_HEIN_MEDI_ORG_NAME { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TDL_PATIENT_CAREER_NAME { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public short? TDL_PATIENT_IS_HAS_NOT_DAY_DOB { get; set; }
        public string TDL_PATIENT_MILITARY_RANK_NAME { get; set; }
        public string TDL_PATIENT_MOBILE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_PATIENT_NATIONAL_NAME { get; set; }
        public string TDL_PATIENT_PHONE { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public string TDL_PATIENT_WORK_PLACE { get; set; }
        public string TDL_PATIENT_WORK_PLACE_NAME { get; set; }
        public string TDL_TREATMENT_CODE { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public string TDL_TREATMENT_TYPE_NAME { get; set; }
        public string KSK_WORK_PLACE_NAME { get; set; }
        public string KSK_CONTRACT_CODE { get; set; }

        public string ICD_CODE { get; set; }

        public string ICD_NAME { get; set; }

        public string TDL_REQUEST_ROOM_NAME { get; set; }

        public string TDL_REQUEST_USERNAME { get; set; }

        public string TDL_EXECUTE_ROOM_NAME { get; set; }

        public int? FEMALE_DOB { get; set; }

        public int? MALE_DOB { get; set; }

        public long REQUEST_ROOM_ID { get; set; }

        public string TDL_REQUEST_LOGINNAME { get; set; }

        public long EXECUTE_ROOM_ID { get; set; }
    }
}
