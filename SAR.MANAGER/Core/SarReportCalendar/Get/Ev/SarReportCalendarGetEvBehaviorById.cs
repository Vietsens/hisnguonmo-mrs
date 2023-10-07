using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportCalendar.Get.Ev
{
    class SarReportCalendarGetEvBehaviorById : BeanObjectBase, ISarReportCalendarGetEv
    {
        long id;

        internal SarReportCalendarGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SAR_REPORT_CALENDAR ISarReportCalendarGetEv.Run()
        {
            try
            {
                return DAOWorker.SarReportCalendarDAO.GetById(id, new SarReportCalendarFilterQuery().Query());
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
