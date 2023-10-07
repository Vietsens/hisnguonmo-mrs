using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceFollow
{
    public partial class HisServiceFollowManager : BusinessBase
    {
        
        public List<V_HIS_SERVICE_FOLLOW> GetView(HisServiceFollowViewFilterQuery filter)
        {
            List<V_HIS_SERVICE_FOLLOW> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERVICE_FOLLOW> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceFollowGet(param).GetView(filter);
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

        
        public V_HIS_SERVICE_FOLLOW GetViewById(long data)
        {
            V_HIS_SERVICE_FOLLOW result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERVICE_FOLLOW resultData = null;
                if (valid)
                {
                    resultData = new HisServiceFollowGet(param).GetViewById(data);
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

        
        public V_HIS_SERVICE_FOLLOW GetViewById(long data, HisServiceFollowViewFilterQuery filter)
        {
            V_HIS_SERVICE_FOLLOW result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERVICE_FOLLOW resultData = null;
                if (valid)
                {
                    resultData = new HisServiceFollowGet(param).GetViewById(data, filter);
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
