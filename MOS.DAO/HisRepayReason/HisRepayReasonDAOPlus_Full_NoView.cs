using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRepayReason
{
    public partial class HisRepayReasonDAO : EntityBase
    {
        public HIS_REPAY_REASON GetByCode(string code, HisRepayReasonSO search)
        {
            HIS_REPAY_REASON result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_REPAY_REASON> GetDicByCode(HisRepayReasonSO search, CommonParam param)
        {
            Dictionary<string, HIS_REPAY_REASON> result = new Dictionary<string, HIS_REPAY_REASON>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
