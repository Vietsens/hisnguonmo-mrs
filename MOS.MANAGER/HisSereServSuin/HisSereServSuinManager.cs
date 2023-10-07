using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServSuin
{
    public partial class HisSereServSuinManager : BusinessBase
    {
        public HisSereServSuinManager()
            : base()
        {

        }

        public HisSereServSuinManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_SERE_SERV_SUIN> Get(HisSereServSuinFilterQuery filter)
        {
            List<HIS_SERE_SERV_SUIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_SUIN> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServSuinGet(param).Get(filter);
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

        
        public HIS_SERE_SERV_SUIN GetById(long data)
        {
            HIS_SERE_SERV_SUIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERE_SERV_SUIN resultData = null;
                if (valid)
                {
                    resultData = new HisSereServSuinGet(param).GetById(data);
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

        
        public HIS_SERE_SERV_SUIN GetById(long data, HisSereServSuinFilterQuery filter)
        {
            HIS_SERE_SERV_SUIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERE_SERV_SUIN resultData = null;
                if (valid)
                {
                    resultData = new HisSereServSuinGet(param).GetById(data, filter);
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

        
        public List<HIS_SERE_SERV_SUIN> GetBySereServIds(List<long> filter)
        {
            List<HIS_SERE_SERV_SUIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_SUIN> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SERE_SERV_SUIN>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServSuinGet(param).GetBySereServIds(Ids));
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

        
        public List<HIS_SERE_SERV_SUIN> GetBySuimIndexId(long filter)
        {
            List<HIS_SERE_SERV_SUIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_SUIN> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServSuinGet(param).GetBySuimIndexId(filter);
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
