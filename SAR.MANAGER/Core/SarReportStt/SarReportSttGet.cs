using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Core.SarReportStt.Get.Ev;
using SAR.MANAGER.Core.SarReportStt.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReportStt
{
    partial class SarReportSttGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SarReportSttGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SAR_REPORT_STT>))
                {
                    ISarReportSttGetListEv behavior = SarReportSttGetListEvBehaviorFactory.MakeISarReportSttGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SAR_REPORT_STT))
                {
                    ISarReportSttGetEv behavior = SarReportSttGetEvBehaviorFactory.MakeISarReportSttGetEv(param, entity);
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
