using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedRoom
{
    class HisBedRoomGet : GetBase
    {
        internal HisBedRoomGet()
            : base()
        {

        }

        internal HisBedRoomGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BED_ROOM> Get(HisBedRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedRoomDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_BED_ROOM> GetView(HisBedRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedRoomDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_BED_ROOM> GetViewByDepartmentId(long departmentId)
        {
            HisBedRoomViewFilterQuery filter = new HisBedRoomViewFilterQuery();
            filter.DEPARTMENT_ID = departmentId;
            return this.GetView(filter);
        }

        internal HIS_BED_ROOM GetById(long id)
        {
            try
            {
                return GetById(id, new HisBedRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED_ROOM GetById(long id, HisBedRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedRoomDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
		
		internal V_HIS_BED_ROOM GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisBedRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BED_ROOM GetViewById(long id, HisBedRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedRoomDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED_ROOM GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBedRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED_ROOM GetByCode(string code, HisBedRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedRoomDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
		
		internal V_HIS_BED_ROOM GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisBedRoomViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BED_ROOM GetViewByCode(string code, HisBedRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedRoomDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_BED_ROOM> GetByRoomIds(List<long> roomIds)
        {
            if (IsNotNullOrEmpty(roomIds))
            {
                HisBedRoomFilterQuery filter = new HisBedRoomFilterQuery();
                filter.ROOM_IDs = roomIds;
                return this.Get(filter);
            }
            return null;
        }

        internal List<V_HIS_BED_ROOM> GetByDepartmentId(long nextDepartmentId)
        {
            HisBedRoomViewFilterQuery filter = new HisBedRoomViewFilterQuery();
            filter.DEPARTMENT_ID = nextDepartmentId;
            return this.GetView(filter);
        }

        internal List<V_HIS_BED_ROOM> GetViewActive()
        {
            HisBedRoomViewFilterQuery filter = new HisBedRoomViewFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            return this.GetView(filter);
        }
    }
}
