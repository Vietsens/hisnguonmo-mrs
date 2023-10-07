using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisUserRoom
{
    class HisUserRoomGet : GetBase
    {
        internal HisUserRoomGet()
            : base()
        {

        }

        internal HisUserRoomGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_USER_ROOM> Get(HisUserRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisUserRoomDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_USER_ROOM> GetView(HisUserRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisUserRoomDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_USER_ROOM GetById(long id)
        {
            try
            {
                return GetById(id, new HisUserRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_USER_ROOM GetById(long id, HisUserRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisUserRoomDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_USER_ROOM GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisUserRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_USER_ROOM GetViewById(long id, HisUserRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisUserRoomDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_USER_ROOM> GetByRoomId(long roomId)
        {
            try
            {
                HisUserRoomFilterQuery filter = new HisUserRoomFilterQuery();
                filter.ROOM_ID = roomId;
                return this.Get(filter);
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
