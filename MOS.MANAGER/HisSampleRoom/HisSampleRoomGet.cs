using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSampleRoom
{
    class HisSampleRoomGet : GetBase
    {
        internal HisSampleRoomGet()
            : base()
        {

        }

        internal HisSampleRoomGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SAMPLE_ROOM> Get(HisSampleRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSampleRoomDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SAMPLE_ROOM> GetView(HisSampleRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSampleRoomDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SAMPLE_ROOM GetById(long id)
        {
            try
            {
                return GetById(id, new HisSampleRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SAMPLE_ROOM GetByRoomId(long roomId)
        {
            try
            {
                HisSampleRoomFilterQuery filter = new HisSampleRoomFilterQuery();
                filter.ROOM_ID = roomId;
                List<HIS_SAMPLE_ROOM> list = this.Get(filter);
                return IsNotNullOrEmpty(list) ? list[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SAMPLE_ROOM GetByRoomId(List<HIS_SAMPLE_ROOM> executeRooms, long roomId)
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

        internal HIS_SAMPLE_ROOM GetById(long id, HisSampleRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSampleRoomDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SAMPLE_ROOM GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisSampleRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SAMPLE_ROOM GetViewById(long id, HisSampleRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSampleRoomDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SAMPLE_ROOM GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisSampleRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SAMPLE_ROOM GetByCode(string code, HisSampleRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSampleRoomDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SAMPLE_ROOM GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisSampleRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SAMPLE_ROOM GetViewByCode(string code, HisSampleRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSampleRoomDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SAMPLE_ROOM> GetByRoomIds(List<long> roomIds)
        {
            if (IsNotNullOrEmpty(roomIds))
            {
                HisSampleRoomFilterQuery filter = new HisSampleRoomFilterQuery();
                filter.ROOM_IDs = roomIds;
                return this.Get(filter);
            }
            return null;
        }

        internal List<V_HIS_SAMPLE_ROOM> GetViewActive()
        {
            HisSampleRoomViewFilterQuery filter = new HisSampleRoomViewFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            return this.GetView(filter);
        }
    }
}
