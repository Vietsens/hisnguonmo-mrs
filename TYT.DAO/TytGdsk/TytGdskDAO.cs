using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytGdsk
{
    public partial class TytGdskDAO : EntityBase
    {
        private TytGdskGet GetWorker
        {
            get
            {
                return (TytGdskGet)Worker.Get<TytGdskGet>();
            }
        }

        public List<TYT_GDSK> Get(TytGdskSO search, CommonParam param)
        {
            List<TYT_GDSK> result = new List<TYT_GDSK>();
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

        public TYT_GDSK GetById(long id, TytGdskSO search)
        {
            TYT_GDSK result = null;
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
