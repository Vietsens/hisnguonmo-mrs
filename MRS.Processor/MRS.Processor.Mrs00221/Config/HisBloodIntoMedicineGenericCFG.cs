using MOS.MANAGER.HisBlood;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00221.Config
{
    class HisBloodIntoMedicineGenericCFG
    {
        private const string CONFIG_KEY = "MRS.HEIN_SERVICE_TYPE.BLOOD_INTO_MEDICINE_LINE_GENERIC";

        private static bool? bloodIntoMedicineLine__Generic;
        public static bool BloodIntoMedicine__Generic
        {
            get
            {
                if (!bloodIntoMedicineLine__Generic.HasValue)
                {
                    bloodIntoMedicineLine__Generic = GetBool(CONFIG_KEY);
                }
                return bloodIntoMedicineLine__Generic.Value;
            }
        }

        private static bool GetBool(string code)
        {
            bool result = false;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                result = (value == "1");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}
