using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReportCalendar.Get.ListV
{
    class SarReportCalendarGetListVBehaviorByViewFilterQuery : BeanObjectBase, ISarReportCalendarGetListV
    {
        SarReportCalendarViewFilterQuery filterQuery;

        internal SarReportCalendarGetListVBehaviorByViewFilterQuery(CommonParam param, SarReportCalendarViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_SAR_REPORT_CALENDAR> ISarReportCalendarGetListV.Run()
        {
            try
            {
                return DAOWorker.SarReportCalendarDAO.GetView(filterQuery.Query(), param);
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
