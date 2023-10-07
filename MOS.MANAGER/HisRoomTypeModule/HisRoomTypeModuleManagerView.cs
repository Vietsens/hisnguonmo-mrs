using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRoomTypeModule
{
    public partial class HisRoomTypeModuleManager : BusinessBase
    {
        
        public List<V_HIS_ROOM_TYPE_MODULE> GetView(HisRoomTypeModuleViewFilterQuery filter)
        {
            List<V_HIS_ROOM_TYPE_MODULE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_ROOM_TYPE_MODULE> resultData = null;
                if (valid)
                {
                    resultData = new HisRoomTypeModuleGet(param).GetView(filter);
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
