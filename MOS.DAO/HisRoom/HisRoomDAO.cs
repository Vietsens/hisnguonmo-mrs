using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRoom
{
    public partial class HisRoomDAO : EntityBase
    {
        private HisRoomGet GetWorker
        {
            get
            {
                return (HisRoomGet)Worker.Get<HisRoomGet>();
            }
        }
        public List<HIS_ROOM> Get(HisRoomSO search, CommonParam param)
        {
            List<HIS_ROOM> result = new List<HIS_ROOM>();
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

        public HIS_ROOM GetById(long id, HisRoomSO search)
        {
            HIS_ROOM result = null;
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
