using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDebate
{
    public partial class HisDebateDAO : EntityBase
    {
        private HisDebateGet GetWorker
        {
            get
            {
                return (HisDebateGet)Worker.Get<HisDebateGet>();
            }
        }
        public List<HIS_DEBATE> Get(HisDebateSO search, CommonParam param)
        {
            List<HIS_DEBATE> result = new List<HIS_DEBATE>();
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

        public HIS_DEBATE GetById(long id, HisDebateSO search)
        {
            HIS_DEBATE result = null;
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
