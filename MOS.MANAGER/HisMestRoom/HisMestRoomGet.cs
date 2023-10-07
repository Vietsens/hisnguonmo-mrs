using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestRoom
{
    class HisMestRoomGet : GetBase
    {
        internal HisMestRoomGet()
            : base()
        {

        }

        internal HisMestRoomGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_ROOM> Get(HisMestRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestRoomDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEST_ROOM> GetView(HisMestRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestRoomDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_ROOM GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_ROOM GetById(long id, HisMestRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestRoomDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_MEST_ROOM GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMestRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEST_ROOM GetViewById(long id, HisMestRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestRoomDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_ROOM> GetByRoomId(long roomId)
        {
            try
            {
                HisMestRoomFilterQuery filter = new HisMestRoomFilterQuery();
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

        internal List<HIS_MEST_ROOM> GetByMediStockId(long mediStockId)
        {
            try
            {
                HisMestRoomFilterQuery filter = new HisMestRoomFilterQuery();
                filter.MEDI_STOCK_ID = mediStockId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_ROOM> GetActive()
        {
            try
            {
                HisMestRoomFilterQuery filter = new HisMestRoomFilterQuery();
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
    }
}
