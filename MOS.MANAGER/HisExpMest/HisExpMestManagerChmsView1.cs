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
        public List<V_HIS_EXP_MEST_CHMS_1> GetChmsView1(HisExpMestChmsView1FilterQuery filter)
        {
            List<V_HIS_EXP_MEST_CHMS_1> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsView1(filter);
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

        public V_HIS_EXP_MEST_CHMS_1 GetChmsView1ById(long data)
        {
            V_HIS_EXP_MEST_CHMS_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsView1ById(data);
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

        public V_HIS_EXP_MEST_CHMS_1 GetChmsView1ById(long data, HisExpMestChmsView1FilterQuery filter)
        {
            V_HIS_EXP_MEST_CHMS_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsView1ById(data, filter);
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

        public V_HIS_EXP_MEST_CHMS_1 GetChmsView1ByCode(string data)
        {
            V_HIS_EXP_MEST_CHMS_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsView1ByCode(data);
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

        public V_HIS_EXP_MEST_CHMS_1 GetChmsView1ByCode(string data, HisExpMestChmsView1FilterQuery filter)
        {
            V_HIS_EXP_MEST_CHMS_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisExpMestGet(param).GetChmsView1ByCode(data, filter);
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
