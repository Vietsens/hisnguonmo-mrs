using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.RevenueBO.HtcRevenue.Get.Ev
{
    class HtcRevenueGetEvBehaviorFactory
    {
        internal static IHtcRevenueGetEv MakeIHtcRevenueGetEv(CommonParam param, object data)
        {
            IHtcRevenueGetEv result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new HtcRevenueGetEvBehaviorById(param, long.Parse(data.ToString()));
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
