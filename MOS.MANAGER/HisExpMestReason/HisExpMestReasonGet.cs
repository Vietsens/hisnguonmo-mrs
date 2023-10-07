using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestReason
{
    partial class HisExpMestReasonGet : BusinessBase
    {
        internal HisExpMestReasonGet()
            : base()
        {

        }

        internal HisExpMestReasonGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXP_MEST_REASON> Get(HisExpMestReasonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestReasonDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_REASON GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestReasonFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_REASON GetById(long id, HisExpMestReasonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestReasonDAO.GetById(id, filter.Query());
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
