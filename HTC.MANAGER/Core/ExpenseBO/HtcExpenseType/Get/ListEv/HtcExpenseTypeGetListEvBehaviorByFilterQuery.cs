using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpenseType.Get.ListEv
{
    class HtcExpenseTypeGetListEvBehaviorByFilterQuery : BeanObjectBase, IHtcExpenseTypeGetListEv
    {
        HtcExpenseTypeFilterQuery filterQuery;

        internal HtcExpenseTypeGetListEvBehaviorByFilterQuery(CommonParam param, HtcExpenseTypeFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<HTC_EXPENSE_TYPE> IHtcExpenseTypeGetListEv.Run()
        {
            try
            {
                return DAOWorker.HtcExpenseTypeDAO.Get(filterQuery.Query(), param);
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
