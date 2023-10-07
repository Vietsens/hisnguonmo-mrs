using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestRoom
{
    public partial class HisMestRoomManager : BusinessBase
    {
        public HisMestRoomManager()
            : base()
        {

        }

        public HisMestRoomManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_MEST_ROOM> Get(HisMestRoomFilterQuery filter)
        {
            List<HIS_MEST_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisMestRoomGet(param).Get(filter);
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

        
        public HIS_MEST_ROOM GetById(long data)
        {
            HIS_MEST_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisMestRoomGet(param).GetById(data);
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

        
        public HIS_MEST_ROOM GetById(long data, HisMestRoomFilterQuery filter)
        {
            HIS_MEST_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisMestRoomGet(param).GetById(data, filter);
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

        
        public List<HIS_MEST_ROOM> GetByRoomId(long data)
        {
            List<HIS_MEST_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisMestRoomGet(param).GetByRoomId(data);
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

        
        public List<HIS_MEST_ROOM> GetByMediStockId(long data)
        {
            List<HIS_MEST_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisMestRoomGet(param).GetByMediStockId(data);
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

        
        public List<HIS_MEST_ROOM> GetActive()
        {
            List<HIS_MEST_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<HIS_MEST_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisMestRoomGet(param).GetActive();
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
