using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Core.SarReportCalendar.Get.Ev;
using SAR.MANAGER.Core.SarReportCalendar.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReportCalendar
{
    partial class SarReportCalendarGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SarReportCalendarGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SAR_REPORT_CALENDAR>))
                {
                    ISarReportCalendarGetListEv behavior = SarReportCalendarGetListEvBehaviorFactory.MakeISarReportCalendarGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SAR_REPORT_CALENDAR))
                {
                    ISarReportCalendarGetEv behavior = SarReportCalendarGetEvBehaviorFactory.MakeISarReportCalendarGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = default(T);
            }
            return result;
        }
    }
}
