using SAR.MANAGER.Core.SarReportCalendar;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Manager
{
    public partial class SarReportCalendarManager : ManagerBase
    {
        public object Scan()
        {
            object result = null;
            try
            {
                SarReportCalendarBO bo = new SarReportCalendarBO();
                if (bo.Scan())
                {
                    //result = data;
                }
                CopyCommonParamInfo(bo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
