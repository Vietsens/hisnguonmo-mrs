using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpenseType.Get.ListEv
{
    class HtcExpenseTypeGetListEvBehaviorFactory
    {
        internal static IHtcExpenseTypeGetListEv MakeIHtcExpenseTypeGetListEv(CommonParam param, object data)
        {
            IHtcExpenseTypeGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(HtcExpenseTypeFilterQuery))
                {
                    result = new HtcExpenseTypeGetListEvBehaviorByFilterQuery(param, (HtcExpenseTypeFilterQuery)data);
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
