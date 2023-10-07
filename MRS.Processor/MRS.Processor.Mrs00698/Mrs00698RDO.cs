using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00698
{
    class Mrs00698RDO : V_HIS_TREATMENT
    {
        public string BARCODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public long INTRUCTION_TIME { get; set; }
        public long START_TIME { get; set; }
        public long RESULT_TIME { get; set; }// thời gian tra kết quả
        public string RESULT_USERNAME { get; set; }// người trả kết quả
        public string RESULT_LOGINNAME { get; set; }
        public Dictionary<string, string> DIC_RESULT { get; set; }
    }

    class SampleLisResultADO : V_HIS_TEST_INDEX
    {
        public string VALUE_RANGE { get; set; }
        //public long? LEVEL { get; set; }
        public string SAMPLE_SERVICE_STT_CODE { get; set; }
        public long? SAMPLE_SERVICE_STT_ID { get; set; }
        public string SAMPLE_SERVICE_STT_NAME { get; set; }
        public long? SAMPLE_ID { get; set; }
        public long? LIS_RESULT_ID { get; set; }
        public long? SAMPLE_SERVICE_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal? MIN_VALUE { get; set; }
        public decimal? MAX_VALUE { get; set; }
        public string NOTE { get; set; }
        public decimal? VALUE { get; set; }
        public bool? IS_NORMAL { get; set; }
        public bool? IS_LOWER { get; set; }
        public bool? IS_HIGHER { get; set; }
        public short? IS_NO_EXECUTE { get; set; }
        public long SERVICE_NUM_ORDER { get; set; }
        public long HIS_SERVICE_NUM_ORDER { get; set; }
        public long? APPOINTMENT_TIME { get; set; }
        public string OLD_VALUE { get; set; }
        public long TREATMENT_ID { get; set; }
    }
    class InfoSampleResult
    {
        public string TREATMENT_CODE { get; set; }
        public string BARCODE { get; set; }
        public long SAMPLE_ID { get; set; }
        public string VALUE { get; set; }
        public string TEST_INDEX_CODE { get; set; }
        public string TEST_INDEX_NAME { get; set; }
        public long? SAMPLE_SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public long? SAMPLE_SERVICE_STT_ID { get; set; }
        public long? SERVICE_NUM_ORDER { get; set; }
        public string OLD_VALUE { get; set; }
        public string DESCRIPTION { get; set; }
        public long? APPOINTMENT_TIME { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public long INTRUCTION_TIME { get; set; }
        public long START_TIME { get; set; }
        public long RESULT_TIME { get; set; }// thời gian tra kết quả
        public string RESULT_USERNAME { get; set; }// người trả kết quả
        public string RESULT_LOGINNAME { get; set; }
    }
}
