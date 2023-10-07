using MRS.Processor.Mrs00518;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00518
{
    public class Mrs00518RDO
    {
        public long SERE_SERV_ID { get; set; }
        public long TDL_TREATMENT_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public long TDL_REQUEST_ROOM_ID { get; set; }
        public string ROOM_NAME { get; set; }
        public decimal COUNT_ALL { get; set; }
        public decimal COUNT_BHYT { get; set; }
        public decimal COUNT_DV { get; set; }
        public decimal COUNT_FREE { get; set; }
        public decimal COUNT_KSK { get; set; }
        public decimal COUNT_TE { get; set; }
        public decimal COUNT_PT { get; set; }
        public decimal COUNT_TT { get; set; }
        public decimal COUNT_IN { get; set; }
        public decimal COUNT_CV { get; set; }
        public decimal COUNT_TV { get; set; }
        public decimal COUNT_CC { get; set; }
        public decimal COUNT_CK { get; set; }
        public decimal COUNT_OUT { get; set; }
        public decimal DAY_OUT { get; set; }


        public Mrs00518RDO(SERE_SERV r, List<V_HIS_ROOM> listRoom)
        {

            var room = listRoom.FirstOrDefault(o => o.ID == r.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM();
            this.ROOM_NAME = room.ROOM_NAME;
            this.TDL_REQUEST_ROOM_ID = r.TDL_REQUEST_ROOM_ID;
            this.TDL_TREATMENT_ID = r.TDL_TREATMENT_ID??0;
            this.TREATMENT_CODE = r.TDL_TREATMENT_CODE;
            if (r.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
            {
                this.COUNT_ALL = 1;
                if (r.IS_EMERGENCY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    this.COUNT_CC = 1;
                if (r.PATIENT_TYPE == "BHYT")
                    this.COUNT_BHYT = 1;
                if (r.PATIENT_TYPE == "IS_FREE")
                    this.COUNT_FREE = 1;
                if (r.PATIENT_TYPE == "DV" || r.PATIENT_TYPE == "FEE")
                    this.COUNT_DV = 1;
                if (r.PATIENT_TYPE == "KSK" || r.PATIENT_TYPE == "KSKHD")
                    this.COUNT_KSK = 1;
                if (r.EXAM_END_TYPE == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ.EXAM_END_TYPE__KHAM_THEM)
                {
                    this.COUNT_CK = 1;
                }
                if (r.TDL_PATIENT_DOB - r.TDL_INTRUCTION_DATE <6)
                    this.COUNT_TE = 1;
                if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    this.COUNT_IN = 1;
                if (r.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && r.END_ROOM_ID == r.TDL_EXECUTE_ROOM_ID)
                    this.COUNT_CV = 1;
                if (r.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET && r.END_ROOM_ID == r.TDL_EXECUTE_ROOM_ID)
                    this.COUNT_TV = 1;
                if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    this.COUNT_OUT = 1;
                if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    this.DAY_OUT = DateDiff.diffDate(r.CLINICAL_IN_TIME ?? r.IN_TIME, r.OUT_TIME);
            }
            else if (r.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
            {
                this.SERE_SERV_ID = r.ID;
                this.COUNT_PT = 1;
            }
            else if (r.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
            {
                this.SERE_SERV_ID = r.ID;
                this.COUNT_TT = 1;
            }


        }

        public Mrs00518RDO()
        {

        }
    }
    public class SERE_SERV
    {
        public decimal AMOUNT { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public long ID { get; set; }
        public short? IS_DELETE { get; set; }
        public short? IS_EXPEND { get; set; }
        public short? IS_NO_EXECUTE { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public long SERVICE_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public long TDL_EXECUTE_ROOM_ID { get; set; }
        public long TDL_INTRUCTION_DATE { get; set; }
        public long TDL_INTRUCTION_TIME { get; set; }
        public long? TDL_PATIENT_ID { get; set; }
        public long TDL_REQUEST_DEPARTMENT_ID { get; set; }
        public string TDL_REQUEST_LOGINNAME { get; set; }
        public long TDL_REQUEST_ROOM_ID { get; set; }
        public string TDL_REQUEST_USERNAME { get; set; }
        public string TDL_SERVICE_CODE { get; set; }
        public string TDL_SERVICE_DESCRIPTION { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public string TDL_SERVICE_REQ_CODE { get; set; }
        public long TDL_SERVICE_REQ_TYPE_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public long TDL_SERVICE_UNIT_ID { get; set; }
        public string TDL_TREATMENT_CODE { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }

        public long? TDL_EXECUTE_DEPARTMENT_ID { get; set; }

        public long? CLINICAL_IN_TIME { get; set; }

        public long? TDL_PATIENT_DOB { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public long? TREATMENT_END_TYPE_ID { get; set; }

        public long? END_ROOM_ID { get; set; }

        public long? OUT_TIME { get; set; }

        public long IN_TIME { get; set; }

        public short? IS_EMERGENCY { get; set; }

        public string PATIENT_TYPE { get; set; }

        public long? EXAM_END_TYPE { get; set; }
    }
}
