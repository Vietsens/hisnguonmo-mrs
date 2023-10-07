using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestInveUser
{
    public partial class HisMestInveUserManager : BusinessBase
    {
        public HisMestInveUserManager()
            : base()
        {

        }

        public HisMestInveUserManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_MEST_INVE_USER> Get(HisMestInveUserFilterQuery filter)
        {
            List<HIS_MEST_INVE_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_INVE_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisMestInveUserGet(param).Get(filter);
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

        
        public HIS_MEST_INVE_USER GetById(long data)
        {
            HIS_MEST_INVE_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_INVE_USER resultData = null;
                if (valid)
                {
                    resultData = new HisMestInveUserGet(param).GetById(data);
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

        
        public HIS_MEST_INVE_USER GetById(long data, HisMestInveUserFilterQuery filter)
        {
            HIS_MEST_INVE_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_INVE_USER resultData = null;
                if (valid)
                {
                    resultData = new HisMestInveUserGet(param).GetById(data, filter);
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

        
        public List<HIS_MEST_INVE_USER> GetByMestInventoryId(long data)
        {
            List<HIS_MEST_INVE_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_INVE_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisMestInveUserGet(param).GetByMestInventoryId(data);
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
