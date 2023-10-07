using HTC.DAO.StagingObject;
using HTC.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.DAO.HtcExpense
{
    public partial class HtcExpenseDAO : EntityBase
    {
        public List<V_HTC_EXPENSE> GetView(HtcExpenseSO search, CommonParam param)
        {
            List<V_HTC_EXPENSE> result = new List<V_HTC_EXPENSE>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HTC_EXPENSE GetViewById(long id, HtcExpenseSO search)
        {
            V_HTC_EXPENSE result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
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
