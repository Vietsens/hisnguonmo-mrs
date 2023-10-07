using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccountBook
{
    public partial class HisAccountBookManager : BusinessBase
    {
        public HisAccountBookManager()
            : base()
        {

        }

        public HisAccountBookManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_ACCOUNT_BOOK> Get(HisAccountBookFilterQuery filter)
        {
            List<HIS_ACCOUNT_BOOK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACCOUNT_BOOK> resultData = null;
                if (valid)
                {
                    resultData = new HisAccountBookGet(param).Get(filter);
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

        
        public HIS_ACCOUNT_BOOK GetById(long data)
        {
            HIS_ACCOUNT_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCOUNT_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisAccountBookGet(param).GetById(data);
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

        
        public HIS_ACCOUNT_BOOK GetById(long data, HisAccountBookFilterQuery filter)
        {
            HIS_ACCOUNT_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCOUNT_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisAccountBookGet(param).GetById(data, filter);
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

        
        public HIS_ACCOUNT_BOOK GetByCode(string data)
        {
            HIS_ACCOUNT_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCOUNT_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisAccountBookGet(param).GetByCode(data);
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

        
        public HIS_ACCOUNT_BOOK GetByCode(string data, HisAccountBookFilterQuery filter)
        {
            HIS_ACCOUNT_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCOUNT_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisAccountBookGet(param).GetByCode(data, filter);
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

        
        public List<HIS_ACCOUNT_BOOK> GetByCodes(List<string> data)
        {
            List<HIS_ACCOUNT_BOOK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_ACCOUNT_BOOK> resultData = null;
                if (valid)
                {
                    resultData = new HisAccountBookGet(param).GetByCodes(data);
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
