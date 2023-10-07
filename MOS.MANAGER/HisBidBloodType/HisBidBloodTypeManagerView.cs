using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidBloodType
{
    public partial class HisBidBloodTypeManager : BusinessBase
    {
        
        public List<V_HIS_BID_BLOOD_TYPE> GetView(HisBidBloodTypeViewFilterQuery filter)
        {
            List<V_HIS_BID_BLOOD_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_BID_BLOOD_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisBidBloodTypeGet(param).GetView(filter);
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

        
        public V_HIS_BID_BLOOD_TYPE GetViewById(long data)
        {
            V_HIS_BID_BLOOD_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_BID_BLOOD_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidBloodTypeGet(param).GetViewById(data);
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

        
        public V_HIS_BID_BLOOD_TYPE GetViewById(long data, HisBidBloodTypeViewFilterQuery filter)
        {
            V_HIS_BID_BLOOD_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_BID_BLOOD_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidBloodTypeGet(param).GetViewById(data, filter);
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
