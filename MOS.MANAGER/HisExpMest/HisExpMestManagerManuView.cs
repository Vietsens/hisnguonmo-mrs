using Inventec.Common.Logging;
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
        public List<V_HIS_EXP_MEST_MANU> GetManuView(HisExpMestManuViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_MANU> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetManuView(filter);
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

        public V_HIS_EXP_MEST_MANU GetManuViewById(long data)
        {
            V_HIS_EXP_MEST_MANU result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetManuViewById(data);
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

        public V_HIS_EXP_MEST_MANU GetManuViewById(long data, HisExpMestManuViewFilterQuery filter)
        {
            V_HIS_EXP_MEST_MANU result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetManuViewById(data, filter);
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

        public V_HIS_EXP_MEST_MANU GetManuViewByCode(string data)
        {
            V_HIS_EXP_MEST_MANU result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetManuViewByCode(data);
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

        public V_HIS_EXP_MEST_MANU GetManuViewByCode(string data, HisExpMestManuViewFilterQuery filter)
        {
            V_HIS_EXP_MEST_MANU result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetManuViewByCode(data, filter);
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
