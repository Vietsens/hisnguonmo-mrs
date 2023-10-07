using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceReqStt
{
    public partial class HisServiceReqSttManager : BusinessBase
    {
        public HisServiceReqSttManager()
            : base()
        {

        }

        public HisServiceReqSttManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_REQ_STT> Get(HisServiceReqSttFilterQuery filter)
        {
             List<HIS_SERVICE_REQ_STT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_REQ_STT> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqSttGet(param).Get(filter);
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

        
        public  HIS_SERVICE_REQ_STT GetById(long data)
        {
             HIS_SERVICE_REQ_STT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_REQ_STT resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqSttGet(param).GetById(data);
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

        
        public  HIS_SERVICE_REQ_STT GetById(long data, HisServiceReqSttFilterQuery filter)
        {
             HIS_SERVICE_REQ_STT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_REQ_STT resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqSttGet(param).GetById(data, filter);
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

        
        public  HIS_SERVICE_REQ_STT GetByCode(string data)
        {
             HIS_SERVICE_REQ_STT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_REQ_STT resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqSttGet(param).GetByCode(data);
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

        
        public  HIS_SERVICE_REQ_STT GetByCode(string data, HisServiceReqSttFilterQuery filter)
        {
             HIS_SERVICE_REQ_STT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_REQ_STT resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqSttGet(param).GetByCode(data, filter);
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
