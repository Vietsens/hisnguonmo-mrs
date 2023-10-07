using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceReqType
{
    class HisServiceReqTypeGet : GetBase
    {
        internal HisServiceReqTypeGet()
            : base()
        {

        }

        internal HisServiceReqTypeGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_REQ_TYPE> Get(HisServiceReqTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceReqTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ_TYPE GetById(long id, HisServiceReqTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisServiceReqTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ_TYPE GetByCode(string code, HisServiceReqTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqTypeDAO.GetByCode(code, filter.Query());
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
