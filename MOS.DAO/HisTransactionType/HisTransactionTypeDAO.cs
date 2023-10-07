using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTransactionType
{
    public partial class HisTransactionTypeDAO : EntityBase
    {
        private HisTransactionTypeGet GetWorker
        {
            get
            {
                return (HisTransactionTypeGet)Worker.Get<HisTransactionTypeGet>();
            }
        }
        public List<HIS_TRANSACTION_TYPE> Get(HisTransactionTypeSO search, CommonParam param)
        {
            List<HIS_TRANSACTION_TYPE> result = new List<HIS_TRANSACTION_TYPE>();
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

        public HIS_TRANSACTION_TYPE GetById(long id, HisTransactionTypeSO search)
        {
            HIS_TRANSACTION_TYPE result = null;
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
