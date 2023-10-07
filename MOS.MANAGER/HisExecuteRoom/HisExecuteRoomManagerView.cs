using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRoom
{
    public partial class HisExecuteRoomManager : BusinessBase
    {
        
        public List<V_HIS_EXECUTE_ROOM> GetView(HisExecuteRoomViewFilterQuery filter)
        {
            List<V_HIS_EXECUTE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXECUTE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetView(filter);
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

        
        public V_HIS_EXECUTE_ROOM GetViewByCode(string data)
        {
            V_HIS_EXECUTE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXECUTE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetViewByCode(data);
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

        
        public V_HIS_EXECUTE_ROOM GetViewByCode(string data, HisExecuteRoomViewFilterQuery filter)
        {
            V_HIS_EXECUTE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXECUTE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_EXECUTE_ROOM GetViewById(long data)
        {
            V_HIS_EXECUTE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXECUTE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetViewById(data);
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

        
        public V_HIS_EXECUTE_ROOM GetViewById(long data, HisExecuteRoomViewFilterQuery filter)
        {
            V_HIS_EXECUTE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXECUTE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_EXECUTE_ROOM> GetViewActive()
        {
            List<V_HIS_EXECUTE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<V_HIS_EXECUTE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoomGet(param).GetViewActive();
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
