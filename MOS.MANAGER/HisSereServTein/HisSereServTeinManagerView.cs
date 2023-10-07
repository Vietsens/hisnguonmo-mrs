using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServTein
{
    public partial class HisSereServTeinManager : BusinessBase
    {
        
        public List<V_HIS_SERE_SERV_TEIN> GetView(HisSereServTeinViewFilterQuery filter)
        {
            List<V_HIS_SERE_SERV_TEIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERE_SERV_TEIN> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServTeinGet(param).GetView(filter);
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

        
        public V_HIS_SERE_SERV_TEIN GetViewById(long data)
        {
            V_HIS_SERE_SERV_TEIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERE_SERV_TEIN resultData = null;
                if (valid)
                {
                    resultData = new HisSereServTeinGet(param).GetViewById(data);
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

        
        public V_HIS_SERE_SERV_TEIN GetViewById(long data, HisSereServTeinViewFilterQuery filter)
        {
            V_HIS_SERE_SERV_TEIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERE_SERV_TEIN resultData = null;
                if (valid)
                {
                    resultData = new HisSereServTeinGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_SERE_SERV_TEIN> GetViewBySereServIds(List<long> filter)
        {
            List<V_HIS_SERE_SERV_TEIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERE_SERV_TEIN> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_SERE_SERV_TEIN>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServTeinGet(param).GetViewBySereServIds(Ids));
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
