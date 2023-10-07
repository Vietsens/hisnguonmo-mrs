using HTC.DAO.StagingObject;
using HTC.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.DAO.HtcExpense
{
    public partial class HtcExpenseDAO : EntityBase
    {
        private HtcExpenseGet GetWorker
        {
            get
            {
                return (HtcExpenseGet)Worker.Get<HtcExpenseGet>();
            }
        }

        public List<HTC_EXPENSE> Get(HtcExpenseSO search, CommonParam param)
        {
            List<HTC_EXPENSE> result = new List<HTC_EXPENSE>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HTC_EXPENSE GetById(long id, HtcExpenseSO search)
        {
            HTC_EXPENSE result = null;
            try
            {
                result = GetWorker.GetById(id, search);
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
