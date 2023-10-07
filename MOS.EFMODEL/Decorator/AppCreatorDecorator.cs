using System;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public class AppCreatorDecorator
    {
        public static void Set<RAW>(RAW raw)
        {
            try
            {
                PropertyInfo piCreator = typeof(RAW).GetProperty("APP_CREATOR");
                PropertyInfo piModifier = typeof(RAW).GetProperty("APP_MODIFIER");

                piCreator.SetValue(raw, "MOS");
                piModifier.SetValue(raw, "MOS");
            }
            catch (Exception)
            {
                
            }
        }
    }
}
