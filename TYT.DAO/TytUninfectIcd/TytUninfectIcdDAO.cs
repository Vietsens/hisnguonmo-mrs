using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytUninfectIcd
{
    public partial class TytUninfectIcdDAO : EntityBase
    {
        private TytUninfectIcdGet GetWorker
        {
            get
            {
                return (TytUninfectIcdGet)Worker.Get<TytUninfectIcdGet>();
            }
        }

        public List<TYT_UNINFECT_ICD> Get(TytUninfectIcdSO search, CommonParam param)
        {
            List<TYT_UNINFECT_ICD> result = new List<TYT_UNINFECT_ICD>();
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

        public TYT_UNINFECT_ICD GetById(long id, TytUninfectIcdSO search)
        {
            TYT_UNINFECT_ICD result = null;
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
