using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestRoom
{
    public partial class HisMestRoomDAO : EntityBase
    {
        public List<V_HIS_MEST_ROOM> GetView(HisMestRoomSO search, CommonParam param)
        {
            List<V_HIS_MEST_ROOM> result = new List<V_HIS_MEST_ROOM>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_MEST_ROOM GetViewById(long id, HisMestRoomSO search)
        {
            V_HIS_MEST_ROOM result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
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
