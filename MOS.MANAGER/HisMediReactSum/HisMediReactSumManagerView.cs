using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediReactSum
{
    public partial class HisMediReactSumManager : BusinessBase
    {
        
        public List<V_HIS_MEDI_REACT_SUM> GetView(HisMediReactSumViewFilterQuery filter)
        {
            List<V_HIS_MEDI_REACT_SUM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDI_REACT_SUM> resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactSumGet(param).GetView(filter);
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

        
        public V_HIS_MEDI_REACT_SUM GetViewById(long data)
        {
            V_HIS_MEDI_REACT_SUM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDI_REACT_SUM resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactSumGet(param).GetViewById(data);
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

        
        public V_HIS_MEDI_REACT_SUM GetViewById(long data, HisMediReactSumViewFilterQuery filter)
        {
            V_HIS_MEDI_REACT_SUM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDI_REACT_SUM resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactSumGet(param).GetViewById(data, filter);
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
