using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRegisterGate
{
    public partial class HisRegisterGateManager : BusinessBase
    {
        public HisRegisterGateManager()
            : base()
        {

        }

        public HisRegisterGateManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_REGISTER_GATE> Get(HisRegisterGateFilterQuery filter)
        {
             List<HIS_REGISTER_GATE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_REGISTER_GATE> resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterGateGet(param).Get(filter);
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

        
        public  HIS_REGISTER_GATE GetById(long data)
        {
             HIS_REGISTER_GATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REGISTER_GATE resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterGateGet(param).GetById(data);
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

        
        public  HIS_REGISTER_GATE GetById(long data, HisRegisterGateFilterQuery filter)
        {
             HIS_REGISTER_GATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REGISTER_GATE resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterGateGet(param).GetById(data, filter);
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

        
        public  HIS_REGISTER_GATE GetByCode(string data)
        {
             HIS_REGISTER_GATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REGISTER_GATE resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterGateGet(param).GetByCode(data);
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

        
        public  HIS_REGISTER_GATE GetByCode(string data, HisRegisterGateFilterQuery filter)
        {
             HIS_REGISTER_GATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REGISTER_GATE resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterGateGet(param).GetByCode(data, filter);
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
