using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServPttt
{
    public partial class HisSereServPtttManager : BusinessBase
    {
        public HisSereServPtttManager()
            : base()
        {

        }

        public HisSereServPtttManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_SERE_SERV_PTTT> Get(HisSereServPtttFilterQuery filter)
        {
            List<HIS_SERE_SERV_PTTT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_PTTT> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServPtttGet(param).Get(filter);
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

        
        public HIS_SERE_SERV_PTTT GetById(long data)
        {
            HIS_SERE_SERV_PTTT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERE_SERV_PTTT resultData = null;
                if (valid)
                {
                    resultData = new HisSereServPtttGet(param).GetById(data);
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

        
        public HIS_SERE_SERV_PTTT GetById(long data, HisSereServPtttFilterQuery filter)
        {
            HIS_SERE_SERV_PTTT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERE_SERV_PTTT resultData = null;
                if (valid)
                {
                    resultData = new HisSereServPtttGet(param).GetById(data, filter);
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

        
        public List<HIS_SERE_SERV_PTTT> GetBySereServIds(List<long> filter)
        {
            List<HIS_SERE_SERV_PTTT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_PTTT> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SERE_SERV_PTTT>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServPtttGet(param).GetBySereServIds(Ids));
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
