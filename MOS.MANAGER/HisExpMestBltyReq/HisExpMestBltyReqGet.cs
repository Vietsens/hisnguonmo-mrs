using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestBltyReq
{
    partial class HisExpMestBltyReqGet : BusinessBase
    {
        internal HisExpMestBltyReqGet()
            : base()
        {

        }

        internal HisExpMestBltyReqGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXP_MEST_BLTY_REQ> Get(HisExpMestBltyReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestBltyReqDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_BLTY_REQ GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestBltyReqFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_BLTY_REQ GetById(long id, HisExpMestBltyReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestBltyReqDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_BLTY_REQ> GetByExpMestId(long expMestId)
        {
            HisExpMestBltyReqFilterQuery filter = new HisExpMestBltyReqFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.Get(filter);
        }
    }
}
