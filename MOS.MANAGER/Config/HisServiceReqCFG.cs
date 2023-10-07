

namespace MOS.MANAGER.Config
{
    class HisServiceReqCFG
    {
        private const string NOT_REQUIRE_FEE_FOR_BHYT_CFG = "MOS.HIS_SERVICE_REQ.NOT_REQUIRE_FEE_FOR_BHYT";
        private const string ALLOW_MODIFYING_OF_STARTED_CFG = "MOS.HIS_SERVICE_REQ.ALLOW_MODIFYING_OF_STARTED";
        private const string SPLIT_BY_PATIENT_TYPE_CFG = "MOS.HIS_SERVICE_REQ.SPLIT_BY_PATIENT_TYPE";
        private const string PRESCRIPTION_SPLIT_OUT_MEDISTOCK_CFG = "MOS.HIS_SERVICE_REQ.PRESCRIPTION_SPLIT_OUT_MEDISTOCK"; //tach rieng don ke ngoai kho

        private static bool? notRequireFeeForBhyt;
        public static bool NOT_REQUIRE_FEE_FOR_BHYT
        {
            get
            {
                if (!notRequireFeeForBhyt.HasValue)
                {
                    notRequireFeeForBhyt = ConfigUtil.GetIntConfig(NOT_REQUIRE_FEE_FOR_BHYT_CFG) == 1;
                }
                return notRequireFeeForBhyt.Value;
            }
            set
            {
                notRequireFeeForBhyt = value;
            }
        }

        private static bool? splitByPatientType;
        public static bool SPLIT_BY_PATIENT_TYPE
        {
            get
            {
                if (!splitByPatientType.HasValue)
                {
                    splitByPatientType = ConfigUtil.GetIntConfig(SPLIT_BY_PATIENT_TYPE_CFG) == 1;
                }
                return splitByPatientType.Value;
            }
            set
            {
                splitByPatientType = value;
            }
        }

        private static bool? prescriptionSplitOutMediStock;
        public static bool PRESCRIPTION_SPLIT_OUT_MEDISTOCK
        {
            get
            {
                if (!prescriptionSplitOutMediStock.HasValue)
                {
                    prescriptionSplitOutMediStock = ConfigUtil.GetIntConfig(PRESCRIPTION_SPLIT_OUT_MEDISTOCK_CFG) == 1;
                }
                return prescriptionSplitOutMediStock.Value;
            }
            set
            {
                prescriptionSplitOutMediStock = value;
            }
        }

        private static bool? allowModifyingOfStarted;
        public static bool ALLOW_MODIFYING_OF_STARTED
        {
            get
            {
                if (!allowModifyingOfStarted.HasValue)
                {
                    allowModifyingOfStarted = ConfigUtil.GetIntConfig(ALLOW_MODIFYING_OF_STARTED_CFG) == 1;
                }
                return allowModifyingOfStarted.Value;
            }
            set
            {
                allowModifyingOfStarted = value;
            }
        }

        public static void Reload()
        {
            notRequireFeeForBhyt = ConfigUtil.GetIntConfig(NOT_REQUIRE_FEE_FOR_BHYT_CFG) == 1;
            splitByPatientType = ConfigUtil.GetIntConfig(SPLIT_BY_PATIENT_TYPE_CFG) == 1;
            allowModifyingOfStarted = ConfigUtil.GetIntConfig(ALLOW_MODIFYING_OF_STARTED_CFG) == 1;
        }
    }
}
