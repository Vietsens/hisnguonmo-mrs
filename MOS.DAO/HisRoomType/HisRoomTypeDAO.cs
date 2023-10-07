using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRoomType
{
    public partial class HisRoomTypeDAO : EntityBase
    {
        private HisRoomTypeGet GetWorker
        {
            get
            {
                return (HisRoomTypeGet)Worker.Get<HisRoomTypeGet>();
            }
        }
        public List<HIS_ROOM_TYPE> Get(HisRoomTypeSO search, CommonParam param)
        {
            List<HIS_ROOM_TYPE> result = new List<HIS_ROOM_TYPE>();
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

        public HIS_ROOM_TYPE GetById(long id, HisRoomTypeSO search)
        {
            HIS_ROOM_TYPE result = null;
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
