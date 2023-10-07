

namespace MOS.MANAGER.Config
{
    class HisTreatmentCFG
    {
        //Cac option cau hinh cho phep tao nhieu ho so dieu tri trong 1 ngay
        public enum ManyTreatmentPerDayOption
        {
            ALLOW = 1, //cho phep
            EMERGENCY = 2, //chi cho phep voi loai la cap cuu
            NO_2_BHYT = 3, //Khong cho phep co 2 ho so la BHYT trong 1 ngay
            NOT_ALLOW = 4, //khong cho phep
        }

        //Tu dong nhan dien khoa don tiep su dung phong xu ly kham dau tien
        private const string IS_AUTO_DETECT_RECEIVING_DEPARTMENT_BY_FIRST_EXAM_EXECUTE_ROOM_CFG = "MOS.IS_AUTO_DETECT_RECEIVING_DEPARTMENT_BY_FIRST_EXAM_EXECUTE_ROOM";
        //Chon cau hinh cho phep tao nhieu ho so dieu tri trong 1 ngay tuong ung voi 1 benh nhan hay khong
        private const string MANY_TREATMENT_PER_DAY_OPTION_CFG = "MOS.MANY_TREATMENT_PER_DAY_OPTION";
        private const string MUST_FINISH_ALL_SERVICES_BEFORE_FINISH_TREATMENT_CFG = "MOS.HIS_TREATMENT.MUST_FINISH_ALL_SERVICES_BEFORE_FINISH_TREATMENT";
        private const string AUTO_LOCK_AFTER_BILL_CFG = "MOS.HIS_TREATMENT.AUTO_LOCK_AFTER_BILL";
        private const string AUTO_LOCK_AFTER_HEIN_APPROVAL_CFG = "MOS.HIS_TREATMENT.AUTO_LOCK_AFTER_HEIN_APPROVAL";
        private const string AUTO_HEIN_APPROVAL_AFTER_FEE_LOCK_CFG = "MOS.HIS_TREATMENT.AUTO_HEIN_APPROVAL_AFTER_FEE_LOCK";
        private const string STORE_CODE_OPTION_CFG = "MOS.HIS_TREATMENT.STORE_CODE_OPTION";
        private const string IS_CHECK_PREVIOUS_DEBT_CFG = "MOS.HIS_TREATMENT.IS_CHECK_PREVIOUS_DEBT";
        private const string IS_CHECK_PREVIOUS_PRESCRIPTION_CFG = "MOS.HIS_TREATMENT.IS_CHECK_PREVIOUS_PRESCRIPTION";

        private static bool? isAutoDetectReceivingDepartmentByFirstExamExecuteRoom;
        public static bool IS_AUTO_DETECT_RECEIVING_DEPARTMENT_BY_FIRST_EXAM_EXECUTE_ROOM
        {
            get
            {
                if (!isAutoDetectReceivingDepartmentByFirstExamExecuteRoom.HasValue)
                {
                    isAutoDetectReceivingDepartmentByFirstExamExecuteRoom = ConfigUtil.GetIntConfig(IS_AUTO_DETECT_RECEIVING_DEPARTMENT_BY_FIRST_EXAM_EXECUTE_ROOM_CFG) == 1;
                }
                return isAutoDetectReceivingDepartmentByFirstExamExecuteRoom.Value;
            }
            set
            {
                isAutoDetectReceivingDepartmentByFirstExamExecuteRoom = value;
            }
        }

        private static int? manyTreatmentPerDayOption;
        public static int MANY_TREATMENT_PER_DAY_OPTION
        {
            get
            {
                if (!manyTreatmentPerDayOption.HasValue)
                {
                    manyTreatmentPerDayOption = ConfigUtil.GetIntConfig(MANY_TREATMENT_PER_DAY_OPTION_CFG);
                }
                return manyTreatmentPerDayOption.Value;
            }
            set
            {
                manyTreatmentPerDayOption = value;
            }
        }

        private static int? storeCodeOption;
        public static int STORE_CODE_OPTION
        {
            get
            {
                if (!storeCodeOption.HasValue)
                {
                    storeCodeOption = ConfigUtil.GetIntConfig(STORE_CODE_OPTION_CFG);
                }
                return storeCodeOption.Value;
            }
            set
            {
                storeCodeOption = value;
            }
        }

