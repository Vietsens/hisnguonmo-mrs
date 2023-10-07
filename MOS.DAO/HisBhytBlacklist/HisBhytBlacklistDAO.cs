using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBhytBlacklist
{
    public partial class HisBhytBlacklistDAO : EntityBase
    {
        private HisBhytBlacklistGet GetWorker
        {
            get
            {
                return (HisBhytBlacklistGet)Worker.Get<HisBhytBlacklistGet>();
            }
        }
        public List<HIS_BHYT_BLACKLIST> Get(HisBhytBlacklistSO search, CommonParam param)
        {
            List<HIS_BHYT_BLACKLIST> result = new List<HIS_BHYT_BLACKLIST>();
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

        public HIS_BHYT_BLACKLIST GetById(long id, HisBhytBlacklistSO search)
        {
            HIS_BHYT_BLACKLIST result = null;
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
