using System;
using System.Collections.Generic;

namespace ACS.MANAGER.Base
{
    class TroubleCache
    {
        private static List<string> troubles = new List<string>();

        internal static bool Add(string trouble)
        {
            bool result = false;
            try
            {
                troubles.Add(trouble);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal static List<string> GetAndClear()
        {
            List<string> result = new List<string>();
            try
            {
                result.AddRange(troubles);
                troubles.Clear();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
