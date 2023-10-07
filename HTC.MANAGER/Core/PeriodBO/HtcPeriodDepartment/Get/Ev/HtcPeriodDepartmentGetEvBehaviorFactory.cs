using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.Ev
{
    class HtcPeriodDepartmentGetEvBehaviorFactory
    {
        internal static IHtcPeriodDepartmentGetEv MakeIHtcPeriodDepartmentGetEv(CommonParam param, object data)
        {
            IHtcPeriodDepartmentGetEv result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new HtcPeriodDepartmentGetEvBehaviorById(param, long.Parse(data.ToString()));
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
