using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPayForm
{
    public partial class HisPayFormManager : BusinessBase
    {
        public HisPayFormManager()
            : base()
        {

        }

        public HisPayFormManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_PAY_FORM> Get(HisPayFormFilterQuery filter)
        {
            List<HIS_PAY_FORM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PAY_FORM> resultData = null;
                if (valid)
                {
                    resultData = new HisPayFormGet(param).Get(filter);
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

        
        public HIS_PAY_FORM GetById(long data)
        {
            HIS_PAY_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PAY_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisPayFormGet(param).GetById(data);
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

        
        public HIS_PAY_FORM GetById(long data, HisPayFormFilterQuery filter)
        {
            HIS_PAY_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PAY_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisPayFormGet(param).GetById(data, filter);
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

        
        public HIS_PAY_FORM GetByCode(string data)
        {
            HIS_PAY_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PAY_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisPayFormGet(param).GetByCode(data);
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

        
        public HIS_PAY_FORM GetByCode(string data, HisPayFormFilterQuery filter)
        {
            HIS_PAY_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PAY_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisPayFormGet(param).GetByCode(data, filter);
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
