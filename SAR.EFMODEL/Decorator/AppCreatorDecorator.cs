using System;
using System.Reflection;

namespace SAR.EFMODEL.Decorator
{
    public class AppCreatorDecorator
    {
        public static void Set<RAW>(RAW raw)
        {
            try
            {
                PropertyInfo piCreator = typeof(RAW).GetProperty("APP_CREATOR");
                PropertyInfo piModifier = typeof(RAW).GetProperty("APP_MODIFIER");

                piCreator.SetValue(raw, "SAR");
                piModifier.SetValue(raw, "SAR");
            }
            catch (Exception)
            {
                
            }
        }
    }
}
