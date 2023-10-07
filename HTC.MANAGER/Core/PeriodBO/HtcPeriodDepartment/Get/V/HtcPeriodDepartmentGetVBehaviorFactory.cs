using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.V
{
    class HtcPeriodDepartmentGetVBehaviorFactory
    {
        internal static IHtcPeriodDepartmentGetV MakeIHtcPeriodDepartmentGetV(CommonParam param, object data)
        {
            IHtcPeriodDepartmentGetV result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new HtcPeriodDepartmentGetVBehaviorById(param, long.Parse(data.ToString()));
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
