using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytHiv
{
    public partial class TytHivDAO : EntityBase
    {
        private TytHivGet GetWorker
        {
            get
            {
                return (TytHivGet)Worker.Get<TytHivGet>();
            }
        }

        public List<TYT_HIV> Get(TytHivSO search, CommonParam param)
        {
            List<TYT_HIV> result = new List<TYT_HIV>();
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

        public TYT_HIV GetById(long id, TytHivSO search)
        {
            TYT_HIV result = null;
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
