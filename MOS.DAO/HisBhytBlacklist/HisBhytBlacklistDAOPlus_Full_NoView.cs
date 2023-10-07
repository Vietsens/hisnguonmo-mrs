using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBhytBlacklist
{
    public partial class HisBhytBlacklistDAO : EntityBase
    {
        public HIS_BHYT_BLACKLIST GetByCode(string code, HisBhytBlacklistSO search)
        {
            HIS_BHYT_BLACKLIST result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_BHYT_BLACKLIST> GetDicByCode(HisBhytBlacklistSO search, CommonParam param)
        {
            Dictionary<string, HIS_BHYT_BLACKLIST> result = new Dictionary<string, HIS_BHYT_BLACKLIST>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
