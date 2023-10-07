using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTestIndex
{
    public partial class HisTestIndexManager : BusinessBase
    {
        
        public V_HIS_TEST_INDEX GetViewByCode(string data)
        {
            V_HIS_TEST_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_TEST_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexGet(param).GetViewByCode(data);
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

        
        public V_HIS_TEST_INDEX GetViewByCode(string data, HisTestIndexViewFilterQuery filter)
        {
            V_HIS_TEST_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_TEST_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_TEST_INDEX GetViewById(long data)
        {
            V_HIS_TEST_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_TEST_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexGet(param).GetViewById(data);
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

        
        public V_HIS_TEST_INDEX GetViewById(long data, HisTestIndexViewFilterQuery filter)
        {
            V_HIS_TEST_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_TEST_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexGet(param).GetViewById(data, filter);
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
