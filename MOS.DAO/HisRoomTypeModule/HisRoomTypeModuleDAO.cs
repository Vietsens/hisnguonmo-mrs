using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRoomTypeModule
{
    public partial class HisRoomTypeModuleDAO : EntityBase
    {
        private HisRoomTypeModuleGet GetWorker
        {
            get
            {
                return (HisRoomTypeModuleGet)Worker.Get<HisRoomTypeModuleGet>();
            }
        }
        public List<HIS_ROOM_TYPE_MODULE> Get(HisRoomTypeModuleSO search, CommonParam param)
        {
            List<HIS_ROOM_TYPE_MODULE> result = new List<HIS_ROOM_TYPE_MODULE>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_ROOM_TYPE_MODULE GetById(long id, HisRoomTypeModuleSO search)
        {
            HIS_ROOM_TYPE_MODULE result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
