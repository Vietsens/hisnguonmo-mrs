using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get.ListEv
{
    class HtcExpenseGetListEvBehaviorFactory
    {
        internal static IHtcExpenseGetListEv MakeIHtcExpenseGetListEv(CommonParam param, object data)
        {
            IHtcExpenseGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(HtcExpenseFilterQuery))
                {
                    result = new HtcExpenseGetListEvBehaviorByFilterQuery(param, (HtcExpenseFilterQuery)data);
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
