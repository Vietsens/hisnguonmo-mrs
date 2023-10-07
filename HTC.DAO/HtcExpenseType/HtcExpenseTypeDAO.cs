using HTC.DAO.StagingObject;
using HTC.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.DAO.HtcExpenseType
{
    public partial class HtcExpenseTypeDAO : EntityBase
    {
        private HtcExpenseTypeGet GetWorker
        {
            get
            {
                return (HtcExpenseTypeGet)Worker.Get<HtcExpenseTypeGet>();
            }
        }

        public List<HTC_EXPENSE_TYPE> Get(HtcExpenseTypeSO search, CommonParam param)
        {
            List<HTC_EXPENSE_TYPE> result = new List<HTC_EXPENSE_TYPE>();
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

        public HTC_EXPENSE_TYPE GetById(long id, HtcExpenseTypeSO search)
        {
            HTC_EXPENSE_TYPE result = null;
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
