using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00041
{
    public class Mrs00041RDO
    {
        public string PATIENT_NAME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public long SERE_SERV_ID { get; set; }
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_PHONE { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public string PATIENT_CODE { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }//
        public string PATIENT_TYPE_NAME { get; set; }//đối tượng thanh toán
        public string TDL_PATIENT_TYPE_CODE { get; set; }//
        public string TDL_PATIENT_TYPE_NAME { get; set; }//đối tượng bệnh nhân
        public decimal? MALE_AGE { get; set; }//sau
        public decimal? FEMALE_AGE { get; set; }//sau
        public string VIR_ADDRESS { get; set; }
        public decimal? VIR_PRICE { get; set; }
        public string IS_BHYT { get; set; }//sau
        public string ICD_TEST { get; set; }
        public long REQUEST_ROOM_ID { get; set; }
        public string REQUEST_ROOM { get; set; }//sau
        public long REQUEST_DEPARTMENT_ID { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string PAR_SERVICE_CODE { get; set; }
        public string PAR_SERVICE_NAME { get; set; }
        public string TDL_HEIN_SERVICE_BHYT_NAME { get; set; }
        public string TDL_HEIN_SERVICE_BHYT_CODE { get; set; }
        public long SERVICE_ID { get; set; }
        public string TIME_TEST_STR { get; set; }//sau
        public decimal? AMOUNT { get; set; }
       
        
        public long? RESULT_TIME { get; set; }
        public string INTRUCTION_TIME_STR { get; set; }//sau
        public string TEST_RESULT { get; set; }//sau
        public long? EXECUTE_ROOM_ID { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }//sau
        public string EXECUTE_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string FEMALE_YEAR { get; set; }//sau
        public string MALE_YEAR { get; set; }//sau
        public string BARCODE { get; set; }
        public long? EXECUTE_TIME { get; set; }
        public string EXECUTE_TIME_STR { get; set; }
        //public long? MACHINE_ID { get; set; }
        public string EXECUTE_MACHINE_NAME { get; set; }
        public string EXECUTE_MACHINE_CODE { get; set; }
        public string MACHINE_NAME { get; set; }
        public string MACHINE_CODE { get; set; }
       // public long? BEGIN_TIME { get; set; }
       // public long? END_TIME { get; set; }
        public long? START_TIME { get; set; }
        public string START_TIME_STR { get; set; }
        public long? FINISH_TIME { get; set; }
        public string FINISH_TIME_STR { get; set; }


        public long? TDL_INTRUCTION_TIME { get; set; }

        public long? TDL_INTRUCTION_DATE { get; set; }

        public string TDL_PATIENT_YEAR { get; set; }

        public string MEDI_ORG_CODE { get; set; }

        public string PATIENT_TYPE_NAME_01 { get; set; }
        public string PATIENT_TYPE_NAME_2 { get; set; }
        public string BLOCK { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public string TREATMENT_TYPE_CODE { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
        public Dictionary<string,string> DIC_TEST_RESULT { get; set; }

        public string DOB_YEAR { get; set; }

        public string TDL_PATIENT_GENDER_NAME { get; set; }

        public long? IS_EXPEND { get; set; }

        public string IS_EXPEND_SERVICE { get; set; }

        public string NOTE { get; set; }

        public string LEAVEN { get; set; }

        public string SAMPLE_LOGINNAME { get; set; }

        public string SAMPLE_USERNAME { get; set; }

        public string SAMPLE_NOTE { get; set; }

        public long? SAMPLE_TIME { get; set; }
    }
    public class SSE
    {
        public long? ID { get; set; }
        public long? SERE_SERV_ID { get; set; }
        public long? TEST_INDEX_ID { get; set; }
        public long? MACHINE_ID { get; set; }
        public string VALUE { get; set; }
        public string DESCRIPTION { get; set; }
        public string CONCLUDE { get; set; }
        public string NOTE { get; set; }
        public string LEAVEN { get; set; }
    }

    public class ParamThread
    {
        public List<Mrs00041RDO> sereServSub { get; set; }
        public List<SSE> sseSub { get; set; }
        public List<Mrs00041RDO> listRdoSub { get; set; }
    }
}
