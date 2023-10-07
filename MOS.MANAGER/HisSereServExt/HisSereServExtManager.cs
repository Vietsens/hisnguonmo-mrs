using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServExt
{
    public partial class HisSereServExtManager : BusinessBase
    {
        public HisSereServExtManager()
            : base()
        {

        }

        public HisSereServExtManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_SERE_SERV_EXT> Get(HisSereServExtFilterQuery filter)
        {
            List<HIS_SERE_SERV_EXT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_EXT> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServExtGet(param).Get(filter);
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

        
        public HIS_SERE_SERV_EXT GetById(long data)
        {
            HIS_SERE_SERV_EXT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERE_SERV_EXT resultData = null;
                if (valid)
                {
                    resultData = new HisSereServExtGet(param).GetById(data);
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

        
        public HIS_SERE_SERV_EXT GetById(long data, HisSereServExtFilterQuery filter)
        {
            HIS_SERE_SERV_EXT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERE_SERV_EXT resultData = null;
                if (valid)
                {
                    resultData = new HisSereServExtGet(param).GetById(data, filter);
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

        
        public HIS_SERE_SERV_EXT GetBySereServId(long data)
        {
            HIS_SERE_SERV_EXT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERE_SERV_EXT resultData = null;
                if (valid)
                {
                    resultData = new HisSereServExtGet(param).GetBySereServId(data);
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

        
        public List<HIS_SERE_SERV_EXT> GetBySereServIds(List<long> data)
        {
            List<HIS_SERE_SERV_EXT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERE_SERV_EXT> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SERE_SERV_EXT>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServExtGet(param).GetBySereServIds(Ids));
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
