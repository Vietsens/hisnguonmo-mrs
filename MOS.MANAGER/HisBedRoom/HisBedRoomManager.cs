using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedRoom
{
    public partial class HisBedRoomManager : BusinessBase
    {
        public HisBedRoomManager()
            : base()
        {

        }

        public HisBedRoomManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_BED_ROOM> Get(HisBedRoomFilterQuery filter)
        {
            List<HIS_BED_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BED_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisBedRoomGet(param).Get(filter);
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

        
        public HIS_BED_ROOM GetById(long data)
        {
            HIS_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BED_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisBedRoomGet(param).GetById(data);
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

        
        public HIS_BED_ROOM GetById(long data, HisBedRoomFilterQuery filter)
        {
            HIS_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BED_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisBedRoomGet(param).GetById(data, filter);
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

        
        public HIS_BED_ROOM GetByCode(string data)
        {
            HIS_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BED_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisBedRoomGet(param).GetByCode(data);
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

        
        public HIS_BED_ROOM GetByCode(string data, HisBedRoomFilterQuery filter)
        {
            HIS_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BED_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisBedRoomGet(param).GetByCode(data, filter);
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
