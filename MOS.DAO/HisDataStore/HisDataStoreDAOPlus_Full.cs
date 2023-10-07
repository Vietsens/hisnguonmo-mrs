using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDataStore
{
    public partial class HisDataStoreDAO : EntityBase
    {
        public List<V_HIS_DATA_STORE> GetView(HisDataStoreSO search, CommonParam param)
        {
            List<V_HIS_DATA_STORE> result = new List<V_HIS_DATA_STORE>();

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

        public HIS_DATA_STORE GetByCode(string code, HisDataStoreSO search)
        {
            HIS_DATA_STORE result = null;

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
        
        public V_HIS_DATA_STORE GetViewById(long id, HisDataStoreSO search)
        {
            V_HIS_DATA_STORE result = null;

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

        public V_HIS_DATA_STORE GetViewByCode(string code, HisDataStoreSO search)
        {
            V_HIS_DATA_STORE result = null;

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

        public Dictionary<string, HIS_DATA_STORE> GetDicByCode(HisDataStoreSO search, CommonParam param)
        {
            Dictionary<string, HIS_DATA_STORE> result = new Dictionary<string, HIS_DATA_STORE>();
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
