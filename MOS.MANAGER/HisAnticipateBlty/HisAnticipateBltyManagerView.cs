using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateBlty
{
    public partial class HisAnticipateBltyManager : BusinessBase
    {
        
        public List<V_HIS_ANTICIPATE_BLTY> GetView(HisAnticipateBltyViewFilterQuery filter)
        {
            List<V_HIS_ANTICIPATE_BLTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_ANTICIPATE_BLTY> resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateBltyGet(param).GetView(filter);
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

        
        public V_HIS_ANTICIPATE_BLTY GetViewById(long data)
        {
            V_HIS_ANTICIPATE_BLTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_ANTICIPATE_BLTY resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateBltyGet(param).GetViewById(data);
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

        
        public V_HIS_ANTICIPATE_BLTY GetViewById(long data, HisAnticipateBltyViewFilterQuery filter)
        {
            V_HIS_ANTICIPATE_BLTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_ANTICIPATE_BLTY resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateBltyGet(param).GetViewById(data, filter);
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
