using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimIndex
{
    public partial class HisSuimIndexManager : BusinessBase
    {
        
        public List<V_HIS_SUIM_INDEX> GetView(HisSuimIndexViewFilterQuery filter)
        {
            List<V_HIS_SUIM_INDEX> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SUIM_INDEX> resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexGet(param).GetView(filter);
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

        
        public V_HIS_SUIM_INDEX GetViewByCode(string data)
        {
            V_HIS_SUIM_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SUIM_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexGet(param).GetViewByCode(data);
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

        
        public V_HIS_SUIM_INDEX GetViewByCode(string data, HisSuimIndexViewFilterQuery filter)
        {
            V_HIS_SUIM_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SUIM_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_SUIM_INDEX GetViewById(long data)
        {
            V_HIS_SUIM_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SUIM_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexGet(param).GetViewById(data);
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

        
        public V_HIS_SUIM_INDEX GetViewById(long data, HisSuimIndexViewFilterQuery filter)
        {
            V_HIS_SUIM_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SUIM_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexGet(param).GetViewById(data, filter);
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
