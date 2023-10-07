

namespace MOS.MANAGER.Config
{
    class HisPrescriptionCFG
    {
        private const string MUST_EXPORT_BEFORE_OUT_OF_DEPARTMENT_WITH_STAY_IN_PATIENT_CFG = "MOS.HIS_PRESCRIPTION.MUST_EXPORT_BEFORE_OUT_OF_DEPARTMENT_WITH_STAY_IN_PATIENT";
        private const string CHECK_MOBA_NOT_YET_IMPORTED__TIME = "MOS.HIS_PRESCRIPTION.CHECK_MOBA_NOT_YET_IMPORTED.TIME";

        private static bool? mustExportBeforeOutOfDepartmentWithStayInPatient;
        public static bool MUST_EXPORT_BEFORE_OUT_OF_DEPARTMENT_WITH_STAY_IN_PATIENT
        {
            get
            {
                if (!mustExportBeforeOutOfDepartmentWithStayInPatient.HasValue)
                {
                    mustExportBeforeOutOfDepartmentWithStayInPatient = ConfigUtil.GetIntConfig(MUST_EXPORT_BEFORE_OUT_OF_DEPARTMENT_WITH_STAY_IN_PATIENT_CFG) == 1;
                }
                return mustExportBeforeOutOfDepartmentWithStayInPatient.Value;
            }
            set
            {
                mustExportBeforeOutOfDepartmentWithStayInPatient = value;
            }
        }

        private static int? checkMobaNotYetImported_Time;
        public static int CheckMobaNotYetImported_Time
        {
            get
            {
                if (!checkMobaNotYetImported_Time.HasValue)
                {
                    checkMobaNotYetImported_Time = ConfigUtil.GetIntConfig(CHECK_MOBA_NOT_YET_IMPORTED__TIME);
                }
                return checkMobaNotYetImported_Time.Value;
            }
        }

        public static void Reload()
        {
            mustExportBeforeOutOfDepartmentWithStayInPatient = ConfigUtil.GetIntConfig(MUST_EXPORT_BEFORE_OUT_OF_DEPARTMENT_WITH_STAY_IN_PATIENT_CFG) == 1;

            var day = ConfigUtil.GetIntConfig(CHECK_MOBA_NOT_YET_IMPORTED__TIME);
            checkMobaNotYetImported_Time = day;
        }
    }
}
