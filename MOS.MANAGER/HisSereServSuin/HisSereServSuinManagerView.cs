using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServSuin
{
    public partial class HisSereServSuinManager : BusinessBase
    {
        
        public List<V_HIS_SERE_SERV_SUIN> GetView(HisSereServSuinViewFilterQuery filter)
        {
            List<V_HIS_SERE_SERV_SUIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERE_SERV_SUIN> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServSuinGet(param).GetView(filter);
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

        
        public V_HIS_SERE_SERV_SUIN GetViewById(long data)
        {
            V_HIS_SERE_SERV_SUIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERE_SERV_SUIN resultData = null;
                if (valid)
                {
                    resultData = new HisSereServSuinGet(param).GetViewById(data);
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

        
        public V_HIS_SERE_SERV_SUIN GetViewById(long data, HisSereServSuinViewFilterQuery filter)
        {
            V_HIS_SERE_SERV_SUIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERE_SERV_SUIN resultData = null;
                if (valid)
                {
                    resultData = new HisSereServSuinGet(param).GetViewById(data, filter);
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
