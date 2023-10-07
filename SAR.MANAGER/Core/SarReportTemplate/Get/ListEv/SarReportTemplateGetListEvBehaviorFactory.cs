using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportTemplate.Get.ListEv
{
    class SarReportTemplateGetListEvBehaviorFactory
    {
        internal static ISarReportTemplateGetListEv MakeISarReportTemplateGetListEv(CommonParam param, object data)
        {
            ISarReportTemplateGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SarReportTemplateFilterQuery))
                {
                    result = new SarReportTemplateGetListEvBehaviorByFilterQuery(param, (SarReportTemplateFilterQuery)data);
                }
                if (result == null) throw new NullReferenceException();
            }
            catch (NullReferenceException ex)
            {
                Inventec.Common.Logging.LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data), ex);
                result = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
