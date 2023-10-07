using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMetyReq
{
    partial class HisExpMestMetyReqGet : BusinessBase
    {
        internal HisExpMestMetyReqGet()
            : base()
        {

        }

        internal HisExpMestMetyReqGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXP_MEST_METY_REQ> Get(HisExpMestMetyReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMetyReqDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_METY_REQ GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestMetyReqFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_METY_REQ GetById(long id, HisExpMestMetyReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMetyReqDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_METY_REQ> GetByExpMestIds(List<long> expMestIds)
        {
            if (IsNotNullOrEmpty(expMestIds))
            {
                HisExpMestMetyReqFilterQuery filter = new HisExpMestMetyReqFilterQuery();
                filter.EXP_MEST_IDs = expMestIds;
                return this.Get(filter);
            }
            return null;
        }

        internal List<HIS_EXP_MEST_METY_REQ> GetByExpMestId(long expMestId)
        {
            HisExpMestMetyReqFilterQuery filter = new HisExpMestMetyReqFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.Get(filter);
        }
    }
}
