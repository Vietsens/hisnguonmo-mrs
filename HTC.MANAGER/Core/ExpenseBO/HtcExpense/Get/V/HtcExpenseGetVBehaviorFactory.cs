using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get.V
{
    class HtcExpenseGetVBehaviorFactory
    {
        internal static IHtcExpenseGetV MakeIHtcExpenseGetV(CommonParam param, object data)
        {
            IHtcExpenseGetV result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new HtcExpenseGetVBehaviorById(param, long.Parse(data.ToString()));
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
