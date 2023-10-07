using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRehaSum
{
    public partial class HisRehaSumDAO : EntityBase
    {
        private HisRehaSumGet GetWorker
        {
            get
            {
                return (HisRehaSumGet)Worker.Get<HisRehaSumGet>();
            }
        }
        public List<HIS_REHA_SUM> Get(HisRehaSumSO search, CommonParam param)
        {
            List<HIS_REHA_SUM> result = new List<HIS_REHA_SUM>();
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

        public HIS_REHA_SUM GetById(long id, HisRehaSumSO search)
        {
            HIS_REHA_SUM result = null;
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
