using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPatySub
{
    public partial class HisMestPatySubManager : BusinessBase
    {
        
        public List<V_HIS_MEST_PATY_SUB> GetView(HisMestPatySubViewFilterQuery filter)
        {
            List<V_HIS_MEST_PATY_SUB> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEST_PATY_SUB> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatySubGet(param).GetView(filter);
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

        
        public V_HIS_MEST_PATY_SUB GetViewById(long data)
        {
            V_HIS_MEST_PATY_SUB result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEST_PATY_SUB resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatySubGet(param).GetViewById(data);
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

        
        public V_HIS_MEST_PATY_SUB GetViewById(long data, HisMestPatySubViewFilterQuery filter)
        {
            V_HIS_MEST_PATY_SUB result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEST_PATY_SUB resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatySubGet(param).GetViewById(data, filter);
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
