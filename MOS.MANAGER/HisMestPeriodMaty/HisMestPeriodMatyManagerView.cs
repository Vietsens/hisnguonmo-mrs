using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMaty
{
    public partial class HisMestPeriodMatyManager : BusinessBase
    {
        
        public List<V_HIS_MEST_PERIOD_MATY> GetView(HisMestPeriodMatyViewFilterQuery filter)
        {
            List<V_HIS_MEST_PERIOD_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEST_PERIOD_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMatyGet(param).GetView(filter);
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

        
        public V_HIS_MEST_PERIOD_MATY GetViewById(long data)
        {
            V_HIS_MEST_PERIOD_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEST_PERIOD_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMatyGet(param).GetViewById(data);
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

        
        public V_HIS_MEST_PERIOD_MATY GetViewById(long data, HisMestPeriodMatyViewFilterQuery filter)
        {
            V_HIS_MEST_PERIOD_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEST_PERIOD_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMatyGet(param).GetViewById(data, filter);
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
