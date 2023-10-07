using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestReason
{
    public partial class HisExpMestReasonManager : BusinessBase
    {
        public HisExpMestReasonManager()
            : base()
        {

        }

        public HisExpMestReasonManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EXP_MEST_REASON> Get(HisExpMestReasonFilterQuery filter)
        {
             List<HIS_EXP_MEST_REASON> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXP_MEST_REASON> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestReasonGet(param).Get(filter);
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

        
        public  HIS_EXP_MEST_REASON GetById(long data)
        {
             HIS_EXP_MEST_REASON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXP_MEST_REASON resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestReasonGet(param).GetById(data);
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

        
        public  HIS_EXP_MEST_REASON GetById(long data, HisExpMestReasonFilterQuery filter)
        {
             HIS_EXP_MEST_REASON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXP_MEST_REASON resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestReasonGet(param).GetById(data, filter);
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
