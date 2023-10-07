using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTracking
{
    public partial class HisTrackingManager : BusinessBase
    {
        
        public List<V_HIS_TRACKING> GetView(HisTrackingViewFilterQuery filter)
        {
            List<V_HIS_TRACKING> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_TRACKING> resultData = null;
                if (valid)
                {
                    resultData = new HisTrackingGet(param).GetView(filter);
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

        
        public List<V_HIS_TRACKING> GetViewByIds(List<long> ids)
        {
            List<V_HIS_TRACKING> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(ids);
                List<V_HIS_TRACKING> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_TRACKING>();
                    var skip = 0;
                    while (ids.Count - skip > 0)
                    {
                        var Ids = ids.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisTrackingGet(param).GetViewByIds(Ids));
                    }
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
