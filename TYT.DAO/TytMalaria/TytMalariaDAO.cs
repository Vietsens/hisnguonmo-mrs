using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytMalaria
{
    public partial class TytMalariaDAO : EntityBase
    {
        private TytMalariaGet GetWorker
        {
            get
            {
                return (TytMalariaGet)Worker.Get<TytMalariaGet>();
            }
        }

        public List<TYT_MALARIA> Get(TytMalariaSO search, CommonParam param)
        {
            List<TYT_MALARIA> result = new List<TYT_MALARIA>();
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

        public TYT_MALARIA GetById(long id, TytMalariaSO search)
        {
            TYT_MALARIA result = null;
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
