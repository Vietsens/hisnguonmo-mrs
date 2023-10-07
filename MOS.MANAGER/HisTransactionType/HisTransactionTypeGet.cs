using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTransactionType
{
    class HisTransactionTypeGet : GetBase
    {
        internal HisTransactionTypeGet()
            : base()
        {

        }

        internal HisTransactionTypeGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TRANSACTION_TYPE> Get(HisTransactionTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRANSACTION_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisTransactionTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRANSACTION_TYPE GetById(long id, HisTransactionTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRANSACTION_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisTransactionTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRANSACTION_TYPE GetByCode(string code, HisTransactionTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionTypeDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

    }
}
