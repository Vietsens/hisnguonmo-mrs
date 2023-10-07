using MOS.MANAGER.HisIcd;
using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00338.Config
{
    class HisIcdCFG
    {
        private const string HIS_ICD__ICD_CODE__EXTEND = "MRS.HIS_ICD.ICD_CODE.EXTEND";

        private static List<string> hisIcdCODE__Extends;
        public static List<string> HisIcdCODE_Extends
        {
            get
            {
                if (hisIcdCODE__Extends==null)
                {
                    hisIcdCODE__Extends = GetCodes(HIS_ICD__ICD_CODE__EXTEND);
                }
                return hisIcdCODE__Extends;
            }
        }


        private static List<string> GetCodes(string ConfigKey)
        {
            List<string> result = null;
            try
            {
                var config = Loader.dictionaryConfig[ConfigKey];
                if (config == null) throw new ArgumentNullException(ConfigKey);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(ConfigKey);
                result = value.Split(',').ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
