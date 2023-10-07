using System;
using System.Reflection;

namespace ACS.EFMODEL.Decorator
{
    public class IsActiveDecorator
    {
        public static void Set<RAW>(RAW raw)
        {
            try
            {
                PropertyInfo pi = typeof(RAW).GetProperty("IS_ACTIVE");
                if (pi.GetValue(raw) == null)
                {
                    pi.SetValue(raw, (short)1);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
