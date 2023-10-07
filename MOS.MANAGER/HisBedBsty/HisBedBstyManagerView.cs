using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedBsty
{
    public partial class HisBedBstyManager : BusinessBase
    {
        
        public List<V_HIS_BED_BSTY> GetView(HisBedBstyViewFilterQuery filter)
        {
            List<V_HIS_BED_BSTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_BED_BSTY> resultData = null;
                if (valid)
                {
                    resultData = new HisBedBstyGet(param).GetView(filter);
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

        
        public V_HIS_BED_BSTY GetViewById(long data)
        {
            V_HIS_BED_BSTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_BED_BSTY resultData = null;
                if (valid)
                {
                    resultData = new HisBedBstyGet(param).GetViewById(data);
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

        
        public V_HIS_BED_BSTY GetViewById(long data, HisBedBstyViewFilterQuery filter)
        {
            V_HIS_BED_BSTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_BED_BSTY resultData = null;
                if (valid)
                {
                    resultData = new HisBedBstyGet(param).GetViewById(data, filter);
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
