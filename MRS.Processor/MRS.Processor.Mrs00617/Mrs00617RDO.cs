using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00617
{
    public class Mrs00617RDO
    {
        public DATA_GET HisSereServ { get; set; }

        public decimal AMOUNT { get; set; }
        public decimal? NUMBER_OF_FILM { get; set; }
        public decimal TOTAL_NUMBER_OF_FILM { get; set; }
        public decimal AMOUNT_KSK { get; set; }

        public decimal AMOUNT_EXAM_ROOM { get; set; }

        public decimal AMOUNT_IN { get; set; }
        public decimal AMOUNT_IN_BHYT { get; set; }
        public decimal AMOUNT_IN_VP { get; set; }
        public decimal AMOUNT_IN_CA { get; set; }
        public decimal AMOUNT_IN_NN { get; set; }

        public decimal AMOUNT_OUT { get; set; }
        public decimal AMOUNT_OUT_BHYT { get; set; }
        public decimal AMOUNT_OUT_VP { get; set; }
        public decimal AMOUNT_OUT_CA { get; set; }
        public decimal AMOUNT_OUT_NN { get; set; }

        public decimal TOTAL_PRICE { get; set; }

        public string SERVICE_UNIT_NAME { get; set; }

        public string SERVICE_TYPE_NAME { get; set; }

        public long SERVICE_TYPE_ID { get; set; }

        public long TDL_REQUEST_DEPARTMENT_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_TYPE_ID { get; set; }
        public string PATIENT_TYPE_CODE_TREA { get; set; }
        public string PATIENT_TYPE_NAME_TREA { get; set; }
        public string PATIENT_TYPE_CODE_SS { get; set; }
        public string PATIENT_TYPE_NAME_SS { get; set; }

        public long TDL_REQUEST_ROOM_ID { get; set; }
        public string TDL_REQUEST_ROOM_CODE { get; set; }
        public string TDL_REQUEST_ROOM_NAME { get; set; }
        public long TDL_EXECUTE_ROOM_ID { get; set; }
        public string TDL_EXECUTE_ROOM_CODE { get; set; }
        public string TDL_EXECUTE_ROOM_NAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string PR_SERVICE_CODE { get; set; }
        public string PR_SERVICE_NAME { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public decimal PRICE { get; set; }
        public long INTRUCTION_TIME { get; set; }


        public string TDL_REQUEST_DEPARTMENT_CODE { get; set; }

        public string TDL_REQUEST_DEPARTMENT_NAME { get; set; }

        public Dictionary<string, decimal> DIC_AMOUNT_REQUEST_ROOM { get; set; }

        public Dictionary<string, decimal> DIC_AMOUNT_REQUEST_DEPA { get; set; }

        public string MACHINE_CODE { get; set; }

        public string MACHINE_NAME { get; set; }

        public string EXECUTE_MACHINE_CODE { get; set; }

        public string EXECUTE_MACHINE_NAME { get; set; }


        public Mrs00617RDO()
        {

        }
        public Mrs00617RDO(DATA_GET sereServGet, Dictionary<long, V_HIS_ROOM> dicRoom, long patientTypeIdKsk, List<HIS_MACHINE> ListMachine, Dictionary<long,List<HIS_SERVICE_MACHINE>> dicServiceMachine, decimal ValueAmount)
        {
            HisSereServ = sereServGet;
            if (sereServGet.TDL_PATIENT_TYPE_ID == patientTypeIdKsk)
            {
                AMOUNT_KSK = ValueAmount;
            }
            else
            {
                AMOUNT = ValueAmount;
            }
            //TOTAL_PRICE = sereServGet.VIR_TOTAL_PRICE ?? 0;
            TOTAL_NUMBER_OF_FILM = sereServGet.NUMBER_OF_FILM ?? 0 * sereServGet.AMOUNT;           
            if (sereServGet.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
            {
                AMOUNT_KHAM = ValueAmount;
                if (sereServGet.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    AMOUNT_KHAM_BHYT = ValueAmount;
                }
                else if (sereServGet.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                {
                    AMOUNT_KHAM_VP = ValueAmount;
                }
            }
            else
            {
                AMOUNT_SERVICE = ValueAmount;
            }
            AMOUNT_EXAM_ROOM = sereServGet.AMOUNT_EXAM_ROOM;
            if (sereServGet.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
            {
                if (sereServGet.CREATE_TIME < sereServGet.CLINICAL_IN_TIME)// nếu thời gian chỉ dịnh của dịch vụ nhỏ hơn thời gian vào điều trị nội trú thì tính cho ngoại trú
                {
                    AMOUNT_OUT = ValueAmount;
                    if (sereServGet.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        AMOUNT_OUT_BHYT = ValueAmount;
                    }
                    else if (sereServGet.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                    {
                        AMOUNT_OUT_VP = ValueAmount;
                    }
                    else if (sereServGet.TDL_PATIENT_TYPE_ID == 62)
                    {
                        AMOUNT_OUT_NN = ValueAmount;
                    }
                }
                else
                {
                    AMOUNT_IN = ValueAmount;
                    if (sereServGet.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        AMOUNT_IN_BHYT = ValueAmount;
                    }
                    else if (sereServGet.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                    {
                        AMOUNT_IN_VP = ValueAmount;
                    }
                    //else if (t.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__GTCA)
                    //{
                    //    AMOUNT_IN_CA = r.AMOUNT;
                    //}
                    else if (sereServGet.TDL_PATIENT_TYPE_ID == 62)
                    {
                        AMOUNT_IN_NN = ValueAmount;
                    }
                }
            }
            else
            {
                AMOUNT_OUT = ValueAmount;
                if (sereServGet.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    AMOUNT_OUT_BHYT = ValueAmount;
                }
                else if (sereServGet.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                {
                    AMOUNT_OUT_VP = ValueAmount;
                }
                //else if (t.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__GTCA)
                //{
                //    AMOUNT_OUT_CA = r.AMOUNT;
                //}
                else if (sereServGet.TDL_PATIENT_TYPE_ID == 62)
                {
                    AMOUNT_OUT_NN = ValueAmount;
                }
            }
            if (sereServGet.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
            {
                AMOUNT_CHUYENVIEN = ValueAmount;
            }
            SERVICE_UNIT_NAME = ((HIS_SERVICE_UNIT)((HisServiceUnitCFG.HisServiceUnits.FirstOrDefault((HIS_SERVICE_UNIT o) => o.ID == sereServGet.TDL_SERVICE_UNIT_ID)) ?? (new HIS_SERVICE_UNIT()))).SERVICE_UNIT_NAME;
            SERVICE_TYPE_NAME = ((HIS_SERVICE_TYPE)((HisServiceTypeCFG.HisServiceTypes.FirstOrDefault((HIS_SERVICE_TYPE o) => o.ID == sereServGet.TDL_SERVICE_TYPE_ID)) ?? (new HIS_SERVICE_TYPE()))).SERVICE_TYPE_NAME;
            PR_SERVICE_CODE = !string.IsNullOrEmpty(sereServGet.PARENT_SERVICE_CODE) ? sereServGet.PARENT_SERVICE_CODE : "NK";
            PR_SERVICE_NAME = !string.IsNullOrEmpty(sereServGet.PARENT_SERVICE_NAME) ? sereServGet.PARENT_SERVICE_NAME : "Nhóm dịch vụ khác";
            SERVICE_TYPE_ID = sereServGet.TDL_SERVICE_TYPE_ID;
            var machine = ListMachine.FirstOrDefault(o=>o.ID == sereServGet.MACHINE_ID);
            if (machine != null)
            {
                EXECUTE_MACHINE_CODE = machine.MACHINE_CODE;
                EXECUTE_MACHINE_NAME = machine.MACHINE_NAME;
            }

            var serviceMachine = dicServiceMachine.ContainsKey(sereServGet.SERVICE_ID) ? dicServiceMachine[sereServGet.SERVICE_ID] : null;
            if (serviceMachine != null && serviceMachine.Count > 0)
            {
                var mc = ListMachine.Where(p => serviceMachine.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                if (mc.Count > 0)
                {
                    MACHINE_NAME = string.Join(";", mc.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                    MACHINE_CODE = string.Join(";", mc.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                }
            }

            TDL_REQUEST_DEPARTMENT_ID = sereServGet.TDL_REQUEST_DEPARTMENT_ID ?? 0;
            TDL_REQUEST_DEPARTMENT_CODE = ((HIS_DEPARTMENT)((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault((HIS_DEPARTMENT o) => o.ID == sereServGet.TDL_REQUEST_DEPARTMENT_ID)) ?? (new HIS_DEPARTMENT()))).DEPARTMENT_CODE;
            TDL_REQUEST_DEPARTMENT_NAME = ((HIS_DEPARTMENT)((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault((HIS_DEPARTMENT o) => o.ID == sereServGet.TDL_REQUEST_DEPARTMENT_ID)) ?? (new HIS_DEPARTMENT()))).DEPARTMENT_NAME;
            TREATMENT_CODE = sereServGet.TDL_TREATMENT_CODE;
            PATIENT_CODE = sereServGet.TDL_PATIENT_CODE;
            PATIENT_NAME = sereServGet.TDL_PATIENT_NAME;
            PATIENT_TYPE_CODE_TREA = ((HIS_PATIENT_TYPE)((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == sereServGet.TDL_PATIENT_TYPE_ID)) ?? (new HIS_PATIENT_TYPE()))).PATIENT_TYPE_CODE;
            PATIENT_TYPE_NAME_TREA = ((HIS_PATIENT_TYPE)((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == sereServGet.TDL_PATIENT_TYPE_ID)) ?? (new HIS_PATIENT_TYPE()))).PATIENT_TYPE_NAME;
            PATIENT_TYPE_CODE_SS = ((HIS_PATIENT_TYPE)((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == sereServGet.PATIENT_TYPE_ID)) ?? (new HIS_PATIENT_TYPE()))).PATIENT_TYPE_CODE;
            PATIENT_TYPE_NAME_SS = ((HIS_PATIENT_TYPE)((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == sereServGet.PATIENT_TYPE_ID)) ?? (new HIS_PATIENT_TYPE()))).PATIENT_TYPE_NAME;
            TDL_REQUEST_ROOM_ID = sereServGet.TDL_REQUEST_ROOM_ID;
            TDL_REQUEST_ROOM_CODE = dicRoom.ContainsKey(sereServGet.TDL_REQUEST_ROOM_ID) ? dicRoom[sereServGet.TDL_REQUEST_ROOM_ID].ROOM_CODE : null;
            TDL_REQUEST_ROOM_NAME = dicRoom.ContainsKey(sereServGet.TDL_REQUEST_ROOM_ID) ? dicRoom[sereServGet.TDL_REQUEST_ROOM_ID].ROOM_NAME : null;
            TDL_EXECUTE_ROOM_ID = sereServGet.TDL_EXECUTE_ROOM_ID ?? 0;
            TDL_EXECUTE_ROOM_CODE = dicRoom.ContainsKey(sereServGet.TDL_EXECUTE_ROOM_ID ?? 0) ? dicRoom[sereServGet.TDL_EXECUTE_ROOM_ID ?? 0].ROOM_CODE : null;
            TDL_EXECUTE_ROOM_NAME = dicRoom.ContainsKey(sereServGet.TDL_EXECUTE_ROOM_ID ?? 0) ? dicRoom[sereServGet.TDL_EXECUTE_ROOM_ID ?? 0].ROOM_NAME : null;
            REQUEST_LOGINNAME = sereServGet.TDL_REQUEST_LOGINNAME;
            REQUEST_USERNAME = sereServGet.TDL_REQUEST_USERNAME;

            SERVICE_CODE = sereServGet.TDL_SERVICE_CODE;
            SERVICE_NAME = sereServGet.TDL_SERVICE_NAME;

            PRICE = sereServGet.PRICE ?? 0;
            VIR_TOTAL_PRICE = sereServGet.VIR_TOTAL_PRICE ?? 0;
            TOTAL_PRICE = sereServGet.TOTAL_PRICE ?? 0;
            INTRUCTION_TIME = sereServGet.TDL_INTRUCTION_TIME ?? 0;
        }




        public decimal AMOUNT_KHAM { get; set; }

        public decimal AMOUNT_KHAM_BHYT { get; set; }

        public decimal AMOUNT_KHAM_VP { get; set; }

        public decimal AMOUNT_CHUYENVIEN { get; set; }

        public decimal AMOUNT_SERVICE { get; set; }

        public long? TREATMENT_END_TYPE_ID { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public decimal VIR_TOTAL_PRICE { get; set; }
    }
    public class DATA_GET
    {
        public decimal AMOUNT_EXAM_ROOM { get; set; }

        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public long CREATE_TIME { get; set; }
        public long IN_TIME { get; set; }
        public long CLINICAL_IN_TIME { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }

        public string PARENT_SERVICE_CODE { get; set; }

        public string PARENT_SERVICE_NAME { get; set; }

        public short? HAS_TEST_VALUE { get; set; }

        public long? TDL_PATIENT_DOB { get; set; }

        public long? TREATMENT_END_TYPE_ID { get; set; }

        public long? PARENT_SERVICE_ID { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }

        public decimal AMOUNT { get; set; }
        public decimal? NUMBER_OF_FILM { get; set; }

        public string TDL_SERVICE_CODE { get; set; }

        public string TDL_SERVICE_NAME { get; set; }

        public decimal? PRICE { get; set; }

        public long SERVICE_ID { get; set; }

        public long? TDL_TREATMENT_ID { get; set; }

        public long TDL_SERVICE_TYPE_ID { get; set; }

        public long PATIENT_TYPE_ID { get; set; }

        public long? SERVICE_REQ_ID { get; set; }

        public string TDL_REQUEST_LOGINNAME { get; set; }

        public string TDL_REQUEST_USERNAME { get; set; }

        public long TDL_REQUEST_ROOM_ID { get; set; }

        public long? TDL_REQUEST_DEPARTMENT_ID { get; set; }

        public decimal? TOTAL_PRICE { get; set; }

        public decimal? VIR_TOTAL_PRICE { get; set; }

        public string TDL_TREATMENT_CODE { get; set; }

        public long? TDL_EXECUTE_ROOM_ID { get; set; }

        public long? TDL_INTRUCTION_TIME { get; set; }

        public long? TDL_SERVICE_UNIT_ID { get; set; }

        public long? MACHINE_ID { get; set; }

        public long ID { get; set; }
    }
}