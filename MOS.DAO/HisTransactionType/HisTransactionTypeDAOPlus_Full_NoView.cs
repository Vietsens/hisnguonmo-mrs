using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTransactionType
{
    public partial class HisTransactionTypeDAO : EntityBase
    {
        public HIS_TRANSACTION_TYPE GetByCode(string code, HisTransactionTypeSO search)
        {
            HIS_TRANSACTION_TYPE result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_TRANSACTION_TYPE> GetDicByCode(HisTransactionTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_TRANSACTION_TYPE> result = new Dictionary<string, HIS_TRANSACTION_TYPE>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
