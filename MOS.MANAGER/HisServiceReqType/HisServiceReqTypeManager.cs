using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceReqType
{
    public partial class HisServiceReqTypeManager : BusinessBase
    {
        public HisServiceReqTypeManager()
            : base()
        {

        }

        public HisServiceReqTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_REQ_TYPE> Get(HisServiceReqTypeFilterQuery filter)
        {
             List<HIS_SERVICE_REQ_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_REQ_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqTypeGet(param).Get(filter);
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

        
        public  HIS_SERVICE_REQ_TYPE GetById(long data)
        {
             HIS_SERVICE_REQ_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_REQ_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqTypeGet(param).GetById(data);
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

        
        public  HIS_SERVICE_REQ_TYPE GetById(long data, HisServiceReqTypeFilterQuery filter)
        {
             HIS_SERVICE_REQ_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_REQ_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqTypeGet(param).GetById(data, filter);
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

        
        public  HIS_SERVICE_REQ_TYPE GetByCode(string data)
        {
             HIS_SERVICE_REQ_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_REQ_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqTypeGet(param).GetByCode(data);
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

        
        public  HIS_SERVICE_REQ_TYPE GetByCode(string data, HisServiceReqTypeFilterQuery filter)
        {
             HIS_SERVICE_REQ_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_REQ_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqTypeGet(param).GetByCode(data, filter);
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
