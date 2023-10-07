using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportCalendar.Get.V
{
    class SarReportCalendarGetVBehaviorByCode : BeanObjectBase, ISarReportCalendarGetV
    {
        string code;

        internal SarReportCalendarGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_SAR_REPORT_CALENDAR ISarReportCalendarGetV.Run()
        {
            try
            {
                return DAOWorker.SarReportCalendarDAO.GetViewByCode(code, new SarReportCalendarViewFilterQuery().Query());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
