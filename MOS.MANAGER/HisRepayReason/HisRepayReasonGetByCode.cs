using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRepayReason
{
    partial class HisRepayReasonGet : BusinessBase
    {
        internal HIS_REPAY_REASON GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisRepayReasonFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REPAY_REASON GetByCode(string code, HisRepayReasonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRepayReasonDAO.GetByCode(code, filter.Query());
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
