using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class NumDigitsOptionCFG
    {
        private const string NUM_DIGITS_OPTION = "XML.EXPORT.4210.NUM_DIGITS_OPTION";

        private static string NumDigitsOptionValue;
        public static string NUM_DIGITS_OPTION_VALUE
        {
            get
            {
                if (NumDigitsOptionValue == null)
                {
                    NumDigitsOptionValue = ConfigUtil.GetStrConfig(NUM_DIGITS_OPTION);
                }
                return NumDigitsOptionValue;
            }
            set
            {
                NumDigitsOptionValue = value;
            }
        }

        public static void Refresh()
        {
            try
            {
                NumDigitsOptionValue = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
