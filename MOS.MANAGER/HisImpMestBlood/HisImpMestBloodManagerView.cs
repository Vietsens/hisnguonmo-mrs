using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestBlood
{
    public partial class HisImpMestBloodManager : BusinessBase
    {
        
        public List<V_HIS_IMP_MEST_BLOOD> GetView(HisImpMestBloodViewFilterQuery filter)
        {
            List<V_HIS_IMP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_IMP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestBloodGet(param).GetView(filter);
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

        
        public V_HIS_IMP_MEST_BLOOD GetViewById(long data)
        {
            V_HIS_IMP_MEST_BLOOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST_BLOOD resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestBloodGet(param).GetViewById(data);
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

        
        public V_HIS_IMP_MEST_BLOOD GetViewById(long data, HisImpMestBloodViewFilterQuery filter)
        {
            V_HIS_IMP_MEST_BLOOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST_BLOOD resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestBloodGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_IMP_MEST_BLOOD> GetViewByImpMestId(long data)
        {
            List<V_HIS_IMP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestBloodGet(param).GetViewByImpMestId(data);
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
