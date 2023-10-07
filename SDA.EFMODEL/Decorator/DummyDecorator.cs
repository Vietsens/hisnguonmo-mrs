using System;
using System.Collections.Generic;
using System.Reflection;

namespace SDA.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static Dictionary<Type, List<PropertyInfo>> properties = new Dictionary<Type, List<PropertyInfo>>();
        private static bool isLoaded = false;
        private static void Load()
        {
            try
            {
                if (!isLoaded)
                {
                    LoadSdaGroup();
                    LoadSdaTrouble();

                    isLoaded = true;
                }
            }
            catch (Exception)
            {

            }
        }

        public static void Set<RAW>(RAW raw)
        {
            try
            {
                Load();
                if (properties.ContainsKey(typeof(RAW)))
                {
                    List<PropertyInfo> values = properties[typeof(RAW)];
                    if (values != null && values.Count > 0)
                    {
                        foreach (var property in values)
                        {
                            property.SetValue(raw, "");
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
