using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestRoom
{
    public partial class HisMestRoomDAO : EntityBase
    {
        private HisMestRoomGet GetWorker
        {
            get
            {
                return (HisMestRoomGet)Worker.Get<HisMestRoomGet>();
            }
        }
        public List<HIS_MEST_ROOM> Get(HisMestRoomSO search, CommonParam param)
        {
            List<HIS_MEST_ROOM> result = new List<HIS_MEST_ROOM>();
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

        public HIS_MEST_ROOM GetById(long id, HisMestRoomSO search)
        {
            HIS_MEST_ROOM result = null;
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
