using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipate
{
    public partial class HisAnticipateManager : BusinessBase
    {
        
        public List<V_HIS_ANTICIPATE> GetView(HisAnticipateViewFilterQuery filter)
        {
            List<V_HIS_ANTICIPATE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_ANTICIPATE> resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateGet(param).GetView(filter);
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

        
        public V_HIS_ANTICIPATE GetViewByCode(string data)
        {
            V_HIS_ANTICIPATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_ANTICIPATE resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateGet(param).GetViewByCode(data);
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

        
        public V_HIS_ANTICIPATE GetViewByCode(string data, HisAnticipateViewFilterQuery filter)
        {
            V_HIS_ANTICIPATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_ANTICIPATE resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_ANTICIPATE GetViewById(long data)
        {
            V_HIS_ANTICIPATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_ANTICIPATE resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateGet(param).GetViewById(data);
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

        
        public V_HIS_ANTICIPATE GetViewById(long data, HisAnticipateViewFilterQuery filter)
        {
            V_HIS_ANTICIPATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_ANTICIPATE resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateGet(param).GetViewById(data, filter);
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
