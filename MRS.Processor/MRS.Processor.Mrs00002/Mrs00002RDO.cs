using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using MOS.Filter;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;


namespace MRS.Processor.Mrs00002
{
    class Mrs00002RDO:HIS_TREATMENT
    {

        public int COUNT_GROUP1 { get; set; }
        public int COUNT_GROUP2 { get; set; }
        public int COUNT_GROUP3 { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_CODE { get; set; }
        public string EXECUTE_ROOM_CODE { get; set; }
        public int COUNT_EXAM { get; set; }
        public int COUNT_EMERGENCY_EXAM { get; set; }
        public int COUNT_TREATMENT_EXAM { get; set; }
        public int COUNT_CHILD_EXAM { get; set; }
        public int COUNT_ELDERLY_EXAM_MORE60 { get; set; }
        public int COUNT_ELDERLY_EXAM_BH_MORE60 { get; set; }
        public int COUNT_ELDERLY_EXAM_VP_MORE60 { get; set; }

        public int COUNT_CHILD_EXAM_LESS7 { get; set; }
        public int COUNT_CHILD_EXAM_LESS6 { get; set; }
        public int COUNT_CHILD_EXAM_LESS5 { get; set; }
        public int COUNT_CHILD_EXAM_LESS1 { get; set; }
        public int COUNT_FEMALE_EXAM { get; set; }

        public int COUNT_OUT_TREAT_EXAM_LESS7 { get; set; }
        public int COUNT_OUT_TREAT_EXAM_LESS6 { get; set; }
        public int COUNT_OUT_TREAT_EXAM_LESS5 { get; set; }
        public int COUNT_OUT_TREAT_EXAM_LESS1 { get; set; }
        public int COUNT_OUT_TREAT_EXAM_LESS15 { get; set; }



        public int COUNT_MALE_EXAM { get; set; }

        public int COUNT_IN_TREAT_EXAM { get; set; }
        public int COUNT_TRANPATI_EXAM { get; set; }
        public int COUNT_END_EXAM { get; set; }
        public int COUNT_MEDI_HOME_EXAM { get; set; }
        public int COUNT_APPOINTMENT_EXAM { get; set; }
        public int COUNT_MOVE_ROOM_EXAM { get; set; }
        public int COUNT_OUT_TREAT_EXAM { get; set; }
        public long DAY_OUT_TREAT_EXAM { get; set; }

        public int COUNT_DONE_EXAM { get; set; }

        public long COUNT_FINISH_EXAM { get; set; }

        public long COUNT_IMP { get; set; }

        public long COUNT_WAIT_EXAM { get; set; }

        public long COUNT_DEATH_EXAM { get; set; }

        public int COUNT_LEFT_ROUTE_EXAM { get; set; }

        public int COUNT_KSK { get; set; }

        public int COUNT_FREE { get; set; }

        public int COUNT_TREAT_EXAM_LESS7 { get; set; }
        public int COUNT_TREAT_EXAM_LESS6 { get; set; }
        public int COUNT_TREAT_EXAM_LESS5 { get; set; }
        public int COUNT_TREAT_EXAM_LESS1 { get; set; }
        public int COUNT_TREAT_EXAM_LESS15 { get; set; }//tre em

        public Mrs00002RDO()
        {
           
        }

        public string EXECUTE_USERNAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public int COUNT_CLINICAL_IN { get; set; }

        public int COUNT_TRANPATI_NOT_TREAT_IN { get; set; }

        public long EXECUTE_ROOM_ID { get; set; }

        public int COUNT_CLINICAL_IN_NOT_NIGHT { get; set; }

        public int COUNT_EXAM_MAIN { get; set; }

        public int COUNT_EXAM_MAIN_BH { get; set; }

        public int COUNT_EXAM_MAIN_DV { get; set; }

        public int COUNT_EXAM_EXT_BH { get; set; }

        public int COUNT_EXAM_EXT_DV { get; set; }

        public int COUNT_EXAM_FROM_REA { get; set; }

        public int COUNT_TRANPATI_EXAM_LEN { get; set; }

        public int COUNT_EXAM_LEN_LIEN_KE { get; set; }

        public int COUNT_EXAM_CUNG_TUYEN { get; set; }

        public int COUNT_EXAM_FROM_REA_BH { get; set; }

        public int COUNT_EXAM_FROM_REA_DV { get; set; }

        public int COUNT_EXAM_MAIN_IN_PROVIN { get; set; }

        public int COUNT_EXAM_MAIN_OUT_PROVIN { get; set; }

        public long PATIENT_TYPE_ID { get; set; }

        public double HEIN_RATIO { get; set; }

        public int? AGE { get; set; }


        public int COUNT_MEDI_HOME_EXAM_BH { get; set; }

        public int COUNT_MEDI_HOME_EXAM_CA { get; set; }

        public int COUNT_EXAM_MAIN_CA { get; set; }

        public int COUNT_EXAM_EXT_CA { get; set; }

        public int COUNT_KSK_CONTRACT_EXAM { get; set; }

        public int COUNT_MEDI_HOME_KSK_CON { get; set; }

        public int COUNT_KSKNN_EXAM { get; set; }

        public int COUNT_MEDI_HOME_KSKNN { get; set; }

        public int COUNT_OLD_EXAM { get; set; }

        public int COUNT_MEDI_HOME_OLD { get; set; }

        public int COUNT_EMERGENCY_EXAM_BHYT_CA { get; set; }

        public int COUNT_EMERGENCY_EXAM_BHYT { get; set; }

        public int COUNT_EMERGENCY_EXAM_PN { get; set; }

        public int COUNT_EMERGENCY_EXAM_CBCC { get; set; }

        public int COUNT_EMERGENCY_EXAM_NN { get; set; }

        public int COUNT_KSK_EXAM_BHYT_CA { get; set; }

        public int COUNT_KSK_EXAM_BHYT { get; set; }

        public int COUNT_KSK_EXAM_PN { get; set; }

        public int COUNT_KSK_EXAM_CBCC { get; set; }

        public int COUNT_KSK_EXAM_VP { get; set; }

        public int COUNT_EMERGENCY_EXAM_VP { get; set; }

        //public long TDL_PATIENT_CLASSIFY_ID { get; set; }
        public string PATIENT_CLASSIFY_CODE { get; set; }
        public Dictionary<string, long> DIC_COUNT_PATIENT_CLASSIFY { get; set; }
        public Dictionary<string, long> DIC_COUNT_EXE_REQ { get; set; }

        public int COUNT_CLASSIFY { get; set; }
        //public string  TDL_PATIENT_ADDRESS { get; set; }

        public string HEIN_CARD_TYPE { get; set; }

        public long? COUNT_CA_EXAM { get; set; }

        public long? COUNT_OTHER_EXAM { get; set; }

        public int COUNT_GROUP_1 { get; set; }

        public int COUNT_GROUP_2 { get; set; }

        public int COUNT_GROUP_3 { get; set; }

        public int COUNT_CA_EXAM_PATIENT_TYPE { get; set; }

        public int COUNT_OTHER_EXAM_PATIENT_TYPE { get; set; }

        public long? COUNT_CA_EXAM_PATIENT_CLASSIFY { get; set; }

        public long? COUNT_OTHER_EXAM_PATIENT_CLASSIFY { get; set; }
        public long? MILITARY_RANK_NUM_ORDER { get; set; }
        public long? COUNT_CA_NORMAL { get; set; }
        public long? COUNT_CA_HIGH_LEVEL { get; set; }




        public int COUNT_IMRO_ROOM_EXAM { get; set; }

        public int COUNT_BH_EXAM { get; set; }

        public int COUNT_CHILD_EXAM_BHYT { get; set; }

        public int COUNT_CHILD_EXAM_BH_LESS7 { get; set; }

        public int COUNT_CHILD_EXAM_BH_LESS6 { get; set; }

        public int COUNT_CHILD_EXAM_BH_LESS5 { get; set; }

        public int COUNT_CHILD_EXAM_BH_LESS1 { get; set; }

        public int COUNT_ELDERLY_EXAM_MORE90 { get; set; }

        public int COUNT_ELDERLY_EXAM_BH_MORE90 { get; set; }

        public int COUNT_OLD_TREATMENT_EXAM { get; set; }
    }
}


