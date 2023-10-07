using ACS.LibraryBug;
using Inventec.Common.Logging;
using Inventec.Core;
using System;

namespace ACS.MANAGER.Base
{
    public class BugUtil
    {
        public static void SetBugCode(CommonParam param, Bug.Enum bugCaseEnum)
        {
            try
            {
                Bug bug = ACS.LibraryBug.DatabaseBug.Get(bugCaseEnum);
                if (bug != null)
                {
                    param.BugCodes.Add(bug.code);
                }
                else
                {
                    LogSystem.Error("Thu vien chua khai bao key.");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
