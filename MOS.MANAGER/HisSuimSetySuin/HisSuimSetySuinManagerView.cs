using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimSetySuin
{
    public partial class HisSuimSetySuinManager : BusinessBase
    {
        
        public List<V_HIS_SUIM_SETY_SUIN> GetView(HisSuimSetySuinViewFilterQuery filter)
        {
            List<V_HIS_SUIM_SETY_SUIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SUIM_SETY_SUIN> resultData = null;
                if (valid)
                {
                    resultData = new HisSuimSetySuinGet(param).GetView(filter);
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

        
        public V_HIS_SUIM_SETY_SUIN GetViewById(long data)
        {
            V_HIS_SUIM_SETY_SUIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SUIM_SETY_SUIN resultData = null;
                if (valid)
                {
                    resultData = new HisSuimSetySuinGet(param).GetViewById(data);
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

        
        public V_HIS_SUIM_SETY_SUIN GetViewById(long data, HisSuimSetySuinViewFilterQuery filter)
        {
            V_HIS_SUIM_SETY_SUIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SUIM_SETY_SUIN resultData = null;
                if (valid)
                {
                    resultData = new HisSuimSetySuinGet(param).GetViewById(data, filter);
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
