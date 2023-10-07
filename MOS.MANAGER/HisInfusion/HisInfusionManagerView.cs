using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInfusion
{
    public partial class HisInfusionManager : BusinessBase
    {
        
        public List<V_HIS_INFUSION> GetView(HisInfusionViewFilterQuery filter)
        {
            List<V_HIS_INFUSION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_INFUSION> resultData = null;
                if (valid)
                {
                    resultData = new HisInfusionGet(param).GetView(filter);
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

        
        public V_HIS_INFUSION GetViewById(long data)
        {
            V_HIS_INFUSION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_INFUSION resultData = null;
                if (valid)
                {
                    resultData = new HisInfusionGet(param).GetViewById(data);
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

        
        public V_HIS_INFUSION GetViewById(long data, HisInfusionViewFilterQuery filter)
        {
            V_HIS_INFUSION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_INFUSION resultData = null;
                if (valid)
                {
                    resultData = new HisInfusionGet(param).GetViewById(data, filter);
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
