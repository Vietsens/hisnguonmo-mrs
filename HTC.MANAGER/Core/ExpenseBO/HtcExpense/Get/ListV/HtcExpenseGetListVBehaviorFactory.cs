using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get.ListV
{
    class HtcExpenseGetListVBehaviorFactory
    {
        internal static IHtcExpenseGetListV MakeIHtcExpenseGetListV(CommonParam param, object data)
        {
            IHtcExpenseGetListV result = null;
            try
            {
                if (data.GetType() == typeof(HtcExpenseViewFilterQuery))
                {
                    result = new HtcExpenseGetListVBehaviorByViewFilterQuery(param, (HtcExpenseViewFilterQuery)data);
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
