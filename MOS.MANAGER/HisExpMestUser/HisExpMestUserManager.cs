using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestUser
{
    public partial class HisExpMestUserManager : BusinessBase
    {
        public HisExpMestUserManager()
            : base()
        {

        }

        public HisExpMestUserManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EXP_MEST_USER> Get(HisExpMestUserFilterQuery filter)
        {
             List<HIS_EXP_MEST_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXP_MEST_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestUserGet(param).Get(filter);
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

        
        public  HIS_EXP_MEST_USER GetById(long data)
        {
             HIS_EXP_MEST_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXP_MEST_USER resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestUserGet(param).GetById(data);
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

        
        public  HIS_EXP_MEST_USER GetById(long data, HisExpMestUserFilterQuery filter)
        {
             HIS_EXP_MEST_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXP_MEST_USER resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestUserGet(param).GetById(data, filter);
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

        
        public  List<HIS_EXP_MEST_USER> GetByExpMestId(long data)
        {
             List<HIS_EXP_MEST_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestUserGet(param).GetByExpMestId(data);
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
