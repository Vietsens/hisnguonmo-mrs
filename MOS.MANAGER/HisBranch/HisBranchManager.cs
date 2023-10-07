using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBranch
{
    public partial class HisBranchManager : BusinessBase
    {
        public HisBranchManager()
            : base()
        {

        }

        public HisBranchManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_BRANCH> Get(HisBranchFilterQuery filter)
        {
            List<HIS_BRANCH> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BRANCH> resultData = null;
                if (valid)
                {
                    resultData = new HisBranchGet(param).Get(filter);
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

        
        public HIS_BRANCH GetById(long data)
        {
            HIS_BRANCH result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BRANCH resultData = null;
                if (valid)
                {
                    resultData = new HisBranchGet(param).GetById(data);
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

        
        public HIS_BRANCH GetById(long data, HisBranchFilterQuery filter)
        {
            HIS_BRANCH result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BRANCH resultData = null;
                if (valid)
                {
                    resultData = new HisBranchGet(param).GetById(data, filter);
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

        
        public HIS_BRANCH GetByCode(string data)
        {
            HIS_BRANCH result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BRANCH resultData = null;
                if (valid)
                {
                    resultData = new HisBranchGet(param).GetByCode(data);
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

        
        public HIS_BRANCH GetByCode(string data, HisBranchFilterQuery filter)
        {
            HIS_BRANCH result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BRANCH resultData = null;
                if (valid)
                {
                    resultData = new HisBranchGet(param).GetByCode(data, filter);
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
