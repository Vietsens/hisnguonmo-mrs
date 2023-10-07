using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRoomType
{
    public partial class HisRoomTypeManager : BusinessBase
    {
        public HisRoomTypeManager()
            : base()
        {

        }

        public HisRoomTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_ROOM_TYPE> Get(HisRoomTypeFilterQuery filter)
        {
            List<HIS_ROOM_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ROOM_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisRoomTypeGet(param).Get(filter);
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

        
        public HIS_ROOM_TYPE GetById(long data)
        {
            HIS_ROOM_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ROOM_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisRoomTypeGet(param).GetById(data);
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

        
        public HIS_ROOM_TYPE GetById(long data, HisRoomTypeFilterQuery filter)
        {
            HIS_ROOM_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ROOM_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisRoomTypeGet(param).GetById(data, filter);
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

        
        public HIS_ROOM_TYPE GetByCode(string data)
        {
            HIS_ROOM_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ROOM_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisRoomTypeGet(param).GetByCode(data);
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

        
        public HIS_ROOM_TYPE GetByCode(string data, HisRoomTypeFilterQuery filter)
        {
            HIS_ROOM_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ROOM_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisRoomTypeGet(param).GetByCode(data, filter);
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
