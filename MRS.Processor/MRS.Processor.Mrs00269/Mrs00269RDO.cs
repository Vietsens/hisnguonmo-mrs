using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;

namespace MRS.Processor.Mrs00269
{

    public class Mrs00269RDO
    {
        public V_HIS_TREATMENT_4 V_HIS_TREATMENT_4 { get; set; }
        public string CREATE_TIME { get; set; }//
        public string IN_TIME { get; set; }//
        public string CREATOR { get; set; }
        public string TREATMENT_CODE { get; set; }//
        public string PATIENT_NAME { get; set; }//Lấy từ bảng V_HIS_TREATMENT_SUMMARY qua treatment_ID
        public int? AGE_MALE { get; set; }//Lấy từ bảng V_HIS_TREATMENT_SUMMARY treatment_ID
        public int? AGE_FEMALE { get; set; }//Lấy từ bảng V_HIS_TREATMENT_SUMMARY treatment_ID
        public string HEIN_CARD_NUMBER { get; set; }
        public string DEPARTMENT_NAME { get; set; }//Lấy từ bảng V_HIS_TREATMENT_SUMMARY
        public string ICD_CODE { get; set; }//Lấy từ bảng V_HIS_TREATMENT_SUMMARY
        public string ICD_NAME { get; set; }//Lấy từ bảng V_HIS_TREATMENT_SUMMARY
        public string FORM_1a { get; set; }//
        public string FORM_1b { get; set; }//
        public string FORM_2 { get; set; }//
        public string FORM_3 { get; set; }//
        public string REASON_4 { get; set; }//
        public string REASON_5 { get; set; }//
        public string MEDI_ORG_NAME { get; set; }//
        public string TOTAL_DAYS_IN_TREATMENT { get; set; }
        public string TRAN_IN_TIME { get; set; }
        public Mrs00269RDO(V_HIS_TREATMENT_4 r)
        {
            this.V_HIS_TREATMENT_4 = r;
        }

        public string TRAN_PATI_REASON_NAME { get; set; }
        public string TRAN_PATI_FORM_NAME { get; set; }
        public string TRAN_PATI_TECH_NAME { get; set; }

        public string IS_BHYT { get; set; }

        public string DOB_STR { get; set; }
        public int? AGE_STR { get; set; }
        public string MEDI_ORG_CODE { get; set; }
        public string LEVEL_CODE { get; set; }
        public int? MEDI_ORG_TYPE { get; set; }
        public string END_ROOM_DEPA_NAME { get; set; }
        public string IN_DEPA_ICD_CODE { get; set; }
        public string IN_DEPA_ICD_NAME { get; set; }
        public string IN_DEPA_NAME { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }

        public string TRANSFER_IN_ICD_CODE { get; set; }

        public string TRANSFER_IN_ICD_NAME { get; set; }

        public string TRANSPORT_VEHICLE { get; set; } //phương tiện vận chuyển
    }
}
