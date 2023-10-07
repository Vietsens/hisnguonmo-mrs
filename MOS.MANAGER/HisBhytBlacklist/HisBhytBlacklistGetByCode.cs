using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBhytBlacklist
{
    partial class HisBhytBlacklistGet : BusinessBase
    {
        internal HIS_BHYT_BLACKLIST GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBhytBlacklistFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BHYT_BLACKLIST GetByCode(string code, HisBhytBlacklistFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBhytBlacklistDAO.GetByCode(code, filter.Query());
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
