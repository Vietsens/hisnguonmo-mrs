using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReportCalendar.Get.ListEv
{
    class SarReportCalendarGetListEvBehaviorByFilterQuery : BeanObjectBase, ISarReportCalendarGetListEv
    {
        SarReportCalendarFilterQuery filterQuery;

        internal SarReportCalendarGetListEvBehaviorByFilterQuery(CommonParam param, SarReportCalendarFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SAR_REPORT_CALENDAR> ISarReportCalendarGetListEv.Run()
        {
            try
            {
                return DAOWorker.SarReportCalendarDAO.Get(filterQuery.Query(), param);
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
