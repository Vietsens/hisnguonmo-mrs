using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestInveUser
{
    public partial class HisMestInveUserManager : BusinessBase
    {
        
        public List<V_HIS_MEST_INVE_USER> GetView(HisMestInveUserViewFilterQuery filter)
        {
            List<V_HIS_MEST_INVE_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEST_INVE_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisMestInveUserGet(param).GetView(filter);
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

        
        public V_HIS_MEST_INVE_USER GetViewById(long data)
        {
            V_HIS_MEST_INVE_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEST_INVE_USER resultData = null;
                if (valid)
                {
                    resultData = new HisMestInveUserGet(param).GetViewById(data);
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

        
        public V_HIS_MEST_INVE_USER GetViewById(long data, HisMestInveUserViewFilterQuery filter)
        {
            V_HIS_MEST_INVE_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEST_INVE_USER resultData = null;
                if (valid)
                {
                    resultData = new HisMestInveUserGet(param).GetViewById(data, filter);
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
