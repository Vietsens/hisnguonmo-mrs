using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDeathCause
{
    class HisDeathCauseGet : GetBase
    {
        internal HisDeathCauseGet()
            : base()
        {

        }

        internal HisDeathCauseGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_DEATH_CAUSE> Get(HisDeathCauseFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDeathCauseDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEATH_CAUSE GetById(long id)
        {
            try
            {
                return GetById(id, new HisDeathCauseFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEATH_CAUSE GetById(long id, HisDeathCauseFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDeathCauseDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEATH_CAUSE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisDeathCauseFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEATH_CAUSE GetByCode(string code, HisDeathCauseFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDeathCauseDAO.GetByCode(code, filter.Query());
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
