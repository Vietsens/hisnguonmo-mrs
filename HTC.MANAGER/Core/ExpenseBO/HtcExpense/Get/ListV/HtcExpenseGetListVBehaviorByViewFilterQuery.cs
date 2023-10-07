using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get.ListV
{
    class HtcExpenseGetListVBehaviorByViewFilterQuery : BeanObjectBase, IHtcExpenseGetListV
    {
        HtcExpenseViewFilterQuery filterQuery;

        internal HtcExpenseGetListVBehaviorByViewFilterQuery(CommonParam param, HtcExpenseViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_HTC_EXPENSE> IHtcExpenseGetListV.Run()
        {
            try
            {
                return DAOWorker.HtcExpenseDAO.GetView(filterQuery.Query(), param);
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
