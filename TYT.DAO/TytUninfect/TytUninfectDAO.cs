using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytUninfect
{
    public partial class TytUninfectDAO : EntityBase
    {
        private TytUninfectGet GetWorker
        {
            get
            {
                return (TytUninfectGet)Worker.Get<TytUninfectGet>();
            }
        }

        public List<TYT_UNINFECT> Get(TytUninfectSO search, CommonParam param)
        {
            List<TYT_UNINFECT> result = new List<TYT_UNINFECT>();
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

        public TYT_UNINFECT GetById(long id, TytUninfectSO search)
        {
            TYT_UNINFECT result = null;
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
