using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAwareness
{
    public partial class HisAwarenessDAO : EntityBase
    {
        private HisAwarenessGet GetWorker
        {
            get
            {
                return (HisAwarenessGet)Worker.Get<HisAwarenessGet>();
            }
        }
        public List<HIS_AWARENESS> Get(HisAwarenessSO search, CommonParam param)
        {
            List<HIS_AWARENESS> result = new List<HIS_AWARENESS>();
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

        public HIS_AWARENESS GetById(long id, HisAwarenessSO search)
        {
            HIS_AWARENESS result = null;
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
