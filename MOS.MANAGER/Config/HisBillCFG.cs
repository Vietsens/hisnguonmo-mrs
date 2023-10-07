using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBedRoom;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisMestRoom;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisBillCFG
    {
        private const string BHYT_MUST_FINISH_TREATMENT_BEFORE_BILLING_CFG = "MOS.HIS_BILL.BHYT.MUST_FINISH_TREATMENT_BEFORE_BILLING";

        private static bool? mustFinishTreatmentBeforeBilling;
        public static bool BHYT_MUST_FINISH_TREATMENT_BEFORE_BILLING
        {
            get
            {
                if (!mustFinishTreatmentBeforeBilling.HasValue)
                {
                    mustFinishTreatmentBeforeBilling = ConfigUtil.GetIntConfig(BHYT_MUST_FINISH_TREATMENT_BEFORE_BILLING_CFG) == 1;
                }
                return mustFinishTreatmentBeforeBilling.Value;
            }
            set
            {
                mustFinishTreatmentBeforeBilling = value;
            }
        }

        public static void Reload()
        {
            mustFinishTreatmentBeforeBilling = ConfigUtil.GetIntConfig(BHYT_MUST_FINISH_TREATMENT_BEFORE_BILLING_CFG) == 1;
        }
    }
}
