using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportTemplate.Get.ListV
{
    class SarReportTemplateGetListVBehaviorFactory
    {
        internal static ISarReportTemplateGetListV MakeISarReportTemplateGetListV(CommonParam param, object data)
        {
            ISarReportTemplateGetListV result = null;
            try
            {
                if (data.GetType() == typeof(SarReportTemplateViewFilterQuery))
                {
                    result = new SarReportTemplateGetListVBehaviorByViewFilterQuery(param, (SarReportTemplateViewFilterQuery)data);
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
