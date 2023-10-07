using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDebate
{
    public partial class HisDebateManager : BusinessBase
    {
        public HisDebateManager()
            : base()
        {

        }

        public HisDebateManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_DEBATE> Get(HisDebateFilterQuery filter)
        {
            List<HIS_DEBATE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DEBATE> resultData = null;
                if (valid)
                {
                    resultData = new HisDebateGet(param).Get(filter);
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

        
        public HIS_DEBATE GetById(long data)
        {
            HIS_DEBATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DEBATE resultData = null;
                if (valid)
                {
                    resultData = new HisDebateGet(param).GetById(data);
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

        
        public HIS_DEBATE GetById(long data, HisDebateFilterQuery filter)
        {
            HIS_DEBATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DEBATE resultData = null;
                if (valid)
                {
                    resultData = new HisDebateGet(param).GetById(data, filter);
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
