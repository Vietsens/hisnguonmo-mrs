using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisHeinServiceType
{
    class HisHeinServiceTypeGet : GetBase
    {
        internal HisHeinServiceTypeGet()
            : base()
        {

        }

        internal HisHeinServiceTypeGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_HEIN_SERVICE_TYPE> Get(HisHeinServiceTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisHeinServiceTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_HEIN_SERVICE_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisHeinServiceTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_HEIN_SERVICE_TYPE GetById(long id, HisHeinServiceTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisHeinServiceTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_HEIN_SERVICE_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisHeinServiceTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_HEIN_SERVICE_TYPE GetByCode(string code, HisHeinServiceTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisHeinServiceTypeDAO.GetByCode(code, filter.Query());
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
