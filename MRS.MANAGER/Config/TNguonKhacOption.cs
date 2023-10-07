using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
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
                    TNguonKhacOption = ConfigUtil.GetStrConfig(T_NGUONKHAC_OPTION);
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
    }
}
