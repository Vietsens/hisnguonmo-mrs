using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytFetusAbortion
{
    public partial class TytFetusAbortionDAO : EntityBase
    {
        private TytFetusAbortionGet GetWorker
        {
            get
            {
                return (TytFetusAbortionGet)Worker.Get<TytFetusAbortionGet>();
            }
        }

        public List<TYT_FETUS_ABORTION> Get(TytFetusAbortionSO search, CommonParam param)
        {
            List<TYT_FETUS_ABORTION> result = new List<TYT_FETUS_ABORTION>();
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

        public TYT_FETUS_ABORTION GetById(long id, TytFetusAbortionSO search)
        {
            TYT_FETUS_ABORTION result = null;
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
