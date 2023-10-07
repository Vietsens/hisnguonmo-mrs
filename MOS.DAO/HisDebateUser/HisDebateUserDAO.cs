using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDebateUser
{
    public partial class HisDebateUserDAO : EntityBase
    {
        private HisDebateUserGet GetWorker
        {
            get
            {
                return (HisDebateUserGet)Worker.Get<HisDebateUserGet>();
            }
        }
        public List<HIS_DEBATE_USER> Get(HisDebateUserSO search, CommonParam param)
        {
            List<HIS_DEBATE_USER> result = new List<HIS_DEBATE_USER>();
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

        public HIS_DEBATE_USER GetById(long id, HisDebateUserSO search)
        {
            HIS_DEBATE_USER result = null;
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
