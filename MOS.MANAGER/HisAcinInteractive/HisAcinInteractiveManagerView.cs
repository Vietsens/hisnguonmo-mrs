using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAcinInteractive
{
    public partial class HisAcinInteractiveManager : BusinessBase
    {
        
        public List<V_HIS_ACIN_INTERACTIVE> GetView(HisAcinInteractiveViewFilterQuery filter)
        {
            List<V_HIS_ACIN_INTERACTIVE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_ACIN_INTERACTIVE> resultData = null;
                if (valid)
                {
                    resultData = new HisAcinInteractiveGet(param).GetView(filter);
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

        
        public V_HIS_ACIN_INTERACTIVE GetViewById(long data)
        {
            V_HIS_ACIN_INTERACTIVE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_ACIN_INTERACTIVE resultData = null;
                if (valid)
                {
                    resultData = new HisAcinInteractiveGet(param).GetViewById(data);
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

        
        public V_HIS_ACIN_INTERACTIVE GetViewById(long data, HisAcinInteractiveViewFilterQuery filter)
        {
            V_HIS_ACIN_INTERACTIVE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_ACIN_INTERACTIVE resultData = null;
                if (valid)
                {
                    resultData = new HisAcinInteractiveGet(param).GetViewById(data, filter);
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
