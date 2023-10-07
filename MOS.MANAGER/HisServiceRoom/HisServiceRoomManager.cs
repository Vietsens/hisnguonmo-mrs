using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceRoom
{
    public partial class HisServiceRoomManager : BusinessBase
    {
        public HisServiceRoomManager()
            : base()
        {

        }

        public HisServiceRoomManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_ROOM> Get(HisServiceRoomFilterQuery filter)
        {
             List<HIS_SERVICE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRoomGet(param).Get(filter);
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

        
        public  HIS_SERVICE_ROOM GetById(long data)
        {
             HIS_SERVICE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRoomGet(param).GetById(data);
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

        
        public  HIS_SERVICE_ROOM GetById(long data, HisServiceRoomFilterQuery filter)
        {
             HIS_SERVICE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRoomGet(param).GetById(data, filter);
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

        
        public  List<HIS_SERVICE_ROOM> GetByRoomId(long data)
        {
             List<HIS_SERVICE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERVICE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRoomGet(param).GetByRoomId(data);
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

        
        public  List<HIS_SERVICE_ROOM> GetByServiceId(long data)
        {
             List<HIS_SERVICE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERVICE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRoomGet(param).GetByServiceId(data);
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
