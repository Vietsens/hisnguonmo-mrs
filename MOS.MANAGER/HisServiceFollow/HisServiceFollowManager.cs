using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceFollow
{
    public partial class HisServiceFollowManager : BusinessBase
    {
        public HisServiceFollowManager()
            : base()
        {

        }

        public HisServiceFollowManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_FOLLOW> Get(HisServiceFollowFilterQuery filter)
        {
             List<HIS_SERVICE_FOLLOW> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_FOLLOW> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceFollowGet(param).Get(filter);
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

        
        public  HIS_SERVICE_FOLLOW GetById(long data)
        {
             HIS_SERVICE_FOLLOW result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_FOLLOW resultData = null;
                if (valid)
                {
                    resultData = new HisServiceFollowGet(param).GetById(data);
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

        
        public  HIS_SERVICE_FOLLOW GetById(long data, HisServiceFollowFilterQuery filter)
        {
             HIS_SERVICE_FOLLOW result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_FOLLOW resultData = null;
                if (valid)
                {
                    resultData = new HisServiceFollowGet(param).GetById(data, filter);
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
