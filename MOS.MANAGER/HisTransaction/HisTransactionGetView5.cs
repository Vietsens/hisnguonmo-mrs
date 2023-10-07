using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTransaction
{
    partial class HisTransactionGet : GetBase
    {
        internal List<V_HIS_TRANSACTION_5> GetView5(HisTransactionView5FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionDAO.GetView5(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TRANSACTION_5 GetView5ById(long id)
        {
            try
            {
                return GetView5ById(id, new HisTransactionView5FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TRANSACTION_5 GetView5ById(long id, HisTransactionView5FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionDAO.GetView5ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TRANSACTION_5> GetView5ByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisTransactionView5FilterQuery filter = new HisTransactionView5FilterQuery();
                filter.IDs = ids;
                return this.GetView5(filter);
            }
            return null;
        }

        internal V_HIS_TRANSACTION_5 GetView5ByCode(string code)
        {
            try
            {
                return GetView5ByCode(code, new HisTransactionView5FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TRANSACTION_5 GetView5ByCode(string code, HisTransactionView5FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionDAO.GetView5ByCode(code, filter.Query());
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
