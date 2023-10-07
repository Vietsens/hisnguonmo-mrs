using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.MANAGER.Config
{
    public class HisHeinBhytCFG
    {
        //Cac option cau hinh cho phep tao nhieu ho so dieu tri trong 1 ngay
        public enum Calc2thExamDiffPriceOption
        {
            //con so tien BN phai tra = so tien dich vu - so tien BHYT chi tra
            NORMAL = 0,
            //so tien chenh lech cua dich vu thu 2 duoc tinh dung bang so tien 
            //chenh lech cua dich vu do trong truong hop dich vu do duoc kham lan 1
            FIRST_DIFF_1 = 1,
            //so tien chenh lech cua dich vu thu 2 duoc tinh dung bang so tien chenh lech cua dich vu thu nhat
            FIRST_DIFF_2 = 2
        }

        //Co ap dung cong thuc tinh tien cong kham lan 2 trong ngay doi voi cac ho so dieu tri khac cua cung BN hay khong
        private const string CALC_2TH_EXAM_FOR_OTHER_TREATMENT_CFG = "MOS.HEIN_BHYT.CALC_2TH_EXAM_IN_DAY_FOR_OTHER_TREATMENT";
        //Lua chon cach tinh gia chenh lech cua cong kham thu 2
        private const string CALC_2TH_EXAM_DIFF_PRICE_OPTION_CFG = "MOS.HEIN_BHYT.CALC_2TH_EXAM_DIFF_PRICE_OPTION";

        private const string BHYT_NDS_ICD_CODE__OTHER_CFG = "MRS.HIS_REPORT_BHYT_NDS_ICD_CODE__OTHER";
        private const string BHYT_NDS_ICD_CODE__TE_CFG = "MRS.HIS_REPORT_BHYT_NDS_ICD_CODE__TE";

        private static bool? calc2thExamForOtherTreatment;
        public static bool CALC_2TH_EXAM_FOR_OTHER_TREATMENT
        {
            get
            {
                if (!calc2thExamForOtherTreatment.HasValue)
                {
                    calc2thExamForOtherTreatment = ConfigUtil.GetIntConfig(CALC_2TH_EXAM_FOR_OTHER_TREATMENT_CFG) == 1;
                }
                return calc2thExamForOtherTreatment.Value;
            }
            set
            {
                calc2thExamForOtherTreatment = value;
            }
        }

        private static List<string> bhytNdsIcdCodeTe;
        public static List<string> BHYT_NDS_ICD_CODE__TE
        {
            get
            {
                if (bhytNdsIcdCodeTe != null)
                {
                    bhytNdsIcdCodeTe = ConfigUtil.GetStrConfigs(BHYT_NDS_ICD_CODE__TE_CFG);
                }
                return bhytNdsIcdCodeTe;
            }
            set
            {
                bhytNdsIcdCodeTe = value;
            }
        }

        private static List<string> bhytNdsIcdCodeOther;
        public static List<string> BHYT_NDS_ICD_CODE__OTHER
        {
            get
            {
                if (bhytNdsIcdCodeOther != null)
                {
                    bhytNdsIcdCodeOther = ConfigUtil.GetStrConfigs(BHYT_NDS_ICD_CODE__OTHER_CFG);
                }
                return bhytNdsIcdCodeOther;
            }
            set
            {
                bhytNdsIcdCodeOther = value;
            }
        }

        private static int? calc2thExamDiffPriceOption;
        public static int CALC_2TH_EXAM_DIFF_PRICE_OPTION
        {
            get
            {
                if (!calc2thExamDiffPriceOption.HasValue)
                {
                    calc2thExamDiffPriceOption = ConfigUtil.GetIntConfig(CALC_2TH_EXAM_DIFF_PRICE_OPTION_CFG);
                }
                return calc2thExamDiffPriceOption.Value;
            }
            set
            {
                calc2thExamDiffPriceOption = value;
            }
        }

        public static void Reload()
        {
            calc2thExamForOtherTreatment = ConfigUtil.GetIntConfig(CALC_2TH_EXAM_FOR_OTHER_TREATMENT_CFG) == 1;
            bhytNdsIcdCodeTe = ConfigUtil.GetStrConfigs(BHYT_NDS_ICD_CODE__TE_CFG);
            bhytNdsIcdCodeOther = ConfigUtil.GetStrConfigs(BHYT_NDS_ICD_CODE__OTHER_CFG);
            calc2thExamDiffPriceOption = ConfigUtil.GetIntConfig(CALC_2TH_EXAM_DIFF_PRICE_OPTION_CFG);
        }
    }
}
