using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceReqStt
{
    class HisServiceReqSttGet : GetBase
    {
        internal HisServiceReqSttGet()
            : base()
        {

        }

        internal HisServiceReqSttGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_REQ_STT> Get(HisServiceReqSttFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqSttDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ_STT GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceReqSttFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ_STT GetById(long id, HisServiceReqSttFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqSttDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ_STT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisServiceReqSttFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ_STT GetByCode(string code, HisServiceReqSttFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqSttDAO.GetByCode(code, filter.Query());
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
