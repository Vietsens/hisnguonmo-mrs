using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.ListV
{
    class HtcPeriodDepartmentGetListVBehaviorFactory
    {
        internal static IHtcPeriodDepartmentGetListV MakeIHtcPeriodDepartmentGetListV(CommonParam param, object data)
        {
            IHtcPeriodDepartmentGetListV result = null;
            try
            {
                if (data.GetType() == typeof(HtcPeriodDepartmentViewFilterQuery))
                {
                    result = new HtcPeriodDepartmentGetListVBehaviorByFilterQuery(param, (HtcPeriodDepartmentViewFilterQuery)data);
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
