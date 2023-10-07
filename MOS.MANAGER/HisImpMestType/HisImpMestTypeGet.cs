using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestType
{
    class HisImpMestTypeGet : GetBase
    {
        internal HisImpMestTypeGet()
            : base()
        {

        }

        internal HisImpMestTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_IMP_MEST_TYPE> Get(HisImpMestTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisImpMestTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_TYPE GetById(long id, HisImpMestTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisImpMestTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_TYPE GetByCode(string code, HisImpMestTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestTypeDAO.GetByCode(code, filter.Query());
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
