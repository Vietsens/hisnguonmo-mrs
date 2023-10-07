using Inventec.Common.Logging;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00065
{
    public class TNguonKhacOptionCFG
    {
        private const string T_NGUONKHAC_OPTION = "XML.EXPORT.4210.T_NGUONKHAC_OPTION";

        private static string TNguonKhacOption;
        public static string XML4210_T_NGUONKHAC_OPTION
        {
            get
            {
                if (TNguonKhacOption == null)
                {
                    TNguonKhacOption = GetStrConfig(T_NGUONKHAC_OPTION);
                }
                return TNguonKhacOption;
            }
            set
            {
                TNguonKhacOption = value;
            }
        }

        public static void Refresh()
        {
            try
            {
                TNguonKhacOption= null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        internal static string GetStrConfig(string code)
        {
            string result = null;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                result = !String.IsNullOrEmpty(config.VALUE) ? config.VALUE : (!String.IsNullOrEmpty(config.DEFAULT_VALUE) ? config.DEFAULT_VALUE : "");
                if (String.IsNullOrEmpty(result)) throw new ArgumentNullException(code);

            }
            catch (Exception ex)
            {
                LogSystem.Error("Loi khi lay Config: " + code, ex);
            }
            return result;
        }
    }
}
