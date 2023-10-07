using System;
using System.Reflection;

namespace SAR.EFMODEL.Decorator
{
    public class AppModifierDecorator
    {
        public static void Set<RAW>(RAW raw)
        {
            try
            {
                PropertyInfo pi = typeof(RAW).GetProperty("APP_MODIFIER");

                pi.SetValue(raw, "SAR");
            }
            catch (Exception)
            {
                
            }
        }
    }
}
