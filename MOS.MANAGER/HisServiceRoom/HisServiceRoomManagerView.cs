using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceRoom
{
    public partial class HisServiceRoomManager : BusinessBase
    {
        
        public List<V_HIS_SERVICE_ROOM> GetView(HisServiceRoomViewFilterQuery filter)
        {
            List<V_HIS_SERVICE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERVICE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRoomGet(param).GetView(filter);
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

        
        public V_HIS_SERVICE_ROOM GetViewById(long data)
        {
            V_HIS_SERVICE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERVICE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRoomGet(param).GetViewById(data);
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

        
        public V_HIS_SERVICE_ROOM GetViewById(long data, HisServiceRoomViewFilterQuery filter)
        {
            V_HIS_SERVICE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERVICE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRoomGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_SERVICE_ROOM> GetActiveView()
        {
            List<V_HIS_SERVICE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<V_HIS_SERVICE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRoomGet(param).GetActiveView();
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
