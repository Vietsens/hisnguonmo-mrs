using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRoom
{
    partial class HisExecuteRoomGet : GetBase
    {
        internal HisExecuteRoomGet()
            : base()
        {

        }

        internal HisExecuteRoomGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXECUTE_ROOM> Get(HisExecuteRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoomDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXECUTE_ROOM> GetView(HisExecuteRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoomDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_ROOM GetById(long id)
        {
            try
            {
                return GetById(id, new HisExecuteRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_ROOM GetByRoomId(long roomId)
        {
            try
            {
                HisExecuteRoomFilterQuery filter = new HisExecuteRoomFilterQuery();
                filter.ROOM_ID = roomId;
                List<HIS_EXECUTE_ROOM> list = this.Get(filter);
                return IsNotNullOrEmpty(list) ? list[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_ROOM GetByRoomId(List<HIS_EXECUTE_ROOM> executeRooms, long roomId)
        {
            try
            {
                return IsNotNullOrEmpty(executeRooms) ? executeRooms.Where(o => o.ROOM_ID == roomId).FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_ROOM GetById(long id, HisExecuteRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoomDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXECUTE_ROOM GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisExecuteRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXECUTE_ROOM GetViewById(long id, HisExecuteRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoomDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_ROOM GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisExecuteRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_ROOM GetByCode(string code, HisExecuteRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoomDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXECUTE_ROOM GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisExecuteRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXECUTE_ROOM GetViewByCode(string code, HisExecuteRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoomDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXECUTE_ROOM> GetActive()
        {
            try
            {
                HisExecuteRoomFilterQuery filter = new HisExecuteRoomFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXECUTE_ROOM> GetViewActive()
        {
            try
            {
                HisExecuteRoomViewFilterQuery filter = new HisExecuteRoomViewFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXECUTE_ROOM> GetByRoomIds(List<long> roomIds)
        {
            if (IsNotNullOrEmpty(roomIds))
            {
                HisExecuteRoomFilterQuery filter = new HisExecuteRoomFilterQuery();
                filter.ROOM_IDs = roomIds;
                return this.Get(filter);
            }
            return null;
        }
    }
}
