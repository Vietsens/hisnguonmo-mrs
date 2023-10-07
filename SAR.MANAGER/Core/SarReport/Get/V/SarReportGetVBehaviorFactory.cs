using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport.Get.V
{
    class SarReportGetVBehaviorFactory
    {
        internal static ISarReportGetV MakeISarReportGetV(CommonParam param, object data)
        {
            ISarReportGetV result = null;
            try
            {
                if (data.GetType() == typeof(string))
                {
                    result = new SarReportGetVBehaviorByCode(param, data.ToString());
                }
                else if (data.GetType() == typeof(long))
                {
                    result = new SarReportGetVBehaviorById(param, long.Parse(data.ToString()));
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
