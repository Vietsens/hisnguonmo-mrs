using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SDA.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static Dictionary<Type, List<string>> properties = new Dictionary<Type, List<string>>();
        private static bool isLoaded = false;
        private static void Load()
        {
            try
            {
                if (!isLoaded)
                {
                    LoadSdaCommune();
                    LoadSdaConfig();
                    LoadSdaConfigApp();
                    LoadSdaConfigAppUser();
                    LoadSdaDistrict();
                    LoadSdaEthnic();
                    LoadSdaEventLog();
                    LoadSdaGroup();
                    LoadSdaGroupType();
                    LoadSdaLanguage();
                    LoadSdaLicense();
                    LoadSdaNational();
                    LoadSdaNotify();
                    LoadSdaProvince();
                    LoadSdaReligion();
                    LoadSdaTranslate();
                    LoadSdaTrouble();

                    isLoaded = true;
                }
            }
            catch (Exception)
            {

            }
        }

        public static List<string> Get<RAW>()
        {
            try
            {
                Load();
                if (properties.ContainsKey(typeof(RAW)))
                {
                    return properties[typeof(RAW)];
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
