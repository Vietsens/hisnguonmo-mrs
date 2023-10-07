using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentHurt
{
    public partial class HisAccidentHurtManager : BusinessBase
    {
        
        public List<V_HIS_ACCIDENT_HURT> GetView(HisAccidentHurtViewFilterQuery filter)
        {
            List<V_HIS_ACCIDENT_HURT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_ACCIDENT_HURT> resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHurtGet(param).GetView(filter);
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

        
        public V_HIS_ACCIDENT_HURT GetViewById(long data)
        {
            V_HIS_ACCIDENT_HURT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_ACCIDENT_HURT resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHurtGet(param).GetViewById(data);
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

        
        public V_HIS_ACCIDENT_HURT GetViewById(long data, HisAccidentHurtViewFilterQuery filter)
        {
            V_HIS_ACCIDENT_HURT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_ACCIDENT_HURT resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHurtGet(param).GetViewById(data, filter);
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
