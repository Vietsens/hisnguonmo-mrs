using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRepayReason
{
    public partial class HisRepayReasonManager : BusinessBase
    {
        public HisRepayReasonManager()
            : base()
        {

        }

        public HisRepayReasonManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_REPAY_REASON> Get(HisRepayReasonFilterQuery filter)
        {
             List<HIS_REPAY_REASON> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_REPAY_REASON> resultData = null;
                if (valid)
                {
                    resultData = new HisRepayReasonGet(param).Get(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  HIS_REPAY_REASON GetById(long data)
        {
             HIS_REPAY_REASON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REPAY_REASON resultData = null;
                if (valid)
                {
                    resultData = new HisRepayReasonGet(param).GetById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  HIS_REPAY_REASON GetById(long data, HisRepayReasonFilterQuery filter)
        {
             HIS_REPAY_REASON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REPAY_REASON resultData = null;
                if (valid)
                {
                    resultData = new HisRepayReasonGet(param).GetById(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  HIS_REPAY_REASON GetByCode(string data)
        {
             HIS_REPAY_REASON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REPAY_REASON resultData = null;
                if (valid)
                {
                    resultData = new HisRepayReasonGet(param).GetByCode(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  HIS_REPAY_REASON GetByCode(string data, HisRepayReasonFilterQuery filter)
        {
             HIS_REPAY_REASON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REPAY_REASON resultData = null;
                if (valid)
                {
                    resultData = new HisRepayReasonGet(param).GetByCode(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
