using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytFetusAbortion
{
    public partial class TytFetusAbortionDAO : EntityBase
    {
        public List<V_TYT_FETUS_ABORTION> GetView(TytFetusAbortionSO search, CommonParam param)
        {
            List<V_TYT_FETUS_ABORTION> result = new List<V_TYT_FETUS_ABORTION>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_TYT_FETUS_ABORTION GetViewById(long id, TytFetusAbortionSO search)
        {
            V_TYT_FETUS_ABORTION result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
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
