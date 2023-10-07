using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServReha
{
    public partial class HisSereServRehaManager : BusinessBase
    {
        public HisSereServRehaManager()
            : base()
        {

        }

        public HisSereServRehaManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_SERE_SERV_REHA> Get(HisSereServRehaFilterQuery filter)
        {
            List<HIS_SERE_SERV_REHA> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_REHA> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServRehaGet(param).Get(filter);
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

        
        public HIS_SERE_SERV_REHA GetById(long data)
        {
            HIS_SERE_SERV_REHA result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERE_SERV_REHA resultData = null;
                if (valid)
                {
                    resultData = new HisSereServRehaGet(param).GetById(data);
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

        
        public HIS_SERE_SERV_REHA GetById(long data, HisSereServRehaFilterQuery filter)
        {
            HIS_SERE_SERV_REHA result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERE_SERV_REHA resultData = null;
                if (valid)
                {
                    resultData = new HisSereServRehaGet(param).GetById(data, filter);
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

        
        public List<HIS_SERE_SERV_REHA> GetBySereServIds(List<long> filter)
        {
            List<HIS_SERE_SERV_REHA> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_REHA> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SERE_SERV_REHA>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServRehaGet(param).GetBySereServIds(Ids));
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
