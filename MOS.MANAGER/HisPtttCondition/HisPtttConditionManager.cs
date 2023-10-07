using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttCondition
{
    public partial class HisPtttConditionManager : BusinessBase
    {
        public HisPtttConditionManager()
            : base()
        {

        }

        public HisPtttConditionManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_PTTT_CONDITION> Get(HisPtttConditionFilterQuery filter)
        {
             List<HIS_PTTT_CONDITION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PTTT_CONDITION> resultData = null;
                if (valid)
                {
                    resultData = new HisPtttConditionGet(param).Get(filter);
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

        
        public  HIS_PTTT_CONDITION GetById(long data)
        {
             HIS_PTTT_CONDITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_CONDITION resultData = null;
                if (valid)
                {
                    resultData = new HisPtttConditionGet(param).GetById(data);
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

        
        public  HIS_PTTT_CONDITION GetById(long data, HisPtttConditionFilterQuery filter)
        {
             HIS_PTTT_CONDITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_CONDITION resultData = null;
                if (valid)
                {
                    resultData = new HisPtttConditionGet(param).GetById(data, filter);
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

        
        public  HIS_PTTT_CONDITION GetByCode(string data)
        {
             HIS_PTTT_CONDITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_CONDITION resultData = null;
                if (valid)
                {
                    resultData = new HisPtttConditionGet(param).GetByCode(data);
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

        
        public  HIS_PTTT_CONDITION GetByCode(string data, HisPtttConditionFilterQuery filter)
        {
             HIS_PTTT_CONDITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_CONDITION resultData = null;
                if (valid)
                {
                    resultData = new HisPtttConditionGet(param).GetByCode(data, filter);
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
