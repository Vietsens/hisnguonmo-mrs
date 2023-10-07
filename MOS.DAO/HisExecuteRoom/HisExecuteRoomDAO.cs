using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExecuteRoom
{
    public partial class HisExecuteRoomDAO : EntityBase
    {
        private HisExecuteRoomGet GetWorker
        {
            get
            {
                return (HisExecuteRoomGet)Worker.Get<HisExecuteRoomGet>();
            }
        }
        public List<HIS_EXECUTE_ROOM> Get(HisExecuteRoomSO search, CommonParam param)
        {
            List<HIS_EXECUTE_ROOM> result = new List<HIS_EXECUTE_ROOM>();
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

        public HIS_EXECUTE_ROOM GetById(long id, HisExecuteRoomSO search)
        {
            HIS_EXECUTE_ROOM result = null;
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
