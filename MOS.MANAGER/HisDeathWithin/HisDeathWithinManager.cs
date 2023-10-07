using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDeathWithin
{
    public partial class HisDeathWithinManager : BusinessBase
    {
        public HisDeathWithinManager()
            : base()
        {

        }

        public HisDeathWithinManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_DEATH_WITHIN> Get(HisDeathWithinFilterQuery filter)
        {
             List<HIS_DEATH_WITHIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DEATH_WITHIN> resultData = null;
                if (valid)
                {
                    resultData = new HisDeathWithinGet(param).Get(filter);
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

        
        public  HIS_DEATH_WITHIN GetById(long data)
        {
             HIS_DEATH_WITHIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DEATH_WITHIN resultData = null;
                if (valid)
                {
                    resultData = new HisDeathWithinGet(param).GetById(data);
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

        
        public  HIS_DEATH_WITHIN GetById(long data, HisDeathWithinFilterQuery filter)
        {
             HIS_DEATH_WITHIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DEATH_WITHIN resultData = null;
                if (valid)
                {
                    resultData = new HisDeathWithinGet(param).GetById(data, filter);
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

        
        public  HIS_DEATH_WITHIN GetByCode(string data)
        {
             HIS_DEATH_WITHIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DEATH_WITHIN resultData = null;
                if (valid)
                {
                    resultData = new HisDeathWithinGet(param).GetByCode(data);
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

        
        public  HIS_DEATH_WITHIN GetByCode(string data, HisDeathWithinFilterQuery filter)
        {
             HIS_DEATH_WITHIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DEATH_WITHIN resultData = null;
                if (valid)
                {
                    resultData = new HisDeathWithinGet(param).GetByCode(data, filter);
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
