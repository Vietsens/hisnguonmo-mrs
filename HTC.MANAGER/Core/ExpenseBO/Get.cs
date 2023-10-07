using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get.Ev;
using HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get.ListEv;
using HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get.ListV;
using HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get.V;
using HTC.MANAGER.Core.ExpenseBO.HtcExpenseType.Get.Ev;
using HTC.MANAGER.Core.ExpenseBO.HtcExpenseType.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.ExpenseBO
{
    partial class Get : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal Get(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<HTC_EXPENSE>))
                {
                    IHtcExpenseGetListEv behavior = HtcExpenseGetListEvBehaviorFactory.MakeIHtcExpenseGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(HTC_EXPENSE))
                {
                    IHtcExpenseGetEv behavior = HtcExpenseGetEvBehaviorFactory.MakeIHtcExpenseGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_HTC_EXPENSE>))
                {
                    IHtcExpenseGetListV behavior = HtcExpenseGetListVBehaviorFactory.MakeIHtcExpenseGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_HTC_EXPENSE))
                {
                    IHtcExpenseGetV behavior = HtcExpenseGetVBehaviorFactory.MakeIHtcExpenseGetV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<HTC_EXPENSE_TYPE>))
                {
                    IHtcExpenseTypeGetListEv behavior = HtcExpenseTypeGetListEvBehaviorFactory.MakeIHtcExpenseTypeGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(HTC_EXPENSE_TYPE))
                {
                    IHtcExpenseTypeGetEv behavior = HtcExpenseTypeGetEvBehaviorFactory.MakeIHtcExpenseTypeGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = default(T);
            }
            return result;
        }
    }
}
