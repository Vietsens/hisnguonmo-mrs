using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmergencyWtime
{
    public partial class HisEmergencyWtimeManager : BusinessBase
    {
        public HisEmergencyWtimeManager()
            : base()
        {

        }

        public HisEmergencyWtimeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EMERGENCY_WTIME> Get(HisEmergencyWtimeFilterQuery filter)
        {
             List<HIS_EMERGENCY_WTIME> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EMERGENCY_WTIME> resultData = null;
                if (valid)
                {
                    resultData = new HisEmergencyWtimeGet(param).Get(filter);
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

        
        public  HIS_EMERGENCY_WTIME GetById(long data)
        {
             HIS_EMERGENCY_WTIME result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EMERGENCY_WTIME resultData = null;
                if (valid)
                {
                    resultData = new HisEmergencyWtimeGet(param).GetById(data);
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

        
        public  HIS_EMERGENCY_WTIME GetById(long data, HisEmergencyWtimeFilterQuery filter)
        {
             HIS_EMERGENCY_WTIME result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EMERGENCY_WTIME resultData = null;
                if (valid)
                {
                    resultData = new HisEmergencyWtimeGet(param).GetById(data, filter);
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

        
        public  HIS_EMERGENCY_WTIME GetByCode(string data)
        {
             HIS_EMERGENCY_WTIME result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EMERGENCY_WTIME resultData = null;
                if (valid)
                {
                    resultData = new HisEmergencyWtimeGet(param).GetByCode(data);
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

        
        public  HIS_EMERGENCY_WTIME GetByCode(string data, HisEmergencyWtimeFilterQuery filter)
        {
             HIS_EMERGENCY_WTIME result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EMERGENCY_WTIME resultData = null;
                if (valid)
                {
                    resultData = new HisEmergencyWtimeGet(param).GetByCode(data, filter);
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
