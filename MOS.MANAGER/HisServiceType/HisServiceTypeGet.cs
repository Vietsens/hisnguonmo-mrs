using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceType
{
    class HisServiceTypeGet : GetBase
    {
        internal HisServiceTypeGet()
            : base()
        {

        }

        internal HisServiceTypeGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_TYPE> Get(HisServiceTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_TYPE GetById(long id, HisServiceTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisServiceTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_TYPE GetByCode(string code, HisServiceTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceTypeDAO.GetByCode(code, filter.Query());
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
