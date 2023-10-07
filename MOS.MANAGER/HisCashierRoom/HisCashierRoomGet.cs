using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCashierRoom
{
    class HisCashierRoomGet : GetBase
    {
        internal HisCashierRoomGet()
            : base()
        {

        }

        internal HisCashierRoomGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CASHIER_ROOM> Get(HisCashierRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCashierRoomDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_CASHIER_ROOM> GetView(HisCashierRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCashierRoomDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CASHIER_ROOM GetById(long id)
        {
            try
            {
                return GetById(id, new HisCashierRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CASHIER_ROOM GetByRoomId(long roomId)
        {
            try
            {
                HisCashierRoomFilterQuery filter = new HisCashierRoomFilterQuery();
                filter.ROOM_ID = roomId;
                List<HIS_CASHIER_ROOM> list = this.Get(filter);
                return IsNotNullOrEmpty(list) ? list[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CASHIER_ROOM GetByRoomId(List<HIS_CASHIER_ROOM> CashierRooms, long roomId)
        {
            try
            {
                return IsNotNullOrEmpty(CashierRooms) ? CashierRooms.Where(o => o.ROOM_ID == roomId).FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CASHIER_ROOM GetById(long id, HisCashierRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCashierRoomDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_CASHIER_ROOM GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisCashierRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_CASHIER_ROOM GetViewById(long id, HisCashierRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCashierRoomDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CASHIER_ROOM GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisCashierRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CASHIER_ROOM GetByCode(string code, HisCashierRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCashierRoomDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_CASHIER_ROOM GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisCashierRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_CASHIER_ROOM GetViewByCode(string code, HisCashierRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCashierRoomDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_CASHIER_ROOM> GetActive()
        {
            try
            {
                HisCashierRoomFilterQuery filter = new HisCashierRoomFilterQuery();
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

        internal List<HIS_CASHIER_ROOM> GetByRoomIds(List<long> roomIds)
        {
            if (IsNotNullOrEmpty(roomIds))
            {
                HisCashierRoomFilterQuery filter = new HisCashierRoomFilterQuery();
                filter.ROOM_IDs = roomIds;
                return this.Get(filter);
            }
            return null;
        }

        internal List<V_HIS_CASHIER_ROOM> GetViewActive()
        {
            HisCashierRoomViewFilterQuery filter = new HisCashierRoomViewFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            return this.GetView(filter);
        }
    }
}
