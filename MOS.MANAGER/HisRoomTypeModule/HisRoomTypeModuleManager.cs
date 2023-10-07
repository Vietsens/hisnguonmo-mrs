using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRoomTypeModule
{
    public partial class HisRoomTypeModuleManager : BusinessBase
    {
        public HisRoomTypeModuleManager()
            : base()
        {

        }
        
        public HisRoomTypeModuleManager(CommonParam param)
            : base(param)
        {

        }
		
		
        public  List<HIS_ROOM_TYPE_MODULE> Get(HisRoomTypeModuleFilterQuery filter)
        {
             List<HIS_ROOM_TYPE_MODULE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ROOM_TYPE_MODULE> resultData = null;
                if (valid)
                {
                    resultData = new HisRoomTypeModuleGet(param).Get(filter);
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
