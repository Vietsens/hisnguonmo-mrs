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
        internal HisBhytWhitelistGet()
            : base()
        {

        }

        internal HisBhytWhitelistGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BHYT_WHITELIST> Get(HisBhytWhitelistFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBhytWhitelistDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BHYT_WHITELIST GetById(long id)
        {
            try
            {
                return GetById(id, new HisBhytWhitelistFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BHYT_WHITELIST GetById(long id, HisBhytWhitelistFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBhytWhitelistDAO.GetById(id, filter.Query());
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
