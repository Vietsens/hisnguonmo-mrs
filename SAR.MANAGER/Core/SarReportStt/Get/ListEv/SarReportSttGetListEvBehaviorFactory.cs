using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportStt.Get.ListEv
{
    class SarReportSttGetListEvBehaviorFactory
    {
        internal static ISarReportSttGetListEv MakeISarReportSttGetListEv(CommonParam param, object data)
        {
            ISarReportSttGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SarReportSttFilterQuery))
                {
                    result = new SarReportSttGetListEvBehaviorByFilterQuery(param, (SarReportSttFilterQuery)data);
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
