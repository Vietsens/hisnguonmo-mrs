using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytDeath
{
    public partial class TytDeathDAO : EntityBase
    {
        public List<V_TYT_DEATH> GetView(TytDeathSO search, CommonParam param)
        {
            List<V_TYT_DEATH> result = new List<V_TYT_DEATH>();
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

        public V_TYT_DEATH GetViewById(long id, TytDeathSO search)
        {
            V_TYT_DEATH result = null;

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
