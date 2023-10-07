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
        internal HIS_EXP_MEST_REASON GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisExpMestReasonFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_REASON GetByCode(string code, HisExpMestReasonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestReasonDAO.GetByCode(code, filter.Query());
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
