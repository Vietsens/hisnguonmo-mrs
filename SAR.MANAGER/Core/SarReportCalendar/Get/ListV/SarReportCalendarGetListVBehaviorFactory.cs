using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportCalendar.Get.ListV
{
    class SarReportCalendarGetListVBehaviorFactory
    {
        internal static ISarReportCalendarGetListV MakeISarReportCalendarGetListV(CommonParam param, object data)
        {
            ISarReportCalendarGetListV result = null;
            try
            {
                if (data.GetType() == typeof(SarReportCalendarViewFilterQuery))
                {
                    result = new SarReportCalendarGetListVBehaviorByViewFilterQuery(param, (SarReportCalendarViewFilterQuery)data);
                }
                if (result == null) throw new NullReferenceException();
            }
            catch (NullReferenceException ex)
            {
                MANAGER.Base.BugUtil.SetBugCode(param, LibraryBug.Bug.Enum.Common__FactoryKhoiTaoDoiTuongThatBai);
                Inventec.Common.Logging.LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data), ex);
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
