using Inventec.Common.Logging;
using Inventec.Core;
using MOS.LibraryBug;
using System;

namespace MOS.MANAGER.Base
{
    class BugUtil
    {
        public static void SetBugCode(CommonParam param, Bug.Enum bugCaseEnum)
        {
            try
            {
                Bug bug = MOS.LibraryBug.DatabaseBug.Get(bugCaseEnum);
                if (bug != null && !param.BugCodes.Contains(bug.code))
                {
                    param.BugCodes.Add(bug.code);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error("Co exception khi SetBugCode.", ex);
            }
        }
    }
}
