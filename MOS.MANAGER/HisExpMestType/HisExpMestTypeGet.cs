using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestType
{
    class HisExpMestTypeGet : GetBase
    {
        internal HisExpMestTypeGet()
            : base()
        {

        }

        internal HisExpMestTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXP_MEST_TYPE> Get(HisExpMestTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_TYPE GetById(long id, HisExpMestTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisExpMestTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_TYPE GetByCode(string code, HisExpMestTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestTypeDAO.GetByCode(code, filter.Query());
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
