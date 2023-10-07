using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceFollow
{
    public partial class HisServiceFollowDAO : EntityBase
    {
        public List<V_HIS_SERVICE_FOLLOW> GetView(HisServiceFollowSO search, CommonParam param)
        {
            List<V_HIS_SERVICE_FOLLOW> result = new List<V_HIS_SERVICE_FOLLOW>();
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

        public V_HIS_SERVICE_FOLLOW GetViewById(long id, HisServiceFollowSO search)
        {
            V_HIS_SERVICE_FOLLOW result = null;

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
