using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRoomTypeModule
{
    partial class HisRoomTypeModuleGet : BusinessBase
    {
        internal List<V_HIS_ROOM_TYPE_MODULE> GetView(HisRoomTypeModuleViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRoomTypeModuleDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
