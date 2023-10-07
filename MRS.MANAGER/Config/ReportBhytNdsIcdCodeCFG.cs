using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class ReportBhytNdsIcdCodeCFG
    {
        private static string HIS_REPORT_BHYT_NDS_ICD_CODE__TE = "MRS.HIS_REPORT_BHYT_NDS_ICD_CODE__TE";
        private static string HIS_REPORT_BHYT_NDS_ICD_CODE__OTHER = "MRS.HIS_REPORT_BHYT_NDS_ICD_CODE__OTHER";

        private static List<string> reportBhytNdsIcdCode__Te;
        public static List<string> ReportBhytNdsIcdCode__Te
        {
            get
            {
                if (reportBhytNdsIcdCode__Te == null || reportBhytNdsIcdCode__Te.Count == 0)
                {
                    reportBhytNdsIcdCode__Te = Get(HIS_REPORT_BHYT_NDS_ICD_CODE__TE);
                }
                return reportBhytNdsIcdCode__Te;
            }
        }

        private static List<string> reportBhytNdsIcdCode__Other;
        public static List<string> ReportBhytNdsIcdCode__Other
        {
            get
            {
                if (reportBhytNdsIcdCode__Other == null || reportBhytNdsIcdCode__Other.Count == 0)
                {
                    reportBhytNdsIcdCode__Other = Get(HIS_REPORT_BHYT_NDS_ICD_CODE__OTHER);
                }
                return reportBhytNdsIcdCode__Other;
            }
        }

        private static List<string> Get(string code)
        {
            List<string> result = new List<string>();
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                result.AddRange(value.Split(new char[] { ',' }));
                if (result == null) throw new ArgumentNullException(code + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => result), result));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<string>();
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                reportBhytNdsIcdCode__Te = null;
                reportBhytNdsIcdCode__Other = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
