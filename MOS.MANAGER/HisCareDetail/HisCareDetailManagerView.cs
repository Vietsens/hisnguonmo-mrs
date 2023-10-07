using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCareDetail
{
    public partial class HisCareDetailManager : BusinessBase
    {
        
        public List<V_HIS_CARE_DETAIL> GetView(HisCareDetailViewFilterQuery filter)
        {
            List<V_HIS_CARE_DETAIL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_CARE_DETAIL> resultData = null;
                if (valid)
                {
                    resultData = new HisCareDetailGet(param).GetView(filter);
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

        
        public V_HIS_CARE_DETAIL GetViewById(long data)
        {
            V_HIS_CARE_DETAIL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_CARE_DETAIL resultData = null;
                if (valid)
                {
                    resultData = new HisCareDetailGet(param).GetViewById(data);
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

        
        public V_HIS_CARE_DETAIL GetViewById(long data, HisCareDetailViewFilterQuery filter)
        {
            V_HIS_CARE_DETAIL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_CARE_DETAIL resultData = null;
                if (valid)
                {
                    resultData = new HisCareDetailGet(param).GetViewById(data, filter);
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
