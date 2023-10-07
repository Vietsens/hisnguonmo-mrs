using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisUserRoom
{
    public partial class HisUserRoomDAO : EntityBase
    {
        private HisUserRoomGet GetWorker
        {
            get
            {
                return (HisUserRoomGet)Worker.Get<HisUserRoomGet>();
            }
        }
        public List<HIS_USER_ROOM> Get(HisUserRoomSO search, CommonParam param)
        {
            List<HIS_USER_ROOM> result = new List<HIS_USER_ROOM>();
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

        public HIS_USER_ROOM GetById(long id, HisUserRoomSO search)
        {
            HIS_USER_ROOM result = null;
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
