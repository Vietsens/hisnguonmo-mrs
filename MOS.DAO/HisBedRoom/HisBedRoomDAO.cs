using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBedRoom
{
    public partial class HisBedRoomDAO : EntityBase
    {
        private HisBedRoomGet GetWorker
        {
            get
            {
                return (HisBedRoomGet)Worker.Get<HisBedRoomGet>();
            }
        }
        public List<HIS_BED_ROOM> Get(HisBedRoomSO search, CommonParam param)
        {
            List<HIS_BED_ROOM> result = new List<HIS_BED_ROOM>();
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

        public HIS_BED_ROOM GetById(long id, HisBedRoomSO search)
        {
            HIS_BED_ROOM result = null;
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
