using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmployee
{
    public partial class HisEmployeeManager : BusinessBase
    {
        public HisEmployeeManager()
            : base()
        {

        }

        public HisEmployeeManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_EMPLOYEE> Get(HisEmployeeFilterQuery filter)
        {
            List<HIS_EMPLOYEE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EMPLOYEE> resultData = null;
                if (valid)
                {
                    resultData = new HisEmployeeGet(param).Get(filter);
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

        
        public HIS_EMPLOYEE GetById(long data)
        {
            HIS_EMPLOYEE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EMPLOYEE resultData = null;
                if (valid)
                {
                    resultData = new HisEmployeeGet(param).GetById(data);
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

        
        public HIS_EMPLOYEE GetById(long data, HisEmployeeFilterQuery filter)
        {
            HIS_EMPLOYEE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EMPLOYEE resultData = null;
                if (valid)
                {
                    resultData = new HisEmployeeGet(param).GetById(data, filter);
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
