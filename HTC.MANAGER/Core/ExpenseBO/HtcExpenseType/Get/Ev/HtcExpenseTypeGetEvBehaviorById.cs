using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpenseType.Get.Ev
{
    class HtcExpenseTypeGetEvBehaviorById : BeanObjectBase, IHtcExpenseTypeGetEv
    {
        long id;

        internal HtcExpenseTypeGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        HTC_EXPENSE_TYPE IHtcExpenseTypeGetEv.Run()
        {
            try
            {
                return DAOWorker.HtcExpenseTypeDAO.GetById(id, new HtcExpenseTypeFilterQuery().Query());
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
