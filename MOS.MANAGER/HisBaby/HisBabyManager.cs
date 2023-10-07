using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBaby
{
    public partial class HisBabyManager : BusinessBase
    {
        public HisBabyManager()
            : base()
        {

        }
        
        public HisBabyManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_BABY> Get(HisBabyFilterQuery filter)
        {
            List<HIS_BABY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BABY> resultData = null;
                if (valid)
                {
                    resultData = new HisBabyGet(param).Get(filter);
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

        public HIS_BABY GetById(long id)
        {
            HIS_BABY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(id);
                HIS_BABY resultData = null;
                if (valid)
                {
                    resultData = new HisBabyGet(param).GetById(id);
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

        public List<HIS_BABY> GetByIds(List<long> ids)
        {
            List<HIS_BABY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(ids);
                List<HIS_BABY> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_BABY>();
                    var skip = 0;
                    while (ids.Count - skip > 0)
                    {
                        var listId = ids.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisBabyGet(param).GetByIds(listId));
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
