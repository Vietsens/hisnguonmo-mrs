using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepositReq
{
    public partial class HisDepositReqManager : BusinessBase
    {
        public HisDepositReqManager()
            : base()
        {

        }

        public HisDepositReqManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_DEPOSIT_REQ> Get(HisDepositReqFilterQuery filter)
        {
             List<HIS_DEPOSIT_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DEPOSIT_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisDepositReqGet(param).Get(filter);
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

        
        public  HIS_DEPOSIT_REQ GetById(long data)
        {
             HIS_DEPOSIT_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DEPOSIT_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisDepositReqGet(param).GetById(data);
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

        
        public  HIS_DEPOSIT_REQ GetById(long data, HisDepositReqFilterQuery filter)
        {
             HIS_DEPOSIT_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DEPOSIT_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisDepositReqGet(param).GetById(data, filter);
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

        
        public  HIS_DEPOSIT_REQ GetByCode(string data)
        {
             HIS_DEPOSIT_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DEPOSIT_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisDepositReqGet(param).GetByCode(data);
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

        
        public  HIS_DEPOSIT_REQ GetByCode(string data, HisDepositReqFilterQuery filter)
        {
             HIS_DEPOSIT_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DEPOSIT_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisDepositReqGet(param).GetByCode(data, filter);
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
