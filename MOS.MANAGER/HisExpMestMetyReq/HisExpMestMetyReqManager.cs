using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMetyReq
{
    public partial class HisExpMestMetyReqManager : BusinessBase
    {
        public HisExpMestMetyReqManager()
            : base()
        {

        }

        public HisExpMestMetyReqManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_EXP_MEST_METY_REQ> Get(HisExpMestMetyReqFilterQuery filter)
        {
            List<HIS_EXP_MEST_METY_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXP_MEST_METY_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMetyReqGet(param).Get(filter);
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

        
        public HIS_EXP_MEST_METY_REQ GetById(long data)
        {
            HIS_EXP_MEST_METY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXP_MEST_METY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMetyReqGet(param).GetById(data);
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

        
        public HIS_EXP_MEST_METY_REQ GetById(long data, HisExpMestMetyReqFilterQuery filter)
        {
            HIS_EXP_MEST_METY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXP_MEST_METY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMetyReqGet(param).GetById(data, filter);
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

        
        public List<HIS_EXP_MEST_METY_REQ> GetByExpMestId(long data)
        {
            List<HIS_EXP_MEST_METY_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_METY_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMetyReqGet(param).GetByExpMestId(data);
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

        
        public List<HIS_EXP_MEST_METY_REQ> GetByExpMestIds(List<long> data)
        {
            List<HIS_EXP_MEST_METY_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_METY_REQ> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_EXP_MEST_METY_REQ>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMetyReqGet(param).GetByExpMestIds(Ids));
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
