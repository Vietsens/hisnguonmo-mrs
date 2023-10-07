using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInfusion
{
    public partial class HisInfusionManager : BusinessBase
    {
        public HisInfusionManager()
            : base()
        {

        }

        public HisInfusionManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_INFUSION> Get(HisInfusionFilterQuery filter)
        {
            List<HIS_INFUSION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_INFUSION> resultData = null;
                if (valid)
                {
                    resultData = new HisInfusionGet(param).Get(filter);
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

        
        public HIS_INFUSION GetById(long data)
        {
            HIS_INFUSION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_INFUSION resultData = null;
                if (valid)
                {
                    resultData = new HisInfusionGet(param).GetById(data);
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

        
        public HIS_INFUSION GetById(long data, HisInfusionFilterQuery filter)
        {
            HIS_INFUSION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_INFUSION resultData = null;
                if (valid)
                {
                    resultData = new HisInfusionGet(param).GetById(data, filter);
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

        
        public List<HIS_INFUSION> GetByInfusionSumId(long data)
        {
            List<HIS_INFUSION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_INFUSION> resultData = null;
                if (valid)
                {
                    resultData = new HisInfusionGet(param).GetByInfusionSumId(data);
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
