using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServicePackage
{
    public partial class HisServicePackageManager : BusinessBase
    {
        public HisServicePackageManager()
            : base()
        {

        }

        public HisServicePackageManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_PACKAGE> Get(HisServicePackageFilterQuery filter)
        {
             List<HIS_SERVICE_PACKAGE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_PACKAGE> resultData = null;
                if (valid)
                {
                    resultData = new HisServicePackageGet(param).Get(filter);
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

        
        public  HIS_SERVICE_PACKAGE GetById(long data)
        {
             HIS_SERVICE_PACKAGE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_PACKAGE resultData = null;
                if (valid)
                {
                    resultData = new HisServicePackageGet(param).GetById(data);
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

        
        public  HIS_SERVICE_PACKAGE GetById(long data, HisServicePackageFilterQuery filter)
        {
             HIS_SERVICE_PACKAGE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_PACKAGE resultData = null;
                if (valid)
                {
                    resultData = new HisServicePackageGet(param).GetById(data, filter);
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

        
        public  List<HIS_SERVICE_PACKAGE> GetByServiceId(long data)
        {
             List<HIS_SERVICE_PACKAGE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERVICE_PACKAGE> resultData = null;
                if (valid)
                {
                    resultData = new HisServicePackageGet(param).GetByServiceId(data);
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
