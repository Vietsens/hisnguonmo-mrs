using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestTypeUser
{
    public partial class HisImpMestTypeUserManager : BusinessBase
    {
        public HisImpMestTypeUserManager()
            : base()
        {

        }

        public HisImpMestTypeUserManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_IMP_MEST_TYPE_USER> Get(HisImpMestTypeUserFilterQuery filter)
        {
             List<HIS_IMP_MEST_TYPE_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_IMP_MEST_TYPE_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestTypeUserGet(param).Get(filter);
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

        
        public  HIS_IMP_MEST_TYPE_USER GetById(long data)
        {
             HIS_IMP_MEST_TYPE_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_IMP_MEST_TYPE_USER resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestTypeUserGet(param).GetById(data);
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

        
        public  HIS_IMP_MEST_TYPE_USER GetById(long data, HisImpMestTypeUserFilterQuery filter)
        {
             HIS_IMP_MEST_TYPE_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_IMP_MEST_TYPE_USER resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestTypeUserGet(param).GetById(data, filter);
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
