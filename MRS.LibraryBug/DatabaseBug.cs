using System;
using System.Collections.Generic;

namespace MRS.LibraryBug
{
    public class DatabaseBug
    {
        private static Dictionary<Bug.Enum, Bug> dic = new Dictionary<Bug.Enum, Bug>();
        private static Object thisLock = new Object();

        public static Bug Get(Bug.Enum en)
        {
            Bug result = null;
            lock (thisLock)
            {
                if (!dic.TryGetValue(en, out result))
                {
                    result = new Bug(en);
                    dic.Add(en, result);
                }
            }
            return result;
        }
    }
}
