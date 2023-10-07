using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestTypeUser
{
    partial class HisImpMestTypeUserGet : BusinessBase
    {
        internal HisImpMestTypeUserGet()
            : base()
        {

        }

        internal HisImpMestTypeUserGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_IMP_MEST_TYPE_USER> Get(HisImpMestTypeUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestTypeUserDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_TYPE_USER GetById(long id)
        {
            try
            {
                return GetById(id, new HisImpMestTypeUserFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_TYPE_USER GetById(long id, HisImpMestTypeUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestTypeUserDAO.GetById(id, filter.Query());
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
