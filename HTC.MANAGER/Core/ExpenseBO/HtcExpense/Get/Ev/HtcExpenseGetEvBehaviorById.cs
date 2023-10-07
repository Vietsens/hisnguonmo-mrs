using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get.Ev
{
    class HtcExpenseGetEvBehaviorById : BeanObjectBase, IHtcExpenseGetEv
    {
        long id;

        internal HtcExpenseGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        HTC_EXPENSE IHtcExpenseGetEv.Run()
        {
            try
            {
                return DAOWorker.HtcExpenseDAO.GetById(id, new HtcExpenseFilterQuery().Query());
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
