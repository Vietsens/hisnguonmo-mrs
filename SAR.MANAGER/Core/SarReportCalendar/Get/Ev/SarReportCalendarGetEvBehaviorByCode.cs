using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportCalendar.Get.Ev
{
    class SarReportCalendarGetEvBehaviorByCode : BeanObjectBase, ISarReportCalendarGetEv
    {
        string code;

        internal SarReportCalendarGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SAR_REPORT_CALENDAR ISarReportCalendarGetEv.Run()
        {
            try
            {
                return DAOWorker.SarReportCalendarDAO.GetByCode(code, new SarReportCalendarFilterQuery().Query());
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
