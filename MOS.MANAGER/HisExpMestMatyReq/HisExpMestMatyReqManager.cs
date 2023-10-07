using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMatyReq
{
    public partial class HisExpMestMatyReqManager : BusinessBase
    {
        public HisExpMestMatyReqManager()
            : base()
        {

        }

        public HisExpMestMatyReqManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_EXP_MEST_MATY_REQ> Get(HisExpMestMatyReqFilterQuery filter)
        {
            List<HIS_EXP_MEST_MATY_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXP_MEST_MATY_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMatyReqGet(param).Get(filter);
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

        
        public HIS_EXP_MEST_MATY_REQ GetById(long data)
        {
            HIS_EXP_MEST_MATY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXP_MEST_MATY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMatyReqGet(param).GetById(data);
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

        
        public HIS_EXP_MEST_MATY_REQ GetById(long data, HisExpMestMatyReqFilterQuery filter)
        {
            HIS_EXP_MEST_MATY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXP_MEST_MATY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMatyReqGet(param).GetById(data, filter);
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

        
        public List<HIS_EXP_MEST_MATY_REQ> GetByExpMestId(long data)
        {
            List<HIS_EXP_MEST_MATY_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_MATY_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMatyReqGet(param).GetByExpMestId(data);
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

        
        public List<HIS_EXP_MEST_MATY_REQ> GetByExpMestIds(List<long> data)
        {
            List<HIS_EXP_MEST_MATY_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_MATY_REQ> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_EXP_MEST_MATY_REQ>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMatyReqGet(param).GetByExpMestIds(Ids));
                    }
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
