using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServReha
{
    public partial class HisSereServRehaManager : BusinessBase
    {
        
        public List<V_HIS_SERE_SERV_REHA> GetView(HisSereServRehaViewFilterQuery filter)
        {
            List<V_HIS_SERE_SERV_REHA> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERE_SERV_REHA> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServRehaGet(param).GetView(filter);
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

        
        public List<V_HIS_SERE_SERV_REHA> GetViewByServiceReqIds(List<long> filter)
        {
            List<V_HIS_SERE_SERV_REHA> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERE_SERV_REHA> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_SERE_SERV_REHA>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServRehaGet(param).GetViewByServiceReqIds(Ids));
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
