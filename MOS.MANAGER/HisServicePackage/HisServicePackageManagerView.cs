using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServicePackage
{
    public partial class HisServicePackageManager : BusinessBase
    {
        
        public List<V_HIS_SERVICE_PACKAGE> GetView(HisServicePackageViewFilterQuery filter)
        {
            List<V_HIS_SERVICE_PACKAGE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERVICE_PACKAGE> resultData = null;
                if (valid)
                {
                    resultData = new HisServicePackageGet(param).GetView(filter);
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

        
        public V_HIS_SERVICE_PACKAGE GetViewById(long data)
        {
            V_HIS_SERVICE_PACKAGE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERVICE_PACKAGE resultData = null;
                if (valid)
                {
                    resultData = new HisServicePackageGet(param).GetViewById(data);
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

        
        public V_HIS_SERVICE_PACKAGE GetViewById(long data, HisServicePackageViewFilterQuery filter)
        {
            V_HIS_SERVICE_PACKAGE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERVICE_PACKAGE resultData = null;
                if (valid)
                {
                    resultData = new HisServicePackageGet(param).GetViewById(data, filter);
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
