using System;
using System.Reflection;

namespace ACS.EFMODEL.Decorator
{
    public class AppCreatorDecorator
    {
        public static void Set<RAW>(RAW raw)
        {
            try
            {
                PropertyInfo piCreator = typeof(RAW).GetProperty("APP_CREATOR");
                PropertyInfo piModifier = typeof(RAW).GetProperty("APP_MODIFIER");

                piCreator.SetValue(raw, "ACS");
                piModifier.SetValue(raw, "ACS");
            }
            catch (Exception)
            {
                
            }
        }
    }
}
