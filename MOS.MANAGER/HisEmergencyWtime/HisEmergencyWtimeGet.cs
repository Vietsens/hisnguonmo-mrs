using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmergencyWtime
{
    class HisEmergencyWtimeGet : GetBase
    {
        internal HisEmergencyWtimeGet()
            : base()
        {

        }

        internal HisEmergencyWtimeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EMERGENCY_WTIME> Get(HisEmergencyWtimeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmergencyWtimeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMERGENCY_WTIME GetById(long id)
        {
            try
            {
                return GetById(id, new HisEmergencyWtimeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMERGENCY_WTIME GetById(long id, HisEmergencyWtimeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmergencyWtimeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMERGENCY_WTIME GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisEmergencyWtimeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMERGENCY_WTIME GetByCode(string code, HisEmergencyWtimeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmergencyWtimeDAO.GetByCode(code, filter.Query());
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
