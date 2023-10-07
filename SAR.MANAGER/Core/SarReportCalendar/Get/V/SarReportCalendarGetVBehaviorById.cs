using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportCalendar.Get.V
{
    class SarReportCalendarGetVBehaviorById : BeanObjectBase, ISarReportCalendarGetV
    {
        long id;

        internal SarReportCalendarGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_SAR_REPORT_CALENDAR ISarReportCalendarGetV.Run()
        {
            try
            {
                return DAOWorker.SarReportCalendarDAO.GetViewById(id, new SarReportCalendarViewFilterQuery().Query());
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
