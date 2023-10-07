using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTracking
{
    public partial class HisTrackingDAO : EntityBase
    {
        private HisTrackingGet GetWorker
        {
            get
            {
                return (HisTrackingGet)Worker.Get<HisTrackingGet>();
            }
        }
        public List<HIS_TRACKING> Get(HisTrackingSO search, CommonParam param)
        {
            List<HIS_TRACKING> result = new List<HIS_TRACKING>();
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

        public HIS_TRACKING GetById(long id, HisTrackingSO search)
        {
            HIS_TRACKING result = null;
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
