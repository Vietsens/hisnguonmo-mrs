using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.ListEv
{
    class HtcPeriodDepartmentGetListEvBehaviorFactory
    {
        internal static IHtcPeriodDepartmentGetListEv MakeIHtcPeriodDepartmentGetListEv(CommonParam param, object data)
        {
            IHtcPeriodDepartmentGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(HtcPeriodDepartmentFilterQuery))
                {
                    result = new HtcPeriodDepartmentGetListEvBehaviorByFilterQuery(param, (HtcPeriodDepartmentFilterQuery)data);
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
