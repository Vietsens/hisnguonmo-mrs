using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestUser
{
    partial class HisExpMestUserGet : BusinessBase
    {
        internal HisExpMestUserGet()
            : base()
        {

        }

        internal HisExpMestUserGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXP_MEST_USER> Get(HisExpMestUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestUserDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_USER GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestUserFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_USER GetById(long id, HisExpMestUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestUserDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_USER> GetByExpMestId(long expMestId)
        {
            HisExpMestUserFilterQuery filter = new HisExpMestUserFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.Get(filter);
        }

    }
}