        private static bool? mustFinishAllServicesBeforeFinishTreatment;
        public static bool MUST_FINISH_ALL_SERVICES_BEFORE_FINISH_TREATMENT
        {
            get
            {
                if (!mustFinishAllServicesBeforeFinishTreatment.HasValue)
                {
                    mustFinishAllServicesBeforeFinishTreatment = ConfigUtil.GetIntConfig(MUST_FINISH_ALL_SERVICES_BEFORE_FINISH_TREATMENT_CFG) == 1;
                }
                return mustFinishAllServicesBeforeFinishTreatment.Value;
            }
            set
            {
                mustFinishAllServicesBeforeFinishTreatment = value;
            }
        }

        private static bool? isCheckPreviousDebt;
        public static bool IS_CHECK_PREVIOUS_DEBT
        {
            get
            {
                if (!isCheckPreviousDebt.HasValue)
                {
                    isCheckPreviousDebt = ConfigUtil.GetIntConfig(IS_CHECK_PREVIOUS_DEBT_CFG) == 1;
                }
                return isCheckPreviousDebt.Value;
            }
            set
            {
                isCheckPreviousDebt = value;
            }
        }

        private static bool? isCheckPreviousPrescription;
        public static bool IS_CHECK_PREVIOUS_PRESCRIPTION
        {
            get
            {
                if (!isCheckPreviousPrescription.HasValue)
                {
                    isCheckPreviousPrescription = ConfigUtil.GetIntConfig(IS_CHECK_PREVIOUS_PRESCRIPTION_CFG) == 1;
                }
                return isCheckPreviousPrescription.Value;
            }
            set
            {
                isCheckPreviousPrescription = value;
            }
        }

        private static bool? autoLockAfterBill;
        public static bool AUTO_LOCK_AFTER_BILL
        {
            get
            {
                if (!autoLockAfterBill.HasValue)
                {
                    autoLockAfterBill = ConfigUtil.GetIntConfig(AUTO_LOCK_AFTER_BILL_CFG) == 1;
                }
                return autoLockAfterBill.Value;
            }
            set
            {
                autoLockAfterBill = value;
            }
        }

        private static bool? autoLockAfterHeinApproval;
        public static bool AUTO_LOCK_AFTER_HEIN_APPROVAL
        {
            get
            {
                if (!autoLockAfterHeinApproval.HasValue)
                {
                    autoLockAfterHeinApproval = ConfigUtil.GetIntConfig(AUTO_LOCK_AFTER_HEIN_APPROVAL_CFG) == 1;
                }
                return autoLockAfterHeinApproval.Value;
            }
            set
            {
                autoLockAfterHeinApproval = value;
            }
        }

        private static bool? autoHeinApprovalAfterFeeLock;
        public static bool AUTO_HEIN_APPROVAL_AFTER_FEE_LOCK
        {
            get
            {
                if (!autoHeinApprovalAfterFeeLock.HasValue)
                {
                    autoHeinApprovalAfterFeeLock = ConfigUtil.GetIntConfig(AUTO_HEIN_APPROVAL_AFTER_FEE_LOCK_CFG) == 1;
                }
                return autoHeinApprovalAfterFeeLock.Value;
            }
            set
            {
                autoHeinApprovalAfterFeeLock = value;
            }
        }

        public static void Reload()
        {
            isAutoDetectReceivingDepartmentByFirstExamExecuteRoom = ConfigUtil.GetIntConfig(IS_AUTO_DETECT_RECEIVING_DEPARTMENT_BY_FIRST_EXAM_EXECUTE_ROOM_CFG) == 1;
            manyTreatmentPerDayOption = ConfigUtil.GetIntConfig(MANY_TREATMENT_PER_DAY_OPTION_CFG);
            storeCodeOption = ConfigUtil.GetIntConfig(STORE_CODE_OPTION_CFG);
            mustFinishAllServicesBeforeFinishTreatment = ConfigUtil.GetIntConfig(MUST_FINISH_ALL_SERVICES_BEFORE_FINISH_TREATMENT_CFG) == 1;
            isCheckPreviousDebt = ConfigUtil.GetIntConfig(IS_CHECK_PREVIOUS_DEBT_CFG) == 1;
            isCheckPreviousPrescription = ConfigUtil.GetIntConfig(IS_CHECK_PREVIOUS_PRESCRIPTION_CFG) == 1;
            autoLockAfterBill = ConfigUtil.GetIntConfig(AUTO_LOCK_AFTER_BILL_CFG) == 1;
            autoLockAfterHeinApproval = ConfigUtil.GetIntConfig(AUTO_LOCK_AFTER_HEIN_APPROVAL_CFG) == 1;
            autoHeinApprovalAfterFeeLock = ConfigUtil.GetIntConfig(AUTO_HEIN_APPROVAL_AFTER_FEE_LOCK_CFG) == 1;
        }
    }
}
