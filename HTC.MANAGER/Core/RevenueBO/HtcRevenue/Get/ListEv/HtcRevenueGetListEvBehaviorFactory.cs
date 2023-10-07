using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.RevenueBO.HtcRevenue.Get.ListEv
{
    class HtcRevenueGetListEvBehaviorFactory
    {
        internal static IHtcRevenueGetListEv MakeIHtcRevenueGetListEv(CommonParam param, object data)
        {
            IHtcRevenueGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(HtcRevenueFilterQuery))
                {
                    result = new HtcRevenueGetListEvBehaviorByFilterQuery(param, (HtcRevenueFilterQuery)data);
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
