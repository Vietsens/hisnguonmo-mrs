using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00688
{
    class Mrs00688RDO : HIS_SERE_SERV_BILL
    {
        public V_HIS_TRANSACTION TRANSACTION { get; set; }
        public HIS_TREATMENT TREATMENT { get; set; }
        public HIS_PATIENT_TYPE_ALTER ALTER { get; set; }
        public HIS_SERE_SERV SERE_SERV { get; set; }

        public string PATIENT_TYPE_NAME { get; set; }

        public string ALTER_PATIENT_TYPE_NAME { get; set; }
        public string ALTER_TREATMENT_TYPE_NAME { get; set; }
        public string ALTER_HEIN_CARD_FROM_TIME_STR { get; set; }
        public string ALTER_HEIN_CARD_TO_TIME_STR { get; set; }

        public string TREAT_IN_TIME_STR { get; set; }
        public string TREAT_OUT_TIME_STR { get; set; }
        public string FEE_LOCK_TIME_STR { get; set; }

        public string TRANSACTION_TIME_STR { get; set; }

        public string TDL_EXECUTE_DEPARTMENT_NAME { get; set; }
        public string TDL_EXECUTE_ROOM_NAME { get; set; }
        public string TDL_REQUEST_ROOM_NAME { get; set; }
        public string TDL_SERVICE_TYPE_NAME { get; set; }
        public string TREAT_DEPARTMENT_NAME { get; set; }

        public Mrs00688RDO(HIS_SERE_SERV_BILL data, HIS_SERE_SERV sereServ, HIS_TREATMENT treat, V_HIS_TRANSACTION tran, HIS_PATIENT_TYPE_ALTER alter)
        {
            if (data != null)
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00688RDO>(this, data);
            }

            if (sereServ != null)
            {
                SERE_SERV = sereServ;
                var patient = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServ.PATIENT_TYPE_ID);
                if (patient != null)
                {
                    PATIENT_TYPE_NAME = patient.PATIENT_TYPE_NAME;
                }

                var reqRoom = MANAGER.Config.HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_ROOM_ID);
                if (reqRoom != null)
                {
                    TDL_REQUEST_ROOM_NAME = reqRoom.ROOM_NAME;
                }

                var exeRoom = MANAGER.Config.HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServ.TDL_EXECUTE_ROOM_ID);
                if (exeRoom != null)
                {
                    TDL_EXECUTE_ROOM_NAME = exeRoom.ROOM_NAME;
                }

                var exeDepa = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sereServ.TDL_EXECUTE_DEPARTMENT_ID);
                if (exeDepa != null)
                {
                    TDL_EXECUTE_DEPARTMENT_NAME = exeDepa.DEPARTMENT_NAME;
                }

                var serviceType = MANAGER.Config.HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == sereServ.TDL_SERVICE_TYPE_ID);
                if (serviceType != null)
                {
                    TDL_SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                }
            }
            else
            {
                SERE_SERV = new HIS_SERE_SERV();
            }

            if (alter != null)
            {
                ALTER = alter;

                var alterPatient = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ALTER.PATIENT_TYPE_ID);
                if (alterPatient != null)
                {
                    ALTER_PATIENT_TYPE_NAME = alterPatient.PATIENT_TYPE_NAME;
                }

                var alterTreatment = MANAGER.Config.HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == ALTER.TREATMENT_TYPE_ID);
                if (alterTreatment != null)
                {
                    ALTER_TREATMENT_TYPE_NAME = alterTreatment.TREATMENT_TYPE_NAME;
                }

                if (ALTER.HEIN_CARD_FROM_TIME.HasValue)
                {
                    ALTER_HEIN_CARD_FROM_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(ALTER.HEIN_CARD_FROM_TIME.Value);
                }

                if (ALTER.HEIN_CARD_TO_TIME.HasValue)
                {
                    ALTER_HEIN_CARD_TO_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(ALTER.HEIN_CARD_TO_TIME.Value);
                }
            }
            else
            {
                ALTER = new HIS_PATIENT_TYPE_ALTER();
            }

            if (treat != null)
            {
                TREATMENT = treat;
                TREAT_IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treat.IN_TIME);

                if (treat.OUT_TIME.HasValue)
                {
                    TREAT_OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treat.OUT_TIME.Value);
                }

                if (treat.FEE_LOCK_TIME.HasValue)
                {
                    FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treat.FEE_LOCK_TIME.Value);
                }

                if (String.IsNullOrWhiteSpace(ALTER_PATIENT_TYPE_NAME))
                {
                    var alterPatient = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treat.TDL_PATIENT_TYPE_ID);
                    if (alterPatient != null)
                    {
                        ALTER_PATIENT_TYPE_NAME = alterPatient.PATIENT_TYPE_NAME;
                    }
                }

                if (String.IsNullOrWhiteSpace(ALTER_TREATMENT_TYPE_NAME))
                {
                    var alterTreatment = MANAGER.Config.HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == treat.TDL_TREATMENT_TYPE_ID);
                    if (alterTreatment != null)
                    {
                        ALTER_TREATMENT_TYPE_NAME = alterTreatment.TREATMENT_TYPE_NAME;
                    }
                }

                if (String.IsNullOrWhiteSpace(ALTER.HEIN_CARD_NUMBER))
                {
                    ALTER.HEIN_CARD_NUMBER = treat.TDL_HEIN_CARD_NUMBER;
                }
            }
            else
            {
                TREATMENT = new HIS_TREATMENT();
            }

            if (tran != null)
            {
                TRANSACTION = tran;
                TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(tran.TRANSACTION_TIME);
            }
            else
            {
                TRANSACTION = new V_HIS_TRANSACTION();
            }
            HIS_DEPARTMENT depa = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treat.LAST_DEPARTMENT_ID);
            if (depa != null)
            {
                TREAT_DEPARTMENT_NAME = depa.DEPARTMENT_NAME;
            }
        }
    }
}
