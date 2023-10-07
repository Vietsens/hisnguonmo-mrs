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
        internal HisRepayReasonGet()
            : base()
        {

        }

        internal HisRepayReasonGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_REPAY_REASON> Get(HisRepayReasonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRepayReasonDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REPAY_REASON GetById(long id)
        {
            try
            {
                return GetById(id, new HisRepayReasonFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REPAY_REASON GetById(long id, HisRepayReasonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRepayReasonDAO.GetById(id, filter.Query());
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
