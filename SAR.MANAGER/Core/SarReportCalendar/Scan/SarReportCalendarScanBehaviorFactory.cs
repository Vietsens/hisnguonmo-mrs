using SAR.EFMODEL.DataModels;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportCalendar.Scan
{
    class SarReportCalendarScanBehaviorFactory
    {
        internal static ISarReportCalendarScan MakeISarReportCalendarScan(CommonParam param)
        {
            ISarReportCalendarScan result = null;
            try
            {
                result = new SarReportCalendarScanBehavior(param);

                if (result == null) throw new NullReferenceException();
            }
            catch (NullReferenceException ex)
            {
                MANAGER.Base.BugUtil.SetBugCode(param, LibraryBug.Bug.Enum.Common__FactoryKhoiTaoDoiTuongThatBai);
                Inventec.Common.Logging.LogSystem.Error("Factory khong khoi tao duoc doi tuong.", ex);
                result = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
