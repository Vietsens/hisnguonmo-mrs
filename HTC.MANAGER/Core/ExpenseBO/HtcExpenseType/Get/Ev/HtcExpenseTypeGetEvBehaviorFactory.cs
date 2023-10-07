using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpenseType.Get.Ev
{
    class HtcExpenseTypeGetEvBehaviorFactory
    {
        internal static IHtcExpenseTypeGetEv MakeIHtcExpenseTypeGetEv(CommonParam param, object data)
        {
            IHtcExpenseTypeGetEv result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new HtcExpenseTypeGetEvBehaviorById(param, long.Parse(data.ToString()));
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
