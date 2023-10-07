using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServicePaty
{
    public partial class HisServicePatyManager : BusinessBase
    {
        
        public List<V_HIS_SERVICE_PATY> GetView(HisServicePatyViewFilterQuery filter)
        {
            List<V_HIS_SERVICE_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERVICE_PATY> resultData = null;
                if (valid)
                {
                    resultData = new HisServicePatyGet(param).GetView(filter);
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

        
        public V_HIS_SERVICE_PATY GetViewById(long data)
        {
            V_HIS_SERVICE_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERVICE_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisServicePatyGet(param).GetViewById(data);
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

        
        public V_HIS_SERVICE_PATY GetViewById(long data, HisServicePatyViewFilterQuery filter)
        {
            V_HIS_SERVICE_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERVICE_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisServicePatyGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_SERVICE_PATY> GetAppliedView(long serviceId, long? treatmentTime)
        {
            List<V_HIS_SERVICE_PATY> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(serviceId);
                List<V_HIS_SERVICE_PATY> resultData = null;
                if (valid)
                {
                    resultData = new HisServicePatyGet(param).GetAppliedView(serviceId, treatmentTime);
                }
                return resultData;
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
