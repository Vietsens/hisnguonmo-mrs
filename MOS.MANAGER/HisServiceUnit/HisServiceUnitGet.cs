using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceUnit
{
    class HisServiceUnitGet : GetBase
    {
        internal HisServiceUnitGet()
            : base()
        {

        }

        internal HisServiceUnitGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_UNIT> Get(HisServiceUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceUnitDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_UNIT GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceUnitFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_UNIT GetById(long id, HisServiceUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceUnitDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_UNIT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisServiceUnitFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_UNIT GetByCode(string code, HisServiceUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceUnitDAO.GetByCode(code, filter.Query());
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
