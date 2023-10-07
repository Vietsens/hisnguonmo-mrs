using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServDebt
{
    public partial class HisSereServDebtManager : BusinessBase
    {
        
        public List<V_HIS_SERE_SERV_DEBT> GetView(HisSereServDebtViewFilterQuery filter)
        {
            List<V_HIS_SERE_SERV_DEBT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERE_SERV_DEBT> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServDebtGet(param).GetView(filter);
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

        
        public V_HIS_SERE_SERV_DEBT GetViewById(long data)
        {
            V_HIS_SERE_SERV_DEBT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERE_SERV_DEBT resultData = null;
                if (valid)
                {
                    resultData = new HisSereServDebtGet(param).GetViewById(data);
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

        
        public V_HIS_SERE_SERV_DEBT GetViewById(long data, HisSereServDebtViewFilterQuery filter)
        {
            V_HIS_SERE_SERV_DEBT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERE_SERV_DEBT resultData = null;
                if (valid)
                {
                    resultData = new HisSereServDebtGet(param).GetViewById(data, filter);
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
