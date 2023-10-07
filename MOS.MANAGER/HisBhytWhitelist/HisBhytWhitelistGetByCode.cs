using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBhytWhitelist
{
    partial class HisBhytWhitelistGet : BusinessBase
    {
        internal HIS_BHYT_WHITELIST GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBhytWhitelistFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BHYT_WHITELIST GetByCode(string code, HisBhytWhitelistFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBhytWhitelistDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
