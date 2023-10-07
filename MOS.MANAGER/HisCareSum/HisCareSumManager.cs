using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCareSum
{
    public partial class HisCareSumManager : BusinessBase
    {
        public HisCareSumManager()
            : base()
        {

        }

        public HisCareSumManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_CARE_SUM> Get(HisCareSumFilterQuery filter)
        {
            List<HIS_CARE_SUM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_CARE_SUM> resultData = null;
                if (valid)
                {
                    resultData = new HisCareSumGet(param).Get(filter);
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

        
        public HIS_CARE_SUM GetById(long data)
        {
            HIS_CARE_SUM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_CARE_SUM resultData = null;
                if (valid)
                {
                    resultData = new HisCareSumGet(param).GetById(data);
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

        
        public HIS_CARE_SUM GetById(long data, HisCareSumFilterQuery filter)
        {
            HIS_CARE_SUM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_CARE_SUM resultData = null;
                if (valid)
                {
                    resultData = new HisCareSumGet(param).GetById(data, filter);
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
