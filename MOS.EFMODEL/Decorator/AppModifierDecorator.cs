using System;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public class AppModifierDecorator
    {
        public static void Set<RAW>(RAW raw)
        {
            try
            {
                PropertyInfo pi = typeof(RAW).GetProperty("APP_MODIFIER");

                pi.SetValue(raw, "MOS");
            }
            catch (Exception)
            {
                
            }
        }
    }
}
