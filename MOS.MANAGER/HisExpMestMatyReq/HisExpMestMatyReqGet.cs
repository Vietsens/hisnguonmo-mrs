using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMatyReq
{
    partial class HisExpMestMatyReqGet : BusinessBase
    {
        internal HisExpMestMatyReqGet()
            : base()
        {

        }

        internal HisExpMestMatyReqGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXP_MEST_MATY_REQ> Get(HisExpMestMatyReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMatyReqDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_MATY_REQ GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestMatyReqFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_MATY_REQ GetById(long id, HisExpMestMatyReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMatyReqDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_MATY_REQ> GetByExpMestIds(List<long> expMestIds)
        {
            if (IsNotNullOrEmpty(expMestIds))
            {
                HisExpMestMatyReqFilterQuery filter = new HisExpMestMatyReqFilterQuery();
                filter.EXP_MEST_IDs = expMestIds;
                return this.Get(filter);
            }
            return null;
        }

        internal List<HIS_EXP_MEST_MATY_REQ> GetByExpMestId(long expMestId)
        {
            HisExpMestMatyReqFilterQuery filter = new HisExpMestMatyReqFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.Get(filter);
        }
    }
}
