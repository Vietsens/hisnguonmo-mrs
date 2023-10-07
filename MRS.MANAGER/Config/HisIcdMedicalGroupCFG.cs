using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.Logging;
using SDA.EFMODEL.DataModels;
using MOS.MANAGER.HisIcd;

namespace MRS.MANAGER.Config
{
    public class HisIcdMedicalGroupCFG
    {
        private static string HIS_ICD_ICD_CODE__GROUP_MRS00136_01 = "MRS.HIS_ICD.HIS_ICD_ICD_CODE__GROUP_MRS00136_01";//Hô hấp (Phổi màng phổi)
        private static string HIS_ICD_ICD_CODE__GROUP_MRS00136_02 = "MRS.HIS_ICD.HIS_ICD_ICD_CODE__GROUP_MRS00136_02";//Dạng lao khác (thận, màng não, hạch, ...)
        private static string HIS_ICD_ICD_CODE__GROUP_MRS00136_03 = "MRS.HIS_ICD.HIS_ICD_ICD_CODE__GROUP_MRS00136_03";//Di chứng lao (tâm phế mạn)
        private static string HIS_ICD_ICD_CODE__GROUP_MRS00136_04 = "MRS.HIS_ICD.HIS_ICD_ICD_CODE__GROUP_MRS00136_04";//U phổi, phế quản, viêm phổi
        private static string HIS_ICD_ICD_CODE__GROUP_MRS00136_05 = "MRS.HIS_ICD.HIS_ICD_ICD_CODE__GROUP_MRS00136_05";//Viêm phế quản, viêm phổi
        private static string HIS_ICD_ICD_CODE__GROUP_MRS00136_06 = "MRS.HIS_ICD.HIS_ICD_ICD_CODE__GROUP_MRS00136_06";//Giãn phế quản, hen phế quản, COPD
        private static string HIS_ICD_ICD_CODE__GROUP_MRS00136_07 = "MRS.HIS_ICD.HIS_ICD_ICD_CODE__GROUP_MRS00136_07";//Bệnh khác của đường hô hấp (viêm hạch, viêm khớp, viêm đại tràng, ...)
        private static string MRS_AGE_CHILDREN = "MRS_AGE_CHILDREN";//Độ tuổi trẻ em
        private static string HIS_ICD_ICD_CODE__GROUP_MRS00439_BORN = "MRS.HIS_ICD.HIS_ICD_ICD_CODE__GROUP_MRS00439_BORN";//Chẩn đoán đi đẻ

        private static List<long> reportMedicalGroup_Born;
        public static List<long> ReportMdeicalGroup_Born
        {
            get
            {
                if (reportMedicalGroup_Born == null || reportMedicalGroup_Born.Count == 0)
                {
                    reportMedicalGroup_Born = GetListId(HIS_ICD_ICD_CODE__GROUP_MRS00439_BORN);
                }
                return reportMedicalGroup_Born;
            }
            set
            {
                reportMedicalGroup_Born = value;
            }
        }

        private static long? reportAgeChildren;
        public static long? ReportAgeChildren
        {
            get
            {
                if (reportAgeChildren == null)
                {
                    reportAgeChildren = long.Parse(GetListValue(MRS_AGE_CHILDREN).FirstOrDefault());
                }
                return reportAgeChildren;
            }
        }

        private static List<string> reportMdeicalGroup_07;
        public static List<string> ReportMdeicalGroup_07
        {
            get
            {
                if (reportMdeicalGroup_07 == null || reportMdeicalGroup_07.Count == 0)
                {
                    reportMdeicalGroup_07 = GetListValue(HIS_ICD_ICD_CODE__GROUP_MRS00136_07);
                }
                return reportMdeicalGroup_07;
            }
        }

        private static List<string> reportMdeicalGroup_06;
        public static List<string> ReportMdeicalGroup_06
        {
            get
            {
                if (reportMdeicalGroup_06 == null || reportMdeicalGroup_06.Count == 0)
                {
                    reportMdeicalGroup_06 = GetListValue(HIS_ICD_ICD_CODE__GROUP_MRS00136_06);
                }
                return reportMdeicalGroup_06;
            }
        }

        private static List<string> reportMdeicalGroup_05;
        public static List<string> ReportMdeicalGroup_05
        {
            get
            {
                if (reportMdeicalGroup_05 == null || reportMdeicalGroup_05.Count == 0)
                {
                    reportMdeicalGroup_05 = GetListValue(HIS_ICD_ICD_CODE__GROUP_MRS00136_05);
                }
                return reportMdeicalGroup_05;
            }
        }

        private static List<string> reportMdeicalGroup_04;
        public static List<string> ReportMdeicalGroup_04
        {
            get
            {
                if (reportMdeicalGroup_04 == null || reportMdeicalGroup_04.Count == 0)
                {
                    reportMdeicalGroup_04 = GetListValue(HIS_ICD_ICD_CODE__GROUP_MRS00136_04);
                }
                return reportMdeicalGroup_04;
            }
        }

        private static List<string> reportMdeicalGroup_03;
        public static List<string> ReportMdeicalGroup_03
        {
            get
            {
                if (reportMdeicalGroup_03 == null || reportMdeicalGroup_03.Count == 0)
                {
                    reportMdeicalGroup_03 = GetListValue(HIS_ICD_ICD_CODE__GROUP_MRS00136_03);
                }
                return reportMdeicalGroup_03;
            }
        }

        private static List<string> reportMdeicalGroup_02;
        public static List<string> ReportMdeicalGroup_02
        {
            get
            {
                if (reportMdeicalGroup_02 == null || reportMdeicalGroup_02.Count == 0)
                {
                    reportMdeicalGroup_02 = GetListValue(HIS_ICD_ICD_CODE__GROUP_MRS00136_02);
                }
                return reportMdeicalGroup_02;
            }
        }

        private static List<string> reportMdeicalGroup_01;
        public static List<string> ReportMdeicalGroup_01
        {
            get
            {
                if (reportMdeicalGroup_01 == null || reportMdeicalGroup_01.Count == 0)
                {
                    reportMdeicalGroup_01 = GetListValue(HIS_ICD_ICD_CODE__GROUP_MRS00136_01);
                }
                return reportMdeicalGroup_01;
            }
        }

        private static List<long> GetListId(string code)
        {
            var result = new List<long>();
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                var value = string.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                var arr = value.Split(',');
                foreach (var s in arr)
                {
                    if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                    var filter = new HisIcdFilterQuery();
                    var data = new HisIcdManager().Get(filter).FirstOrDefault(o => o.ICD_CODE == s);
                    if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                    result.Add(data.ID);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private static List<string> GetListValue(string code)
        {
            var result = new List<string>();
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                var value = string.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                var arr = value.Split(',');
                result.AddRange(arr);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                reportMedicalGroup_Born = null;
                reportAgeChildren = null;
                reportMdeicalGroup_07 = null;
                reportMdeicalGroup_06 = null;
                reportMdeicalGroup_05 = null;
                reportMdeicalGroup_04 = null;
                reportMdeicalGroup_03 = null;
                reportMdeicalGroup_02 = null;
                reportMdeicalGroup_01 = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
