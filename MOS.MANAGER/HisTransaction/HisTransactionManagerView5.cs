using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTransaction
{
    public partial class HisTransactionManager : BusinessBase
    {
        public List<V_HIS_TRANSACTION_5> GetView5(HisTransactionView5FilterQuery filter)
        {
            List<V_HIS_TRANSACTION_5> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisTransactionGet(param).GetView5(filter);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }

            return result;
        }

        public V_HIS_TRANSACTION_5 GetView5ByCode(string data)
        {
            V_HIS_TRANSACTION_5 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                if (valid)
                {
                    result = new HisTransactionGet(param).GetView5ByCode(data);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public V_HIS_TRANSACTION_5 GetView5ByCode(string data, HisTransactionView5FilterQuery filter)
        {
            V_HIS_TRANSACTION_5 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisTransactionGet(param).GetView5ByCode(data, filter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public V_HIS_TRANSACTION_5 GetView5ById(long data)
        {
            V_HIS_TRANSACTION_5 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                if (valid)
                {
                    result = new HisTransactionGet(param).GetView5ById(data);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public V_HIS_TRANSACTION_5 GetView5ById(long data, HisTransactionView5FilterQuery filter)
        {
            V_HIS_TRANSACTION_5 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisTransactionGet(param).GetView5ById(data, filter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<V_HIS_TRANSACTION_5> GetView5ByIds(List<long> ids)
        {
            List<V_HIS_TRANSACTION_5> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(ids);
                if (valid)
                {
                    result = new HisTransactionGet(param).GetView5ByIds(ids);
                }
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
