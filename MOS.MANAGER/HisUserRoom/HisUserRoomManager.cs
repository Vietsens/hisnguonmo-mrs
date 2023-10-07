using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisUserRoom
{
    public partial class HisUserRoomManager : BusinessBase
    {
        public HisUserRoomManager()
            : base()
        {

        }

        public HisUserRoomManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_USER_ROOM> Get(HisUserRoomFilterQuery filter)
        {
            List<HIS_USER_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_USER_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisUserRoomGet(param).Get(filter);
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

        public HIS_USER_ROOM GetById(long data)
        {
            HIS_USER_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_USER_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisUserRoomGet(param).GetById(data);
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

        public HIS_USER_ROOM GetById(long data, HisUserRoomFilterQuery filter)
        {
            HIS_USER_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_USER_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisUserRoomGet(param).GetById(data, filter);
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
