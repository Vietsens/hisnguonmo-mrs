using System;
using System.Reflection;

namespace ACS.EFMODEL.Decorator
{
    public class GroupCodeDecorator
    {
        public static void Set<RAW>(RAW raw, string groupCode)
        {
            try
            {
                PropertyInfo pi = typeof(RAW).GetProperty("GROUP_CODE");

                pi.SetValue(raw, groupCode);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
