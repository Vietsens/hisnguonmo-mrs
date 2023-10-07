using System;

namespace SAR.MANAGER.Core.SarReportCalendar
{
    partial class SarReportCalendarBO : BusinessObjectBase
    {
        internal bool Scan()
        {
            bool result = false;
            try
            {
                IDelegacy delegacy = new SarReportCalendarScan(param);
                result = delegacy.Execute();
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
