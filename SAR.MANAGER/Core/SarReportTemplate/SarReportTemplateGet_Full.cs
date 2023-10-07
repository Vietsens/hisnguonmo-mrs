using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Core.SarReportTemplate.Get.Ev;
using SAR.MANAGER.Core.SarReportTemplate.Get.ListEv;
using SAR.MANAGER.Core.SarReportTemplate.Get.ListV;
using SAR.MANAGER.Core.SarReportTemplate.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReportTemplate
{
    partial class SarReportTemplateGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SarReportTemplateGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SAR_REPORT_TEMPLATE>))
                {
                    ISarReportTemplateGetListEv behavior = SarReportTemplateGetListEvBehaviorFactory.MakeISarReportTemplateGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SAR_REPORT_TEMPLATE))
                {
                    ISarReportTemplateGetEv behavior = SarReportTemplateGetEvBehaviorFactory.MakeISarReportTemplateGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_SAR_REPORT_TEMPLATE>))
                {
                    ISarReportTemplateGetListV behavior = SarReportTemplateGetListVBehaviorFactory.MakeISarReportTemplateGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_SAR_REPORT_TEMPLATE))
                {
                    ISarReportTemplateGetV behavior = SarReportTemplateGetVBehaviorFactory.MakeISarReportTemplateGetV(param, entity);
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
