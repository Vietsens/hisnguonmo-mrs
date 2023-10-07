using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRoom
{
    public partial class HisExecuteRoomManager : BusinessBase
    {
        public HisExecuteRoomManager()
            : base()
        {

        }

        public HisExecuteRoomManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_EXECUTE_ROOM> Get(HisExecuteRoomFilterQuery filter)
        {
            List<HIS_EXECUTE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXECUTE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).Get(filter);
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

        
        public HIS_EXECUTE_ROOM GetById(long data)
        {
            HIS_EXECUTE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXECUTE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetById(data);
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

        
        public HIS_EXECUTE_ROOM GetById(long data, HisExecuteRoomFilterQuery filter)
        {
            HIS_EXECUTE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXECUTE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetById(data, filter);
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

        
        public HIS_EXECUTE_ROOM GetByCode(string data)
        {
            HIS_EXECUTE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXECUTE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetByCode(data);
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

        
        public HIS_EXECUTE_ROOM GetByCode(string data, HisExecuteRoomFilterQuery filter)
        {
            HIS_EXECUTE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXECUTE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetByCode(data, filter);
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

        
        public List<HIS_EXECUTE_ROOM> GetActive()
        {
            List<HIS_EXECUTE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<HIS_EXECUTE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetActive();
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

        
        public List<HIS_EXECUTE_ROOM> GetByRoomIds(List<long> filter)
        {
            List<HIS_EXECUTE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXECUTE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_EXECUTE_ROOM>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExecuteRoomGet(param).GetByRoomIds(Ids));
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
