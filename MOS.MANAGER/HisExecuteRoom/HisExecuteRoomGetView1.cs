using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRoom
{
    partial class HisExecuteRoomGet : GetBase
    {
        internal List<V_HIS_EXECUTE_ROOM_1> GetView1(HisExecuteRoomView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoomDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

		internal V_HIS_EXECUTE_ROOM_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisExecuteRoomView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXECUTE_ROOM_1 GetView1ById(long id, HisExecuteRoomView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoomDAO.GetView1ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

		internal V_HIS_EXECUTE_ROOM_1 GetView1ByCode(string code)
        {
            try
            {
                return GetView1ByCode(code, new HisExecuteRoomView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXECUTE_ROOM_1 GetView1ByCode(string code, HisExecuteRoomView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoomDAO.GetView1ByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
