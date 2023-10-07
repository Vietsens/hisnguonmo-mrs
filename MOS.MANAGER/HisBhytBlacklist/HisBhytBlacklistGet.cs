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
        internal HisBhytBlacklistGet()
            : base()
        {

        }

        internal HisBhytBlacklistGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BHYT_BLACKLIST> Get(HisBhytBlacklistFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBhytBlacklistDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BHYT_BLACKLIST GetById(long id)
        {
            try
            {
                return GetById(id, new HisBhytBlacklistFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BHYT_BLACKLIST GetById(long id, HisBhytBlacklistFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBhytBlacklistDAO.GetById(id, filter.Query());
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
