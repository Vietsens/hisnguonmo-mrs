using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAwareness
{
    public partial class HisAwarenessManager : BusinessBase
    {
        public HisAwarenessManager()
            : base()
        {

        }

        public HisAwarenessManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_AWARENESS> Get(HisAwarenessFilterQuery filter)
        {
             List<HIS_AWARENESS> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_AWARENESS> resultData = null;
                if (valid)
                {
                    resultData = new HisAwarenessGet(param).Get(filter);
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

        
        public  HIS_AWARENESS GetById(long data)
        {
             HIS_AWARENESS result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_AWARENESS resultData = null;
                if (valid)
                {
                    resultData = new HisAwarenessGet(param).GetById(data);
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

        
        public  HIS_AWARENESS GetById(long data, HisAwarenessFilterQuery filter)
        {
             HIS_AWARENESS result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_AWARENESS resultData = null;
                if (valid)
                {
                    resultData = new HisAwarenessGet(param).GetById(data, filter);
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

        
        public  HIS_AWARENESS GetByCode(string data)
        {
             HIS_AWARENESS result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_AWARENESS resultData = null;
                if (valid)
                {
                    resultData = new HisAwarenessGet(param).GetByCode(data);
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

        
        public  HIS_AWARENESS GetByCode(string data, HisAwarenessFilterQuery filter)
        {
             HIS_AWARENESS result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_AWARENESS resultData = null;
                if (valid)
                {
                    resultData = new HisAwarenessGet(param).GetByCode(data, filter);
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
