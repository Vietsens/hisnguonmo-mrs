

namespace MOS.MANAGER.Config
{
    class HisSereServCFG
    {
        private const string AUTO_SET_NO_EXECUTE_FOR_REPAY_CFG = "MOS.HIS_SERE_SERV.AUTO_SET_NO_EXECUTE_FOR_REPAY";

        //Ti le tinh tien cong kham thu 2 doi voi BN vien phi (dich vu)
        private const string RATIO_FOR_2ND_EXAM_FOR_HOSPITAL_FEE_CFG = "MOS.HIS_SERE_SERV.RATIO_FOR_2ND_EXAM_FOR_HOSPITAL_FEE";

        private static bool? autoSetNoExecuteForRepay;
        public static bool AUTO_SET_NO_EXECUTE_FOR_REPAY
        {
            get
            {
                if (!autoSetNoExecuteForRepay.HasValue)
                {
                    autoSetNoExecuteForRepay = ConfigUtil.GetIntConfig(AUTO_SET_NO_EXECUTE_FOR_REPAY_CFG) == 1;
                }
                return autoSetNoExecuteForRepay.Value;
            }
            set
            {
                autoSetNoExecuteForRepay = value;
            }
        }

        private static decimal? ratioFor2ndExamForHospitalFee;
        public static decimal? RATIO_FOR_2ND_EXAM_FOR_HOSPITAL_FEE
        {
            get
            {
                if (!ratioFor2ndExamForHospitalFee.HasValue)
                {
                    ratioFor2ndExamForHospitalFee = ConfigUtil.GetDecimalConfig(RATIO_FOR_2ND_EXAM_FOR_HOSPITAL_FEE_CFG);
                }
                return ratioFor2ndExamForHospitalFee.Value;
            }
            set
            {
                ratioFor2ndExamForHospitalFee = value;
            }
        }

        public static void Reload()
        {
            autoSetNoExecuteForRepay = ConfigUtil.GetIntConfig(AUTO_SET_NO_EXECUTE_FOR_REPAY_CFG) == 1;
            ratioFor2ndExamForHospitalFee = ConfigUtil.GetDecimalConfig(RATIO_FOR_2ND_EXAM_FOR_HOSPITAL_FEE_CFG);
        }
    }
}
