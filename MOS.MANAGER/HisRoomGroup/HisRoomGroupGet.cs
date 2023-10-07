using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRoomGroup
{
    partial class HisRoomGroupGet : BusinessBase
    {
        internal HisRoomGroupGet()
            : base()
        {

        }

        internal HisRoomGroupGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ROOM_GROUP> Get(HisRoomGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRoomGroupDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ROOM_GROUP GetById(long id)
        {
            try
            {
                return GetById(id, new HisRoomGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ROOM_GROUP GetById(long id, HisRoomGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRoomGroupDAO.GetById(id, filter.Query());
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
