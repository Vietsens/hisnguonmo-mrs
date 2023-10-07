using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestBltyReq
{
    public partial class HisExpMestBltyReqManager : BusinessBase
    {
        public HisExpMestBltyReqManager()
            : base()
        {

        }

        public HisExpMestBltyReqManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_EXP_MEST_BLTY_REQ> Get(HisExpMestBltyReqFilterQuery filter)
        {
            List<HIS_EXP_MEST_BLTY_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXP_MEST_BLTY_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBltyReqGet(param).Get(filter);
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

        
        public HIS_EXP_MEST_BLTY_REQ GetById(long data)
        {
            HIS_EXP_MEST_BLTY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXP_MEST_BLTY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBltyReqGet(param).GetById(data);
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

        
        public HIS_EXP_MEST_BLTY_REQ GetById(long data, HisExpMestBltyReqFilterQuery filter)
        {
            HIS_EXP_MEST_BLTY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXP_MEST_BLTY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBltyReqGet(param).GetById(data, filter);
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

        
        public List<HIS_EXP_MEST_BLTY_REQ> GetByExpMestId(long data)
        {
            List<HIS_EXP_MEST_BLTY_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_BLTY_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBltyReqGet(param).GetByExpMestId(data);
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
