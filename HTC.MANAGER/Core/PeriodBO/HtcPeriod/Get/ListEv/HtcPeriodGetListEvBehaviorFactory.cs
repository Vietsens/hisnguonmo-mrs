using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriod.Get.ListEv
{
    class HtcPeriodGetListEvBehaviorFactory
    {
        internal static IHtcPeriodGetListEv MakeIHtcPeriodGetListEv(CommonParam param, object data)
        {
            IHtcPeriodGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(HtcPeriodFilterQuery))
                {
                    result = new HtcPeriodGetListEvBehaviorByFilterQuery(param, (HtcPeriodFilterQuery)data);
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
