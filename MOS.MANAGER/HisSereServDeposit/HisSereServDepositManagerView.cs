using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServDeposit
{
    public partial class HisSereServDepositManager : BusinessBase
    {
        
        public List<V_HIS_SERE_SERV_DEPOSIT> GetView(HisSereServDepositViewFilterQuery filter)
        {
            List<V_HIS_SERE_SERV_DEPOSIT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERE_SERV_DEPOSIT> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServDepositGet(param).GetView(filter);
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

        
        public V_HIS_SERE_SERV_DEPOSIT GetViewById(long data)
        {
            V_HIS_SERE_SERV_DEPOSIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERE_SERV_DEPOSIT resultData = null;
                if (valid)
                {
                    resultData = new HisSereServDepositGet(param).GetViewById(data);
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

        
        public V_HIS_SERE_SERV_DEPOSIT GetViewById(long data, HisSereServDepositViewFilterQuery filter)
        {
            V_HIS_SERE_SERV_DEPOSIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERE_SERV_DEPOSIT resultData = null;
                if (valid)
                {
                    resultData = new HisSereServDepositGet(param).GetViewById(data, filter);
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
