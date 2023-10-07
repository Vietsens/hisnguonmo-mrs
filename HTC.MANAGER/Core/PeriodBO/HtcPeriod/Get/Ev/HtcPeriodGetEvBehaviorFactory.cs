using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriod.Get.Ev
{
    class HtcPeriodGetEvBehaviorFactory
    {
        internal static IHtcPeriodGetEv MakeIHtcPeriodGetEv(CommonParam param, object data)
        {
            IHtcPeriodGetEv result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new HtcPeriodGetEvBehaviorById(param, long.Parse(data.ToString()));
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
