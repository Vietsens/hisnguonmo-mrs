using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytDeath
{
    public partial class TytDeathDAO : EntityBase
    {
        private TytDeathGet GetWorker
        {
            get
            {
                return (TytDeathGet)Worker.Get<TytDeathGet>();
            }
        }

        public List<TYT_DEATH> Get(TytDeathSO search, CommonParam param)
        {
            List<TYT_DEATH> result = new List<TYT_DEATH>();
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

        public TYT_DEATH GetById(long id, TytDeathSO search)
        {
            TYT_DEATH result = null;
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
