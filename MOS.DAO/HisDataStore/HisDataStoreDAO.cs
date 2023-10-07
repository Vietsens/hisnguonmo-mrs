using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDataStore
{
    public partial class HisDataStoreDAO : EntityBase
    {
        private HisDataStoreGet GetWorker
        {
            get
            {
                return (HisDataStoreGet)Worker.Get<HisDataStoreGet>();
            }
        }
        public List<HIS_DATA_STORE> Get(HisDataStoreSO search, CommonParam param)
        {
            List<HIS_DATA_STORE> result = new List<HIS_DATA_STORE>();
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

        public HIS_DATA_STORE GetById(long id, HisDataStoreSO search)
        {
            HIS_DATA_STORE result = null;
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
