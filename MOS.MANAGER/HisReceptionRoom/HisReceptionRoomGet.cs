using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisReceptionRoom
{
    class HisReceptionRoomGet : GetBase
    {
        internal HisReceptionRoomGet()
            : base()
        {

        }

        internal HisReceptionRoomGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_RECEPTION_ROOM> Get(HisReceptionRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisReceptionRoomDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_RECEPTION_ROOM> GetView(HisReceptionRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisReceptionRoomDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_RECEPTION_ROOM GetById(long id)
        {
            try
            {
                return GetById(id, new HisReceptionRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_RECEPTION_ROOM GetById(long id, HisReceptionRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisReceptionRoomDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_RECEPTION_ROOM GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisReceptionRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_RECEPTION_ROOM GetViewById(long id, HisReceptionRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisReceptionRoomDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_RECEPTION_ROOM GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisReceptionRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_RECEPTION_ROOM GetByCode(string code, HisReceptionRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisReceptionRoomDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_RECEPTION_ROOM GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisReceptionRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_RECEPTION_ROOM GetViewByCode(string code, HisReceptionRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisReceptionRoomDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_RECEPTION_ROOM> GetByRoomIds(List<long> roomIds)
        {
            if (IsNotNullOrEmpty(roomIds))
            {
                HisReceptionRoomFilterQuery filter = new HisReceptionRoomFilterQuery();
                filter.ROOM_IDs = roomIds;
                return this.Get(filter);
            }
            return null;
        }

        internal List<V_HIS_RECEPTION_ROOM> GetViewActive()
        {
            HisReceptionRoomViewFilterQuery filter = new HisReceptionRoomViewFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            return this.GetView(filter);
        }
    }
}
