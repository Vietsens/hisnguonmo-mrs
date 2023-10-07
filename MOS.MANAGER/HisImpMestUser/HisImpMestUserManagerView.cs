using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestUser
{
    public partial class HisImpMestUserManager : BusinessBase
    {
        
        public List<V_HIS_IMP_MEST_USER> GetView(HisImpMestUserViewFilterQuery filter)
        {
            List<V_HIS_IMP_MEST_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_IMP_MEST_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestUserGet(param).GetView(filter);
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

        
        public V_HIS_IMP_MEST_USER GetViewById(long data)
        {
            V_HIS_IMP_MEST_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST_USER resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestUserGet(param).GetViewById(data);
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

        
        public V_HIS_IMP_MEST_USER GetViewById(long data, HisImpMestUserViewFilterQuery filter)
        {
            V_HIS_IMP_MEST_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST_USER resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestUserGet(param).GetViewById(data, filter);
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
