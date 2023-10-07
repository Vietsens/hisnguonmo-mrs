using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class MaterialPriceOptionCFG
    {
        private const string MATERIAL_PRICE_OPTION = "XML.EXPORT.4210.MATERIAL_PRICE_OPTION";

        private static string materialPriceOptionValue;
        public static string MATERIAL_PRICE_OPTION_VALUE
        {
            get
            {
                if (materialPriceOptionValue == null)
                {
                    materialPriceOptionValue = ConfigUtil.GetStrConfig(MATERIAL_PRICE_OPTION);
                }
                return materialPriceOptionValue;
            }
            set
            {
                materialPriceOptionValue = value;
            }
        }

        public static void Refresh()
        {
            try
            {
                materialPriceOptionValue = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
