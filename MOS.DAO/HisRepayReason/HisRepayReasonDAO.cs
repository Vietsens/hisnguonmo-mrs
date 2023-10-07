using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRepayReason
{
    public partial class HisRepayReasonDAO : EntityBase
    {
        private HisRepayReasonGet GetWorker
        {
            get
            {
                return (HisRepayReasonGet)Worker.Get<HisRepayReasonGet>();
            }
        }
        public List<HIS_REPAY_REASON> Get(HisRepayReasonSO search, CommonParam param)
        {
            List<HIS_REPAY_REASON> result = new List<HIS_REPAY_REASON>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_REPAY_REASON GetById(long id, HisRepayReasonSO search)
        {
            HIS_REPAY_REASON result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
