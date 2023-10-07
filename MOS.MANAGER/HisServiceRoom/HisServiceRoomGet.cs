using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceRoom
{
    partial class HisServiceRoomGet : GetBase
    {
        internal HisServiceRoomGet()
            : base()
        {

        }

        internal HisServiceRoomGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_ROOM> Get(HisServiceRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceRoomDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_ROOM> GetView(HisServiceRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceRoomDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_ROOM> GetByRoomId(long roomId)
        {
            try
            {
                HisServiceRoomFilterQuery filter = new HisServiceRoomFilterQuery();
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

        internal HIS_SERVICE_ROOM GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_ROOM GetById(long id, HisServiceRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceRoomDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_SERVICE_ROOM GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisServiceRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_ROOM GetViewById(long id, HisServiceRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceRoomDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_ROOM> GetByServiceId(long serviceId)
        {
            try
            {
                HisServiceRoomFilterQuery filter = new HisServiceRoomFilterQuery();
                filter.SERVICE_ID = serviceId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_ROOM> GetActiveView()
        {
            try
            {
                HisServiceRoomViewFilterQuery filter = new HisServiceRoomViewFilterQuery();
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
    }
}
