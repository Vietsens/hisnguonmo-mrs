using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestUser
{
    public partial class HisImpMestUserManager : BusinessBase
    {
        public HisImpMestUserManager()
            : base()
        {

        }

        public HisImpMestUserManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_IMP_MEST_USER> Get(HisImpMestUserFilterQuery filter)
        {
            List<HIS_IMP_MEST_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_IMP_MEST_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestUserGet(param).Get(filter);
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

        
        public HIS_IMP_MEST_USER GetById(long data)
        {
            HIS_IMP_MEST_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_IMP_MEST_USER resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestUserGet(param).GetById(data);
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

        
        public HIS_IMP_MEST_USER GetById(long data, HisImpMestUserFilterQuery filter)
        {
            HIS_IMP_MEST_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_IMP_MEST_USER resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestUserGet(param).GetById(data, filter);
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

        
        public List<HIS_IMP_MEST_USER> GetByImpMestId(long data)
        {
            List<HIS_IMP_MEST_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_IMP_MEST_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestUserGet(param).GetByImpMestId(data);
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
