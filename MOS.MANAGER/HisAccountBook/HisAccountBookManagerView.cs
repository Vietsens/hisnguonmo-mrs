using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccountBook
{
    public partial class HisAccountBookManager : BusinessBase
    {
        
        public List<V_HIS_ACCOUNT_BOOK> GetView(HisAccountBookViewFilterQuery filter)
        {
            List<V_HIS_ACCOUNT_BOOK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_ACCOUNT_BOOK> resultData = null;
                if (valid)
                {
                    resultData = new HisAccountBookGet(param).GetView(filter);
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

        
        public V_HIS_ACCOUNT_BOOK GetViewByCode(string data)
        {
            V_HIS_ACCOUNT_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_ACCOUNT_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisAccountBookGet(param).GetViewByCode(data);
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

        
        public V_HIS_ACCOUNT_BOOK GetViewByCode(string data, HisAccountBookViewFilterQuery filter)
        {
            V_HIS_ACCOUNT_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_ACCOUNT_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisAccountBookGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_ACCOUNT_BOOK GetViewById(long data)
        {
            V_HIS_ACCOUNT_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_ACCOUNT_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisAccountBookGet(param).GetViewById(data);
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

        
        public V_HIS_ACCOUNT_BOOK GetViewById(long data, HisAccountBookViewFilterQuery filter)
        {
            V_HIS_ACCOUNT_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_ACCOUNT_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisAccountBookGet(param).GetViewById(data, filter);
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
