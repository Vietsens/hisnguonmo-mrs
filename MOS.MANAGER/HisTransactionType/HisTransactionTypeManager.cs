using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTransactionType
{
    public partial class HisTransactionTypeManager : BusinessBase
    {
        public HisTransactionTypeManager()
            : base()
        {

        }

        public HisTransactionTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_TRANSACTION_TYPE> Get(HisTransactionTypeFilterQuery filter)
        {
             List<HIS_TRANSACTION_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRANSACTION_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionTypeGet(param).Get(filter);
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

        
        public  HIS_TRANSACTION_TYPE GetById(long data)
        {
             HIS_TRANSACTION_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TRANSACTION_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionTypeGet(param).GetById(data);
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

        
        public  HIS_TRANSACTION_TYPE GetById(long data, HisTransactionTypeFilterQuery filter)
        {
             HIS_TRANSACTION_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TRANSACTION_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionTypeGet(param).GetById(data, filter);
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

        
        public  HIS_TRANSACTION_TYPE GetByCode(string data)
        {
             HIS_TRANSACTION_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TRANSACTION_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionTypeGet(param).GetByCode(data);
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

        
        public  HIS_TRANSACTION_TYPE GetByCode(string data, HisTransactionTypeFilterQuery filter)
        {
             HIS_TRANSACTION_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TRANSACTION_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionTypeGet(param).GetByCode(data, filter);
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
