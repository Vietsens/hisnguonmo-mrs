using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttPriority
{
    public partial class HisPtttPriorityManager : BusinessBase
    {
        public HisPtttPriorityManager()
            : base()
        {

        }

        public HisPtttPriorityManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_PTTT_PRIORITY> Get(HisPtttPriorityFilterQuery filter)
        {
             List<HIS_PTTT_PRIORITY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PTTT_PRIORITY> resultData = null;
                if (valid)
                {
                    resultData = new HisPtttPriorityGet(param).Get(filter);
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

        
        public  HIS_PTTT_PRIORITY GetById(long data)
        {
             HIS_PTTT_PRIORITY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_PRIORITY resultData = null;
                if (valid)
                {
                    resultData = new HisPtttPriorityGet(param).GetById(data);
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

        
        public  HIS_PTTT_PRIORITY GetById(long data, HisPtttPriorityFilterQuery filter)
        {
             HIS_PTTT_PRIORITY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_PRIORITY resultData = null;
                if (valid)
                {
                    resultData = new HisPtttPriorityGet(param).GetById(data, filter);
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

        
        public  HIS_PTTT_PRIORITY GetByCode(string data)
        {
             HIS_PTTT_PRIORITY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_PRIORITY resultData = null;
                if (valid)
                {
                    resultData = new HisPtttPriorityGet(param).GetByCode(data);
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

        
        public  HIS_PTTT_PRIORITY GetByCode(string data, HisPtttPriorityFilterQuery filter)
        {
             HIS_PTTT_PRIORITY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_PRIORITY resultData = null;
                if (valid)
                {
                    resultData = new HisPtttPriorityGet(param).GetByCode(data, filter);
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
