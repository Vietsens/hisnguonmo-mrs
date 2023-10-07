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

        public List<V_HIS_EXP_MEST_CHMS> GetChmsView(HisExpMestChmsViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_CHMS> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsView(filter);
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

        public V_HIS_EXP_MEST_CHMS GetChmsViewById(long data)
        {
            V_HIS_EXP_MEST_CHMS result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsViewById(data);
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

        public V_HIS_EXP_MEST_CHMS GetChmsViewById(long data, HisExpMestChmsViewFilterQuery filter)
        {
            V_HIS_EXP_MEST_CHMS result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsViewById(data, filter);
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

        public V_HIS_EXP_MEST_CHMS GetChmsViewByCode(string data)
        {
            V_HIS_EXP_MEST_CHMS result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsViewByCode(data);
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

        public V_HIS_EXP_MEST_CHMS GetChmsViewByCode(string data, HisExpMestChmsViewFilterQuery filter)
        {
            V_HIS_EXP_MEST_CHMS result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsViewByCode(data, filter);
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
