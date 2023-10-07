using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBhytWhitelist
{
    public partial class HisBhytWhitelistDAO : EntityBase
    {
        private HisBhytWhitelistGet GetWorker
        {
            get
            {
                return (HisBhytWhitelistGet)Worker.Get<HisBhytWhitelistGet>();
            }
        }
        public List<HIS_BHYT_WHITELIST> Get(HisBhytWhitelistSO search, CommonParam param)
        {
            List<HIS_BHYT_WHITELIST> result = new List<HIS_BHYT_WHITELIST>();
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

        public HIS_BHYT_WHITELIST GetById(long id, HisBhytWhitelistSO search)
        {
            HIS_BHYT_WHITELIST result = null;
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
