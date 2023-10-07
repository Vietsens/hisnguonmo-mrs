using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportTemplate.Get.Ev
{
    class SarReportTemplateGetEvBehaviorFactory
    {
        internal static ISarReportTemplateGetEv MakeISarReportTemplateGetEv(CommonParam param, object data)
        {
            ISarReportTemplateGetEv result = null;
            try
            {
                if (data.GetType() == typeof(string))
                {
                    result = new SarReportTemplateGetEvBehaviorByCode(param, data.ToString());
                }
                else if (data.GetType() == typeof(long))
                {
                    result = new SarReportTemplateGetEvBehaviorById(param, long.Parse(data.ToString()));
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
