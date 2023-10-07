using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteGroup
{
    class HisExecuteGroupGet : GetBase
    {
        internal HisExecuteGroupGet()
            : base()
        {

        }

        internal HisExecuteGroupGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXECUTE_GROUP> Get(HisExecuteGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteGroupDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_GROUP GetById(long id)
        {
            try
            {
                return GetById(id, new HisExecuteGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_GROUP GetById(long id, HisExecuteGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteGroupDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_GROUP GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisExecuteGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_GROUP GetByCode(string code, HisExecuteGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteGroupDAO.GetByCode(code, filter.Query());
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
