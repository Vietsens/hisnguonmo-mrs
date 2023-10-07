using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Core.SarReport.Get.Ev;
using SAR.MANAGER.Core.SarReport.Get.ListEv;
using SAR.MANAGER.Core.SarReport.Get.ListV;
using SAR.MANAGER.Core.SarReport.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReport
{
    partial class SarReportGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SarReportGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SAR_REPORT>))
                {
                    ISarReportGetListEv behavior = SarReportGetListEvBehaviorFactory.MakeISarReportGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SAR_REPORT))
                {
                    ISarReportGetEv behavior = SarReportGetEvBehaviorFactory.MakeISarReportGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_SAR_REPORT>))
                {
                    ISarReportGetListV behavior = SarReportGetListVBehaviorFactory.MakeISarReportGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_SAR_REPORT))
                {
                    ISarReportGetV behavior = SarReportGetVBehaviorFactory.MakeISarReportGetV(param, entity);
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
