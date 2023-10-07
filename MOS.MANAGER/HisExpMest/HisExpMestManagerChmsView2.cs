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
        public List<V_HIS_EXP_MEST_CHMS_2> GetChmsView2(HisExpMestChmsView2FilterQuery filter)
        {
            List<V_HIS_EXP_MEST_CHMS_2> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsView2(filter);
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

        public V_HIS_EXP_MEST_CHMS_2 GetChmsView2ById(long data)
        {
            V_HIS_EXP_MEST_CHMS_2 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsView2ById(data);
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

        public V_HIS_EXP_MEST_CHMS_2 GetChmsView2ById(long data, HisExpMestChmsView2FilterQuery filter)
        {
            V_HIS_EXP_MEST_CHMS_2 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsView2ById(data, filter);
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

        public V_HIS_EXP_MEST_CHMS_2 GetChmsView2ByCode(string data)
        {
            V_HIS_EXP_MEST_CHMS_2 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsView2ByCode(data);
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

        public V_HIS_EXP_MEST_CHMS_2 GetChmsView2ByCode(string data, HisExpMestChmsView2FilterQuery filter)
        {
            V_HIS_EXP_MEST_CHMS_2 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsView2ByCode(data, filter);
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
