using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRoomType
{
    class HisRoomTypeGet : GetBase
    {
        internal HisRoomTypeGet()
            : base()
        {

        }

        internal HisRoomTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ROOM_TYPE> Get(HisRoomTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRoomTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ROOM_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisRoomTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ROOM_TYPE GetById(long id, HisRoomTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRoomTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ROOM_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisRoomTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ROOM_TYPE GetByCode(string code, HisRoomTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRoomTypeDAO.GetByCode(code, filter.Query());
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
