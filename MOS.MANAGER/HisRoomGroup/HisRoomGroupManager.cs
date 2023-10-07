using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRoomGroup
{
    public partial class HisRoomGroupManager : BusinessBase
    {
        public HisRoomGroupManager()
            : base()
        {

        }

        public HisRoomGroupManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_ROOM_GROUP> Get(HisRoomGroupFilterQuery filter)
        {
            List<HIS_ROOM_GROUP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisRoomGroupGet(param).Get(filter);
                }
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
