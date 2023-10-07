using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPackage
{
    public partial class HisPackageManager : BusinessBase
    {
        public HisPackageManager()
            : base()
        {

        }

        public HisPackageManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_PACKAGE> Get(HisPackageFilterQuery filter)
        {
            List<HIS_PACKAGE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PACKAGE> resultData = null;
                if (valid)
                {
                    resultData = new HisPackageGet(param).Get(filter);
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

        
        public HIS_PACKAGE GetById(long data)
        {
            HIS_PACKAGE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PACKAGE resultData = null;
                if (valid)
                {
                    resultData = new HisPackageGet(param).GetById(data);
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

        
        public HIS_PACKAGE GetById(long data, HisPackageFilterQuery filter)
        {
            HIS_PACKAGE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PACKAGE resultData = null;
                if (valid)
                {
                    resultData = new HisPackageGet(param).GetById(data, filter);
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

        
        public List<HIS_PACKAGE> GetActive()
        {
            List<HIS_PACKAGE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<HIS_PACKAGE> resultData = null;
                if (valid)
                {
                    resultData = new HisPackageGet(param).GetActive();
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
