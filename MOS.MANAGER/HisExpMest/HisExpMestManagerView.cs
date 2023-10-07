using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisExpMest.Common.Get;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMest
{
    public partial class HisExpMestManager : BusinessBase
    {
        
        public List<V_HIS_EXP_MEST> GetView(HisExpMestViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXP_MEST> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestGet(param).GetView(filter);
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

        
        public V_HIS_EXP_MEST GetViewByCode(string data)
        {
            V_HIS_EXP_MEST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXP_MEST resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestGet(param).GetViewByCode(data);
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

        
        public V_HIS_EXP_MEST GetViewByCode(string data, HisExpMestViewFilterQuery filter)
        {
            V_HIS_EXP_MEST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXP_MEST resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_EXP_MEST GetViewById(long data)
        {
            V_HIS_EXP_MEST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXP_MEST resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestGet(param).GetViewById(data);
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

        
        public V_HIS_EXP_MEST GetViewById(long data, HisExpMestViewFilterQuery filter)
        {
            V_HIS_EXP_MEST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXP_MEST resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestGet(param).GetViewById(data, filter);
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
