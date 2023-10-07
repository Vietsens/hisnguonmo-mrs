using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceGroup
{
    class HisServiceGroupGet : GetBase
    {
        internal HisServiceGroupGet()
            : base()
        {

        }

        internal HisServiceGroupGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_GROUP> Get(HisServiceGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceGroupDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_GROUP GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_GROUP GetById(long id, HisServiceGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceGroupDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_GROUP GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisServiceGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_GROUP GetByCode(string code, HisServiceGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceGroupDAO.GetByCode(code, filter.Query());
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
