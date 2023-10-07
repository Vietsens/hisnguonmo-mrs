using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceRoom
{
    public partial class HisServiceRoomDAO : EntityBase
    {
        private HisServiceRoomGet GetWorker
        {
            get
            {
                return (HisServiceRoomGet)Worker.Get<HisServiceRoomGet>();
            }
        }
        public List<HIS_SERVICE_ROOM> Get(HisServiceRoomSO search, CommonParam param)
        {
            List<HIS_SERVICE_ROOM> result = new List<HIS_SERVICE_ROOM>();
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

        public HIS_SERVICE_ROOM GetById(long id, HisServiceRoomSO search)
        {
            HIS_SERVICE_ROOM result = null;
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
