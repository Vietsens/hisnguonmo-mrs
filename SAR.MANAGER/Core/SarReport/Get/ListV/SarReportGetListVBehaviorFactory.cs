using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport.Get.ListV
{
    class SarReportGetListVBehaviorFactory
    {
        internal static ISarReportGetListV MakeISarReportGetListV(CommonParam param, object data)
        {
            ISarReportGetListV result = null;
            try
            {
                if (data.GetType() == typeof(SarReportViewFilterQuery))
                {
                    result = new SarReportGetListVBehaviorByViewFilterQuery(param, (SarReportViewFilterQuery)data);
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
