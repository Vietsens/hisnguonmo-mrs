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
        internal HisRoomTypeModuleGet()
            : base()
        {

        }

        internal HisRoomTypeModuleGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ROOM_TYPE_MODULE> Get(HisRoomTypeModuleFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRoomTypeModuleDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ROOM_TYPE_MODULE GetById(long id)
        {
            try
            {
                return GetById(id, new HisRoomTypeModuleFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ROOM_TYPE_MODULE GetById(long id, HisRoomTypeModuleFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRoomTypeModuleDAO.GetById(id, filter.Query());
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
