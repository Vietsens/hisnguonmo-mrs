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

        public TYT_FETUS_ABORTION GetByCode(string code, TytFetusAbortionSO search)
        {
            TYT_FETUS_ABORTION result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
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

        public V_TYT_FETUS_ABORTION GetViewByCode(string code, TytFetusAbortionSO search)
        {
            V_TYT_FETUS_ABORTION result = null;

            try
            {
                result = GetWorker.GetViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public Dictionary<string, TYT_FETUS_ABORTION> GetDicByCode(TytFetusAbortionSO search, CommonParam param)
        {
            Dictionary<string, TYT_FETUS_ABORTION> result = new Dictionary<string, TYT_FETUS_ABORTION>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public bool ExistsCode(string code, long? id)
        {
            try
            {
                return CheckWorker.ExistsCode(code, id);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                throw;
            }
        }
    }
}
