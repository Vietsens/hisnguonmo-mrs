using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRemuneration
{
    public partial class HisRemunerationManager : BusinessBase
    {
        
        public List<V_HIS_REMUNERATION> GetView(HisRemunerationViewFilterQuery filter)
        {
            List<V_HIS_REMUNERATION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_REMUNERATION> resultData = null;
                if (valid)
                {
                    resultData = new HisRemunerationGet(param).GetView(filter);
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

        
        public V_HIS_REMUNERATION GetViewById(long data)
        {
            V_HIS_REMUNERATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_REMUNERATION resultData = null;
                if (valid)
                {
                    resultData = new HisRemunerationGet(param).GetViewById(data);
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

        
        public V_HIS_REMUNERATION GetViewById(long data, HisRemunerationViewFilterQuery filter)
        {
            V_HIS_REMUNERATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_REMUNERATION resultData = null;
                if (valid)
                {
                    resultData = new HisRemunerationGet(param).GetViewById(data, filter);
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
