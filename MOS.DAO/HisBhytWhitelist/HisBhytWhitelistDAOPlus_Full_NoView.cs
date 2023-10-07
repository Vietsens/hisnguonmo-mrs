using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBhytWhitelist
{
    public partial class HisBhytWhitelistDAO : EntityBase
    {
        public HIS_BHYT_WHITELIST GetByCode(string code, HisBhytWhitelistSO search)
        {
            HIS_BHYT_WHITELIST result = null;

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

        public Dictionary<string, HIS_BHYT_WHITELIST> GetDicByCode(HisBhytWhitelistSO search, CommonParam param)
        {
            Dictionary<string, HIS_BHYT_WHITELIST> result = new Dictionary<string, HIS_BHYT_WHITELIST>();
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
