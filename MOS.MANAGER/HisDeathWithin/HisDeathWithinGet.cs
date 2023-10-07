using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDeathWithin
{
    class HisDeathWithinGet : GetBase
    {
        internal HisDeathWithinGet()
            : base()
        {

        }

        internal HisDeathWithinGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_DEATH_WITHIN> Get(HisDeathWithinFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDeathWithinDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEATH_WITHIN GetById(long id)
        {
            try
            {
                return GetById(id, new HisDeathWithinFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEATH_WITHIN GetById(long id, HisDeathWithinFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDeathWithinDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEATH_WITHIN GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisDeathWithinFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEATH_WITHIN GetByCode(string code, HisDeathWithinFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDeathWithinDAO.GetByCode(code, filter.Query());
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
