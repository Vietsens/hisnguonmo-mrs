using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceFollow
{
    public partial class HisServiceFollowDAO : EntityBase
    {
        private HisServiceFollowGet GetWorker
        {
            get
            {
                return (HisServiceFollowGet)Worker.Get<HisServiceFollowGet>();
            }
        }
        public List<HIS_SERVICE_FOLLOW> Get(HisServiceFollowSO search, CommonParam param)
        {
            List<HIS_SERVICE_FOLLOW> result = new List<HIS_SERVICE_FOLLOW>();
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

        public HIS_SERVICE_FOLLOW GetById(long id, HisServiceFollowSO search)
        {
            HIS_SERVICE_FOLLOW result = null;
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
