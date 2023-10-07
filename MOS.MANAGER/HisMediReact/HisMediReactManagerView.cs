using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediReact
{
    public partial class HisMediReactManager : BusinessBase
    {
        
        public List<V_HIS_MEDI_REACT> GetView(HisMediReactViewFilterQuery filter)
        {
            List<V_HIS_MEDI_REACT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDI_REACT> resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactGet(param).GetView(filter);
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

        
        public V_HIS_MEDI_REACT GetViewById(long data)
        {
            V_HIS_MEDI_REACT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDI_REACT resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactGet(param).GetViewById(data);
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

        
        public V_HIS_MEDI_REACT GetViewById(long data, HisMediReactViewFilterQuery filter)
        {
            V_HIS_MEDI_REACT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDI_REACT resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactGet(param).GetViewById(data, filter);
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
