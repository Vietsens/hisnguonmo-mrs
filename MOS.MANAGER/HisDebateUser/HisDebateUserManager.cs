using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDebateUser
{
    public partial class HisDebateUserManager : BusinessBase
    {
        public HisDebateUserManager()
            : base()
        {

        }

        public HisDebateUserManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_DEBATE_USER> Get(HisDebateUserFilterQuery filter)
        {
             List<HIS_DEBATE_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DEBATE_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisDebateUserGet(param).Get(filter);
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

        
        public  HIS_DEBATE_USER GetById(long data)
        {
             HIS_DEBATE_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DEBATE_USER resultData = null;
                if (valid)
                {
                    resultData = new HisDebateUserGet(param).GetById(data);
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

        
        public  HIS_DEBATE_USER GetById(long data, HisDebateUserFilterQuery filter)
        {
             HIS_DEBATE_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DEBATE_USER resultData = null;
                if (valid)
                {
                    resultData = new HisDebateUserGet(param).GetById(data, filter);
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
