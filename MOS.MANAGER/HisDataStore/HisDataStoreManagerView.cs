using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDataStore
{
    public partial class HisDataStoreManager : BusinessBase
    {
        
        public List<V_HIS_DATA_STORE> GetView(HisDataStoreViewFilterQuery filter)
        {
            List<V_HIS_DATA_STORE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_DATA_STORE> resultData = null;
                if (valid)
                {
                    resultData = new HisDataStoreGet(param).GetView(filter);
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

        
        public V_HIS_DATA_STORE GetViewByCode(string data)
        {
            V_HIS_DATA_STORE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_DATA_STORE resultData = null;
                if (valid)
                {
                    resultData = new HisDataStoreGet(param).GetViewByCode(data);
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

        
        public V_HIS_DATA_STORE GetViewByCode(string data, HisDataStoreViewFilterQuery filter)
        {
            V_HIS_DATA_STORE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_DATA_STORE resultData = null;
                if (valid)
                {
                    resultData = new HisDataStoreGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_DATA_STORE GetViewById(long data)
        {
            V_HIS_DATA_STORE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_DATA_STORE resultData = null;
                if (valid)
                {
                    resultData = new HisDataStoreGet(param).GetViewById(data);
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

        
        public V_HIS_DATA_STORE GetViewById(long data, HisDataStoreViewFilterQuery filter)
        {
            V_HIS_DATA_STORE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_DATA_STORE resultData = null;
                if (valid)
                {
                    resultData = new HisDataStoreGet(param).GetViewById(data, filter);
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
