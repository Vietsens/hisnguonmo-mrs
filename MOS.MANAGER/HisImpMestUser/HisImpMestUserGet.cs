using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestUser
{
    partial class HisImpMestUserGet : BusinessBase
    {
        internal HisImpMestUserGet()
            : base()
        {

        }

        internal HisImpMestUserGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_IMP_MEST_USER> Get(HisImpMestUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestUserDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_USER GetById(long id)
        {
            try
            {
                return GetById(id, new HisImpMestUserFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_USER GetById(long id, HisImpMestUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestUserDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_IMP_MEST_USER> GetByImpMestId(long impMestId)
        {
            HisImpMestUserFilterQuery filter = new HisImpMestUserFilterQuery();
            filter.IMP_MEST_ID = impMestId;
            return this.Get(filter);
        }
    }
}
