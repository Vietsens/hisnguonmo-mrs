using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport.Get.ListEv
{
    class SarReportGetListEvBehaviorFactory
    {
        internal static ISarReportGetListEv MakeISarReportGetListEv(CommonParam param, object data)
        {
            ISarReportGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SarReportFilterQuery))
                {
                    result = new SarReportGetListEvBehaviorByFilterQuery(param, (SarReportFilterQuery)data);
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
