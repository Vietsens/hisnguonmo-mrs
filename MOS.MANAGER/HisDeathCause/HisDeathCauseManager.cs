using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDeathCause
{
    public partial class HisDeathCauseManager : BusinessBase
    {
        public HisDeathCauseManager()
            : base()
        {

        }

        public HisDeathCauseManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_DEATH_CAUSE> Get(HisDeathCauseFilterQuery filter)
        {
             List<HIS_DEATH_CAUSE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DEATH_CAUSE> resultData = null;
                if (valid)
                {
                    resultData = new HisDeathCauseGet(param).Get(filter);
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

        
        public  HIS_DEATH_CAUSE GetById(long data)
        {
             HIS_DEATH_CAUSE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DEATH_CAUSE resultData = null;
                if (valid)
                {
                    resultData = new HisDeathCauseGet(param).GetById(data);
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

        
        public  HIS_DEATH_CAUSE GetById(long data, HisDeathCauseFilterQuery filter)
        {
             HIS_DEATH_CAUSE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DEATH_CAUSE resultData = null;
                if (valid)
                {
                    resultData = new HisDeathCauseGet(param).GetById(data, filter);
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

        
        public  HIS_DEATH_CAUSE GetByCode(string data)
        {
             HIS_DEATH_CAUSE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DEATH_CAUSE resultData = null;
                if (valid)
                {
                    resultData = new HisDeathCauseGet(param).GetByCode(data);
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

        
        public  HIS_DEATH_CAUSE GetByCode(string data, HisDeathCauseFilterQuery filter)
        {
             HIS_DEATH_CAUSE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DEATH_CAUSE resultData = null;
                if (valid)
                {
                    resultData = new HisDeathCauseGet(param).GetByCode(data, filter);
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
