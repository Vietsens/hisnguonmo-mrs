using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTransaction
{
    public partial class HisTransactionDAO : EntityBase
    {
        public List<V_HIS_TRANSACTION> GetView(HisTransactionSO search, CommonParam param)
        {
            List<V_HIS_TRANSACTION> result = new List<V_HIS_TRANSACTION>();

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

        public HIS_TRANSACTION GetByCode(string code, HisTransactionSO search)
        {
            HIS_TRANSACTION result = null;

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
        
        public V_HIS_TRANSACTION GetViewById(long id, HisTransactionSO search)
        {
            V_HIS_TRANSACTION result = null;

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

        public V_HIS_TRANSACTION GetViewByCode(string code, HisTransactionSO search)
        {
            V_HIS_TRANSACTION result = null;

            try
            {
                result = GetWorker.GetViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public Dictionary<string, HIS_TRANSACTION> GetDicByCode(HisTransactionSO search, CommonParam param)
        {
            Dictionary<string, HIS_TRANSACTION> result = new Dictionary<string, HIS_TRANSACTION>();
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
