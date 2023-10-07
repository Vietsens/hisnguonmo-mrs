using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDataStore
{
    public partial class HisDataStoreManager : BusinessBase
    {
        public HisDataStoreManager()
            : base()
        {

        }

        public HisDataStoreManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_DATA_STORE> Get(HisDataStoreFilterQuery filter)
        {
             List<HIS_DATA_STORE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DATA_STORE> resultData = null;
                if (valid)
                {
                    resultData = new HisDataStoreGet(param).Get(filter);
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

        
        public  HIS_DATA_STORE GetById(long data)
        {
             HIS_DATA_STORE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DATA_STORE resultData = null;
                if (valid)
                {
                    resultData = new HisDataStoreGet(param).GetById(data);
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

        
        public  HIS_DATA_STORE GetById(long data, HisDataStoreFilterQuery filter)
        {
             HIS_DATA_STORE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DATA_STORE resultData = null;
                if (valid)
                {
                    resultData = new HisDataStoreGet(param).GetById(data, filter);
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

        
        public  HIS_DATA_STORE GetByCode(string data)
        {
             HIS_DATA_STORE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DATA_STORE resultData = null;
                if (valid)
                {
                    resultData = new HisDataStoreGet(param).GetByCode(data);
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

        
        public  HIS_DATA_STORE GetByCode(string data, HisDataStoreFilterQuery filter)
        {
             HIS_DATA_STORE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DATA_STORE resultData = null;
                if (valid)
                {
                    resultData = new HisDataStoreGet(param).GetByCode(data, filter);
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
