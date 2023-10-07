using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRoom
{
    public partial class HisExecuteRoomManager : BusinessBase
    {
        public List<V_HIS_EXECUTE_ROOM_1> GetView1(HisExecuteRoomView1FilterQuery filter)
        {
            List<V_HIS_EXECUTE_ROOM_1> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXECUTE_ROOM_1> resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetView1(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }

            return result;
        }

        public V_HIS_EXECUTE_ROOM_1 GetView1ByCode(string data)
        {
            V_HIS_EXECUTE_ROOM_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXECUTE_ROOM_1 resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetView1ByCode(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public V_HIS_EXECUTE_ROOM_1 GetView1ByCode(string data, HisExecuteRoomView1FilterQuery filter)
        {
            V_HIS_EXECUTE_ROOM_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXECUTE_ROOM_1 resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetView1ByCode(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public V_HIS_EXECUTE_ROOM_1 GetView1ById(long data)
        {
            V_HIS_EXECUTE_ROOM_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXECUTE_ROOM_1 resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetView1ById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public V_HIS_EXECUTE_ROOM_1 GetViewById(long data, HisExecuteRoomView1FilterQuery filter)
        {
            V_HIS_EXECUTE_ROOM_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXECUTE_ROOM_1 resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetView1ById(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
