using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRegisterReq
{
    public partial class HisRegisterReqManager : BusinessBase
    {
        public HisRegisterReqManager()
            : base()
        {

        }

        public HisRegisterReqManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_REGISTER_REQ> Get(HisRegisterReqFilterQuery filter)
        {
             List<HIS_REGISTER_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_REGISTER_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterReqGet(param).Get(filter);
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

        
        public  HIS_REGISTER_REQ GetById(long data)
        {
             HIS_REGISTER_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REGISTER_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterReqGet(param).GetById(data);
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

        
        public  HIS_REGISTER_REQ GetById(long data, HisRegisterReqFilterQuery filter)
        {
             HIS_REGISTER_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REGISTER_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterReqGet(param).GetById(data, filter);
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

        
        public  List<HIS_REGISTER_REQ> GetByRegisterGateId(long data)
        {
             List<HIS_REGISTER_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_REGISTER_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterReqGet(param).GetByRegisterGateId(data);
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
