using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get.V
{
    class HtcExpenseGetVBehaviorById : BeanObjectBase, IHtcExpenseGetV
    {
        long id;

        internal HtcExpenseGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_HTC_EXPENSE IHtcExpenseGetV.Run()
        {
            try
            {
                return DAOWorker.HtcExpenseDAO.GetViewById(id, new HtcExpenseViewFilterQuery().Query());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
