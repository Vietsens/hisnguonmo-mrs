using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServPttt
{
    public partial class HisSereServPtttManager : BusinessBase
    {
        
        public List<V_HIS_SERE_SERV_PTTT> GetView(HisSereServPtttViewFilterQuery filter)
        {
            List<V_HIS_SERE_SERV_PTTT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERE_SERV_PTTT> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServPtttGet(param).GetView(filter);
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

        
        public V_HIS_SERE_SERV_PTTT GetViewById(long data)
        {
            V_HIS_SERE_SERV_PTTT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERE_SERV_PTTT resultData = null;
                if (valid)
                {
                    resultData = new HisSereServPtttGet(param).GetViewById(data);
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

        
        public V_HIS_SERE_SERV_PTTT GetViewById(long data, HisSereServPtttViewFilterQuery filter)
        {
            V_HIS_SERE_SERV_PTTT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERE_SERV_PTTT resultData = null;
                if (valid)
                {
                    resultData = new HisSereServPtttGet(param).GetViewById(data, filter);
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
