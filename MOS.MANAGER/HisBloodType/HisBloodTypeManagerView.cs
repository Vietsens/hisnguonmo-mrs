using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodType
{
    public partial class HisBloodTypeManager : BusinessBase
    {
        
        public List<V_HIS_BLOOD_TYPE> GetView(HisBloodTypeViewFilterQuery filter)
        {
            List<V_HIS_BLOOD_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_BLOOD_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisBloodTypeGet(param).GetView(filter);
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

        
        public V_HIS_BLOOD_TYPE GetViewByCode(string data)
        {
            V_HIS_BLOOD_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_BLOOD_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBloodTypeGet(param).GetViewByCode(data);
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

        
        public V_HIS_BLOOD_TYPE GetViewByCode(string data, HisBloodTypeViewFilterQuery filter)
        {
            V_HIS_BLOOD_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_BLOOD_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBloodTypeGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_BLOOD_TYPE GetViewById(long data)
        {
            V_HIS_BLOOD_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_BLOOD_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBloodTypeGet(param).GetViewById(data);
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

        
        public V_HIS_BLOOD_TYPE GetViewById(long data, HisBloodTypeViewFilterQuery filter)
        {
            V_HIS_BLOOD_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_BLOOD_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBloodTypeGet(param).GetViewById(data, filter);
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
