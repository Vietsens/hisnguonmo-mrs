using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytUninfectIcdGroup
{
    public partial class TytUninfectIcdGroupDAO : EntityBase
    {
        private TytUninfectIcdGroupGet GetWorker
        {
            get
            {
                return (TytUninfectIcdGroupGet)Worker.Get<TytUninfectIcdGroupGet>();
            }
        }

        public List<TYT_UNINFECT_ICD_GROUP> Get(TytUninfectIcdGroupSO search, CommonParam param)
        {
            List<TYT_UNINFECT_ICD_GROUP> result = new List<TYT_UNINFECT_ICD_GROUP>();
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

        public TYT_UNINFECT_ICD_GROUP GetById(long id, TytUninfectIcdGroupSO search)
        {
            TYT_UNINFECT_ICD_GROUP result = null;
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
