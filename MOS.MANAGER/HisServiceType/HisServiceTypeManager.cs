using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceType
{
    public partial class HisServiceTypeManager : BusinessBase
    {
        public HisServiceTypeManager()
            : base()
        {

        }

        public HisServiceTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_TYPE> Get(HisServiceTypeFilterQuery filter)
        {
             List<HIS_SERVICE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceTypeGet(param).Get(filter);
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

        
        public  HIS_SERVICE_TYPE GetById(long data)
        {
             HIS_SERVICE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisServiceTypeGet(param).GetById(data);
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

        
        public  HIS_SERVICE_TYPE GetById(long data, HisServiceTypeFilterQuery filter)
        {
             HIS_SERVICE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisServiceTypeGet(param).GetById(data, filter);
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

        
        public  HIS_SERVICE_TYPE GetByCode(string data)
        {
             HIS_SERVICE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisServiceTypeGet(param).GetByCode(data);
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

        
        public  HIS_SERVICE_TYPE GetByCode(string data, HisServiceTypeFilterQuery filter)
        {
             HIS_SERVICE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisServiceTypeGet(param).GetByCode(data, filter);
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
