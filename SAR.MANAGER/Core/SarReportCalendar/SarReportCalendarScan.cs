using SAR.MANAGER.Core.SarReportCalendar.Scan;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportCalendar
{
    partial class SarReportCalendarScan : BeanObjectBase, IDelegacy
    {
        internal SarReportCalendarScan(CommonParam param)
            : base(param)
        {
        }

        bool IDelegacy.Execute()
        {
            bool result = false;
            try
            {
                ISarReportCalendarScan behavior = SarReportCalendarScanBehaviorFactory.MakeISarReportCalendarScan(param);
                result = behavior != null ? behavior.Run() : false;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}
