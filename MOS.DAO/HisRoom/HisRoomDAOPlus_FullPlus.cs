using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRoom
{
    public partial class HisRoomDAO : EntityBase
    {
        public List<V_HIS_ROOM_COUNTER> GetCounterView(HisRoomSO search, CommonParam param)
        {
            List<V_HIS_ROOM_COUNTER> result = new List<V_HIS_ROOM_COUNTER>();

            try
            {
                result = GetWorker.GetCounterView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }


            return result;
        }

        public List<V_HIS_ROOM_COUNTER_1> GetCounter1View(HisRoomSO search, CommonParam param)
        {
            List<V_HIS_ROOM_COUNTER_1> result = new List<V_HIS_ROOM_COUNTER_1>();

            try
            {
                result = GetWorker.GetCounter1View(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
