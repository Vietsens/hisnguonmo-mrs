using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBaby
{
    public partial class HisBabyManager : BusinessBase
    {
        public List<V_HIS_BABY> GetView(HisBabyViewFilterQuery filter)
        {
            List<V_HIS_BABY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_BABY> resultData = null;
                if (valid)
                {
                    resultData = new HisBabyGet(param).GetView(filter);
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

        public V_HIS_BABY GetViewById(long id)
        {
            V_HIS_BABY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(id);
                V_HIS_BABY resultData = null;
                if (valid)
                {
                    resultData = new HisBabyGet(param).GetViewById(id);
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

        public List<V_HIS_BABY> GetViewByIds(List<long> ids)
        {
            List<V_HIS_BABY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(ids);
                List<V_HIS_BABY> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_BABY>();
                    var skip = 0;
                    while (ids.Count - skip > 0)
                    {
                        var listId = ids.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisBabyGet(param).GetViewByIds(listId));
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
