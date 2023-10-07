using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisReceptionRoom
{
    public partial class HisReceptionRoomDAO : EntityBase
    {
        private HisReceptionRoomGet GetWorker
        {
            get
            {
                return (HisReceptionRoomGet)Worker.Get<HisReceptionRoomGet>();
            }
        }
        public List<HIS_RECEPTION_ROOM> Get(HisReceptionRoomSO search, CommonParam param)
        {
            List<HIS_RECEPTION_ROOM> result = new List<HIS_RECEPTION_ROOM>();
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

        public HIS_RECEPTION_ROOM GetById(long id, HisReceptionRoomSO search)
        {
            HIS_RECEPTION_ROOM result = null;
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
