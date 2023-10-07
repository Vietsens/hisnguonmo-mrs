using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRoomGroup
{
    public partial class HisRoomGroupDAO : EntityBase
    {
        private HisRoomGroupGet GetWorker
        {
            get
            {
                return (HisRoomGroupGet)Worker.Get<HisRoomGroupGet>();
            }
        }

        public List<HIS_ROOM_GROUP> Get(HisRoomGroupSO search, CommonParam param)
        {
            List<HIS_ROOM_GROUP> result = new List<HIS_ROOM_GROUP>();
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

        public HIS_ROOM_GROUP GetById(long id, HisRoomGroupSO search)
        {
            HIS_ROOM_GROUP result = null;
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
