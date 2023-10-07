using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTransaction
{
    public partial class HisTransactionDAO : EntityBase
    {
        private HisTransactionGet GetWorker
        {
            get
            {
                return (HisTransactionGet)Worker.Get<HisTransactionGet>();
            }
        }
        public List<HIS_TRANSACTION> Get(HisTransactionSO search, CommonParam param)
        {
            List<HIS_TRANSACTION> result = new List<HIS_TRANSACTION>();
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

        public HIS_TRANSACTION GetById(long id, HisTransactionSO search)
        {
            HIS_TRANSACTION result = null;
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
