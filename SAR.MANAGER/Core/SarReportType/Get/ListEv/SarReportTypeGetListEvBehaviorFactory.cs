using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportType.Get.ListEv
{
    class SarReportTypeGetListEvBehaviorFactory
    {
        internal static ISarReportTypeGetListEv MakeISarReportTypeGetListEv(CommonParam param, object data)
        {
            ISarReportTypeGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SarReportTypeFilterQuery))
                {
                    result = new SarReportTypeGetListEvBehaviorByFilterQuery(param, (SarReportTypeFilterQuery)data);
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
