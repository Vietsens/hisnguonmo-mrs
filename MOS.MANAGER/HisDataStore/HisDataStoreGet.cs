using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDataStore
{
    class HisDataStoreGet : GetBase
    {
        internal HisDataStoreGet()
            : base()
        {

        }

        internal HisDataStoreGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_DATA_STORE> Get(HisDataStoreFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDataStoreDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_DATA_STORE> GetView(HisDataStoreViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDataStoreDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DATA_STORE GetById(long id)
        {
            try
            {
                return GetById(id, new HisDataStoreFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DATA_STORE GetById(long id, HisDataStoreFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDataStoreDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_DATA_STORE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisDataStoreViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DATA_STORE GetViewById(long id, HisDataStoreViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDataStoreDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DATA_STORE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisDataStoreFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DATA_STORE GetByCode(string code, HisDataStoreFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDataStoreDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_DATA_STORE GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisDataStoreViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DATA_STORE GetViewByCode(string code, HisDataStoreViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDataStoreDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_DATA_STORE> GetByParentId(long id)
        {
            HisDataStoreFilterQuery filter = new HisDataStoreFilterQuery();
            filter.PARENT_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_DATA_STORE> GetByRoomIds(List<long> roomIds)
        {
            if (IsNotNullOrEmpty(roomIds))
            {
                HisDataStoreFilterQuery filter = new HisDataStoreFilterQuery();
                filter.ROOM_IDs = roomIds;
                return this.Get(filter);
            }
            return null;
        }

        internal List<V_HIS_DATA_STORE> GetViewActive()
        {
            HisDataStoreViewFilterQuery filter = new HisDataStoreViewFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            return this.GetView(filter);
        }
    }
}
