using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTracking
{
    public partial class HisTrackingDAO : EntityBase
    {
        public List<V_HIS_TRACKING> GetView(HisTrackingSO search, CommonParam param)
        {
            List<V_HIS_TRACKING> result = new List<V_HIS_TRACKING>();
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

        public V_HIS_TRACKING GetViewById(long id, HisTrackingSO search)
        {
            V_HIS_TRACKING result = null;

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
