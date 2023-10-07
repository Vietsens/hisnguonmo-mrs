using System;
using System.Reflection;

namespace SDA.EFMODEL.Decorator
{
    public class AppModifierDecorator
    {
        public static void Set<RAW>(RAW raw)
        {
            try
            {
                PropertyInfo pi = typeof(RAW).GetProperty("APP_MODIFIER");

                pi.SetValue(raw, "SDA");
            }
            catch (Exception)
            {
                
            }
        }
    }
}
