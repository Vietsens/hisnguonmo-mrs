using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceUnit
{
    public partial class HisServiceUnitManager : BusinessBase
    {
        public HisServiceUnitManager()
            : base()
        {

        }

        public HisServiceUnitManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_UNIT> Get(HisServiceUnitFilterQuery filter)
        {
             List<HIS_SERVICE_UNIT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_UNIT> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceUnitGet(param).Get(filter);
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

        
        public  HIS_SERVICE_UNIT GetById(long data)
        {
             HIS_SERVICE_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisServiceUnitGet(param).GetById(data);
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

        
        public  HIS_SERVICE_UNIT GetById(long data, HisServiceUnitFilterQuery filter)
        {
             HIS_SERVICE_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisServiceUnitGet(param).GetById(data, filter);
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

        
        public  HIS_SERVICE_UNIT GetByCode(string data)
        {
             HIS_SERVICE_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisServiceUnitGet(param).GetByCode(data);
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

        
        public  HIS_SERVICE_UNIT GetByCode(string data, HisServiceUnitFilterQuery filter)
        {
             HIS_SERVICE_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisServiceUnitGet(param).GetByCode(data, filter);
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
