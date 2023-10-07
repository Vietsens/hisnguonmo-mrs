using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get.ListEv
{
    class HtcExpenseGetListEvBehaviorByFilterQuery : BeanObjectBase, IHtcExpenseGetListEv
    {
        HtcExpenseFilterQuery filterQuery;

        internal HtcExpenseGetListEvBehaviorByFilterQuery(CommonParam param, HtcExpenseFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<HTC_EXPENSE> IHtcExpenseGetListEv.Run()
        {
            try
            {
                return DAOWorker.HtcExpenseDAO.Get(filterQuery.Query(), param);
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
