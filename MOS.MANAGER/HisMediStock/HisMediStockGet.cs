using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStock
{
    partial class HisMediStockGet : GetBase
    {
        internal HisMediStockGet()
            : base()
        {

        }

        internal HisMediStockGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDI_STOCK> Get(HisMediStockFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEDI_STOCK> GetView(HisMediStockViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_STOCK GetById(long id)
        {
            try
            {
                return GetById(id, new HisMediStockFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_STOCK GetById(long id, HisMediStockFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDI_STOCK GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMediStockViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_STOCK GetByRoomId(long roomId)
        {
            try
            {
                HisMediStockFilterQuery filter = new HisMediStockFilterQuery();
                filter.ROOM_ID = roomId;
                List<HIS_MEDI_STOCK> hisMediStockDTOs = Get(filter);
                return hisMediStockDTOs != null ? hisMediStockDTOs[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDI_STOCK> GetByRoomIds(List<long> roomIds)
        {
            if (IsNotNullOrEmpty(roomIds))
            {
                HisMediStockFilterQuery filter = new HisMediStockFilterQuery();
                filter.ROOM_IDs = roomIds;
                return Get(filter);
            }
            return null;
        }

        internal V_HIS_MEDI_STOCK GetViewById(long id, HisMediStockViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_STOCK GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisMediStockFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_STOCK GetByCode(string code, HisMediStockFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDI_STOCK GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisMediStockViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDI_STOCK GetViewByCode(string code, HisMediStockViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDI_STOCK> GetByParentId(long id)
        {
            HisMediStockFilterQuery filter = new HisMediStockFilterQuery();
            filter.PARENT_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_MEDI_STOCK> GetActive()
        {
            HisMediStockFilterQuery filter = new HisMediStockFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            return this.Get(filter);
        }

        internal List<V_HIS_MEDI_STOCK> GetViewActive()
        {
            HisMediStockViewFilterQuery filter = new HisMediStockViewFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            return this.GetView(filter);
        }
    }
}
