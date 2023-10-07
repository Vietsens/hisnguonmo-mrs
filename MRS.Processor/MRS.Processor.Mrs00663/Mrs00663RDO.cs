using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00663
{
    public class Mrs00663RDO
    {
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string DOB { get; set; }
        public string ICD_NAME { get; set; }
        public string SERVICE_NAME { get; set; }
        public string ICD_TEXT { get; set; }
        public string REQ_USERNAME { get; set; }
        public string REQ_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public string INSTRUCTION_TIME { get; set; }
        public string FINISH_TIME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public long REQUEST_DEPARTMENT_ID { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string WORK_PLACE_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal VIR_PRICE { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }


        //public decimal AMOUNT_1 { get; set; }

        //public decimal AMOUNT_2 { get; set; }

        //public decimal AMOUNT_3 { get; set; }

        //public decimal AMOUNT_4 { get; set; }

        //public decimal AMOUNT_5 { get; set; }

        //public decimal AMOUNT_6 { get; set; }

        //public decimal AMOUNT_7 { get; set; }

        //public decimal AMOUNT_8 { get; set; }

        //public decimal AMOUNT_9 { get; set; }

        //public decimal AMOUNT_10 { get; set; }

        //public decimal AMOUNT_11 { get; set; }

        //public decimal AMOUNT_12 { get; set; }

        //public decimal AMOUNT_13 { get; set; }

        //public decimal AMOUNT_14 { get; set; }

        //public decimal AMOUNT_15 { get; set; }

        //public decimal AMOUNT_16 { get; set; }

        //public decimal AMOUNT_17 { get; set; }

        //public decimal AMOUNT_18 { get; set; }

        //public decimal AMOUNT_19 { get; set; }

        //public decimal AMOUNT_20 { get; set; }

        //public decimal AMOUNT_21 { get; set; }

        //public decimal AMOUNT_22 { get; set; }

        //public decimal AMOUNT_23 { get; set; }

        //public decimal AMOUNT_24 { get; set; }

        //public decimal AMOUNT_25 { get; set; }

        //public decimal AMOUNT_26 { get; set; }

        //public decimal AMOUNT_27 { get; set; }

        //public decimal AMOUNT_28 { get; set; }

        //public decimal AMOUNT_29 { get; set; }

        //public decimal AMOUNT_30 { get; set; }

        //public decimal AMOUNT_31 { get; set; }

        //public decimal AMOUNT_32 { get; set; }

        //public decimal AMOUNT_33 { get; set; }

        //public decimal AMOUNT_34 { get; set; }

        //public decimal AMOUNT_35 { get; set; }

        //public decimal AMOUNT_36 { get; set; }

        //public decimal AMOUNT_37 { get; set; }

        //public decimal AMOUNT_38 { get; set; }

        //public decimal AMOUNT_39 { get; set; }

        //public decimal AMOUNT_40 { get; set; }

        //public decimal AMOUNT_41 { get; set; }

        //public decimal AMOUNT_42 { get; set; }

        //public decimal AMOUNT_43 { get; set; }

        //public decimal AMOUNT_44 { get; set; }

        //public decimal AMOUNT_45 { get; set; }

        //public decimal AMOUNT_46 { get; set; }

        //public decimal AMOUNT_47 { get; set; }

        //public decimal AMOUNT_48 { get; set; }

        //public decimal AMOUNT_49 { get; set; }

        //public decimal AMOUNT_50 { get; set; }

        public Mrs00663RDO()
        {
            // TODO: Complete member initialization
        }
    }
    //public class SERVICE_NAME
    //{
    //    public decimal SERVICE_NAME_1 { get; set; }

    //    public decimal SERVICE_NAME_2 { get; set; }

    //    public decimal SERVICE_NAME_3 { get; set; }

    //    public decimal SERVICE_NAME_4 { get; set; }

    //    public decimal SERVICE_NAME_5 { get; set; }

    //    public decimal SERVICE_NAME_6 { get; set; }

    //    public decimal SERVICE_NAME_7 { get; set; }

    //    public decimal SERVICE_NAME_8 { get; set; }

    //    public decimal SERVICE_NAME_9 { get; set; }

    //    public decimal SERVICE_NAME_10 { get; set; }

    //    public decimal SERVICE_NAME_11 { get; set; }

    //    public decimal SERVICE_NAME_12 { get; set; }

    //    public decimal SERVICE_NAME_13 { get; set; }

    //    public decimal SERVICE_NAME_14 { get; set; }

    //    public decimal SERVICE_NAME_15 { get; set; }

    //    public decimal SERVICE_NAME_16 { get; set; }

    //    public decimal SERVICE_NAME_17 { get; set; }

    //    public decimal SERVICE_NAME_18 { get; set; }

    //    public decimal SERVICE_NAME_19 { get; set; }

    //    public decimal SERVICE_NAME_20 { get; set; }

    //    public decimal SERVICE_NAME_21 { get; set; }

    //    public decimal SERVICE_NAME_22 { get; set; }

    //    public decimal SERVICE_NAME_23 { get; set; }

    //    public decimal SERVICE_NAME_24 { get; set; }

    //    public decimal SERVICE_NAME_25 { get; set; }

    //    public decimal SERVICE_NAME_26 { get; set; }

    //    public decimal SERVICE_NAME_27 { get; set; }

    //    public decimal SERVICE_NAME_28 { get; set; }

    //    public decimal SERVICE_NAME_29 { get; set; }

    //    public decimal SERVICE_NAME_30 { get; set; }

    //    public decimal SERVICE_NAME_31 { get; set; }

    //    public decimal SERVICE_NAME_32 { get; set; }

    //    public decimal SERVICE_NAME_33 { get; set; }

    //    public decimal SERVICE_NAME_34 { get; set; }

    //    public decimal SERVICE_NAME_35 { get; set; }

    //    public decimal SERVICE_NAME_36 { get; set; }

    //    public decimal SERVICE_NAME_37 { get; set; }

    //    public decimal SERVICE_NAME_38 { get; set; }

    //    public decimal SERVICE_NAME_39 { get; set; }

    //    public decimal SERVICE_NAME_40 { get; set; }

    //    public decimal SERVICE_NAME_41 { get; set; }

    //    public decimal SERVICE_NAME_42 { get; set; }

    //    public decimal SERVICE_NAME_43 { get; set; }

    //    public decimal SERVICE_NAME_44 { get; set; }

    //    public decimal SERVICE_NAME_45 { get; set; }

    //    public decimal SERVICE_NAME_46 { get; set; }

    //    public decimal SERVICE_NAME_47 { get; set; }

    //    public decimal SERVICE_NAME_48 { get; set; }

    //    public decimal SERVICE_NAME_49 { get; set; }

    //    public decimal SERVICE_NAME_50 { get; set; }
    //}


}
