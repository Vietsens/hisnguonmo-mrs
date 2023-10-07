using System;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public class ModifierDecorator
    {
        public static void Set<RAW>(RAW raw, string modifier)
        {
            try
            {
                PropertyInfo piModifier = typeof(RAW).GetProperty("MODIFIER");

                piModifier.SetValue(raw, modifier);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
