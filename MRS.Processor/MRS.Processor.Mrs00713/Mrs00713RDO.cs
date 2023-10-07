using MOS.MANAGER.HisService;
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

namespace MRS.Processor.Mrs00713
{
    public class Mrs00713RDO
    {
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? FEE_LOCK_TIME { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public short? IS_PRIORITY { get; set; }
        public long? EXAM_START_TIME { get; set; }
        public long? TEST_START_TIME { get; set; }
        public long? TEST_RESULT_TIME { get; set; }
        public long? CDHA_START_TIME { get; set; }
        public long? CDHA_FINISH_TIME { get; set; }
        public long? TDCN_START_TIME { get; set; }
        public long? TDCN_FINISH_TIME { get; set; }
        public long? CLS_FINISH_TIME { get; set; }
        public long? EXP_FINISH_TIME { get; set; }
        public double WAIT_EXAM { get; set; }
        public double WAIT_OUT { get; set; }
        public double WAIT_TEST_RESULT { get; set; }
        public double WAIT_CDHA_FINISH { get; set; }
        public double WAIT_TDCN_FINISH { get; set; }
        public double WAIT_CLS_OUT { get; set; }
        public double WAIT_FEE_LOCK { get; set; }
        public double WAIT_MEDI_EXP { get; set; }

        public long? EXAM_FINISH_TIME { get; set; }

        public long CREATE_TIME { get; set; }

        public string CREATE_TIME_STR { get; set; }

        public string IN_TIME_STR { get; set; }

        public string EXAM_START_TIME_STR { get; set; }

        public string TEST_START_TIME_STR { get; set; }

        public string EXAM_END_TIME_STR { get; set; }

        public double WAIT_FOR_IN { get; set; }

        public long? EXAM_INTRUCTION_TIME { get; set; }

        public long? TEST_INTRUCTION_TIME { get; set; }

        public long? CDHA_INTRUCTION_TIME { get; set; }

        public long? TDCN_INTRUCTION_TIME { get; set; }

        public string TEST_RESULT_TIME_STR { get; set; }

        public string CDHA_START_TIME_STR { get; set; }

        public string CDHA_FINISH_TIME_STR { get; set; }

        public string TDCN_START_TIME_STR { get; set; }

        public string TDCN_FINISH_TIME_STR { get; set; }

        public string FEE_LOCK_TIME_STR { get; set; }

        public string EXP_FINISH_TIME_STR { get; set; }

        public string EXAM_INTRUCTION_TIME_STR { get; set; }

        public string TEST_INTRUCTION_TIME_STR { get; set; }

        public string CDHA_INTRUCTION_TIME_STR { get; set; }

        public string TDCN_INTRUCTION_TIME_STR { get; set; }

        public double WAIT_FOR_TDCN { get; set; }

        public double WAIT_FOR_CDHA { get; set; }

        public double WAIT_FOR_TEST { get; set; }

        public long? TDL_FIRST_EXAM_ROOM_ID { get; set; }

        public string EXAM_ROOM_CODE { get; set; }

        public string EXAM_ROOM_NAME { get; set; }

        public double TIME_FOR_EXAM { get; set; }

        public long? TEST_FINISH_TIME { get; set; }

        public string TEST_FINISH_TIME_STR { get; set; }
    }
}
