using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisHeinServiceType
{
    public partial class HisHeinServiceTypeManager : BusinessBase
    {
        public HisHeinServiceTypeManager()
            : base()
        {

        }

        public HisHeinServiceTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_HEIN_SERVICE_TYPE> Get(HisHeinServiceTypeFilterQuery filter)
        {
             List<HIS_HEIN_SERVICE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_HEIN_SERVICE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisHeinServiceTypeGet(param).Get(filter);
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

        
        public  HIS_HEIN_SERVICE_TYPE GetById(long data)
        {
             HIS_HEIN_SERVICE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_HEIN_SERVICE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisHeinServiceTypeGet(param).GetById(data);
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

        
        public  HIS_HEIN_SERVICE_TYPE GetById(long data, HisHeinServiceTypeFilterQuery filter)
        {
             HIS_HEIN_SERVICE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_HEIN_SERVICE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisHeinServiceTypeGet(param).GetById(data, filter);
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

        
        public  HIS_HEIN_SERVICE_TYPE GetByCode(string data)
        {
             HIS_HEIN_SERVICE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_HEIN_SERVICE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisHeinServiceTypeGet(param).GetByCode(data);
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

        
        public  HIS_HEIN_SERVICE_TYPE GetByCode(string data, HisHeinServiceTypeFilterQuery filter)
        {
             HIS_HEIN_SERVICE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_HEIN_SERVICE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisHeinServiceTypeGet(param).GetByCode(data, filter);
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
