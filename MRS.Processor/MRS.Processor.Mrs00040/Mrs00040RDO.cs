using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00040
{
    public class Mrs00040RDO : HIS_SERE_SERV
    {
        public string SERVICE_REQ_CODE { get; set; }
        public string PTTT_GROUP_NAME { get; set; }
        public string PATIENT_NAME { get; set; }
        public string TDL_PATIENT_FIRST_NAME { get; set; }
        public string PATIENT_CODE { get; set; }
        public long? TDL_FINISH_TIME { get; set; }
        public long? START_TIME { get; set; }
        public string VIR_ADDRESS { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public string PATIENT_FIRST_NAME { get; set; }
        public string BEFORE_MISU { get; set; }
        public string AFTER_MISU { get; set; }
        public string MISU_PPPT { get; set; }
        public string MISU_PPVC { get; set; }
        public string TIME_MISU_STR { get; set; }
        public string MISU_TYPE_NAME { get; set; }
        public string DEFAULT_MISU_TYPE_NAME { get; set; }
        public string NOTE { get; set; }
        public string IS_BHYT { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string FEMALE_YEAR { get; set; }
        public string MALE_YEAR { get; set; }
        public string SERVICE_NAME { get; set; }
        public decimal? MALE_AGE { get; set; }
        public decimal? FEMALE_AGE { get; set; }
        public string SERVICE_REQ_ICD_NAME { get; set; }
        public string SERVICE_REQ_ICD_CODE { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public long finish_Time { get; set; }
        public string FINISH_TIME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public Dictionary<string, string> DICR_EXECUTE_USERNAME { get; set; }
        public Dictionary<string, string> DICR_EXECUTE_USERNAME_REAL { get; set; }
        public HIS_TRANSACTION HIS_TRANSACTION { get; set; }
        public string SURG_PPVC_2 { get; set; }

        public string PTTT_PRIORITY_NAME { get; set; }

        public string PTTT_TABLE_NAME { get; set; }

        public string EMOTIONLESS_RESULT_NAME { get; set; }

        public string BEGIN_TIME_STR { get; set; }

        public string END_TIME_STR { get; set; }

        public long? BEGIN_TIME { get; set; }

        public long? END_TIME { get; set; }
        public string TDL_EXECUTE_DEPARTMENT_NAME { get; set; }
        public string TDL_REQUEST_DEPARTMENT_NAME { get; set; }
        public string TDL_HEIN_MEDI_ORG_CODE { get; set; }
        public string PATIENT_TYPE_NAME_01 { get; set; }
        public string PATIENT_TYPE_NAME_02 { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string TREA_ICD_NAME { get; set; }
        public string TREA_ICD_CODE { get; set; }
        public string TREA_SUB_NAME { get; set; }
        public string TREA_SUB_CODE { get; set; }
        public Mrs00040RDO(Mrs00040RDO data, HIS_TRANSACTION HIS_TRANSACTION)
        {
            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00040RDO>(this, data);
            this.HIS_TRANSACTION = HIS_TRANSACTION;
            if (this.DICR_EXECUTE_USERNAME == null)
            {
                this.DICR_EXECUTE_USERNAME = new Dictionary<string, string>();
            }
            if (this.DICR_EXECUTE_USERNAME_REAL == null)
            {
                this.DICR_EXECUTE_USERNAME_REAL = new Dictionary<string, string>();
            }


        }
        public Mrs00040RDO() { }


        public string DESCRIPTION_SURGERY { get; set; }

        public string MISU_PPPT_REAL { get; set; }

        public string PTTT_GROUP_NAME_REAL { get; set; }

        public string HEIN_CARD_NUMBER_02 { get; set; }

        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public short? HAS_REAL { get; set; }

        public Dictionary<string, decimal> DIC_REQ_DEPARTMENT_AMOUNT { get; set; }

        public decimal SERVICE_AMOUNT { get; set; }

        public long? PTTT_GROUP_ID { get; set; }

        public long? PTTT_METHOD_ID { get; set; }

        public long? EMOTIONLESS_METHOD_ID { get; set; }
        public string BEFORE_MISU_CODE { get; set; }
        public string AFTER_MISU_CODE { get; set; }
        public string TDL_PATIENT_DOB_MALE_STR { get; set; }
        public string TDL_PATIENT_DOB_FEMALE_STR { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string BEFORE_MISU_FULL { set; get; }
        public string AFTER_MISU_FULL { get; set; }

        public string ICD_SUB_CODE { get; set; }
        public string ICD_SUB_NAME { get; set; }
        public string ICD_SUB_FULL { get; set; }

        public string INTRUCTION_NOTE { get; set; }

        public string PATIENT_TYPE_CODE { get; set; }//
        public string PATIENT_TYPE_NAME { get; set; }//đối tượng thanh toán

        public string TDL_PATIENT_TYPE_CODE { get; set; }//
        public string TDL_PATIENT_TYPE_NAME { get; set; }//đối tượng bệnh nhân

        public string MACHINE_CODE { get; set; }//
        public string MACHINE_NAME { get; set; }//máy dịch vụ

        public string EXECUTE_MACHINE_CODE { get; set; }//
        public string EXECUTE_MACHINE_NAME { get; set; }//máy thực hiện
    }
}
