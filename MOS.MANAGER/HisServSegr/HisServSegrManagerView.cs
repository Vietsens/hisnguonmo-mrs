using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServSegr
{
    public partial class HisServSegrManager : BusinessBase
    {
        
        public List<V_HIS_SERV_SEGR> GetView(HisServSegrViewFilterQuery filter)
        {
            List<V_HIS_SERV_SEGR> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERV_SEGR> resultData = null;
                if (valid)
                {
                    resultData = new HisServSegrGet(param).GetView(filter);
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

        
        public V_HIS_SERV_SEGR GetViewById(long data)
        {
            V_HIS_SERV_SEGR result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERV_SEGR resultData = null;
                if (valid)
                {
                    resultData = new HisServSegrGet(param).GetViewById(data);
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

        
        public V_HIS_SERV_SEGR GetViewById(long data, HisServSegrViewFilterQuery filter)
        {
            V_HIS_SERV_SEGR result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERV_SEGR resultData = null;
                if (valid)
                {
                    resultData = new HisServSegrGet(param).GetViewById(data, filter);
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
