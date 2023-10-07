using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisReceptionRoom
{
    public partial class HisReceptionRoomManager : BusinessBase
    {
        
        public List<V_HIS_RECEPTION_ROOM> GetView(HisReceptionRoomViewFilterQuery filter)
        {
            List<V_HIS_RECEPTION_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_RECEPTION_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisReceptionRoomGet(param).GetView(filter);
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

        
        public V_HIS_RECEPTION_ROOM GetViewByCode(string data)
        {
            V_HIS_RECEPTION_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_RECEPTION_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisReceptionRoomGet(param).GetViewByCode(data);
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

        
        public V_HIS_RECEPTION_ROOM GetViewByCode(string data, HisReceptionRoomViewFilterQuery filter)
        {
            V_HIS_RECEPTION_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_RECEPTION_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisReceptionRoomGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_RECEPTION_ROOM GetViewById(long data)
        {
            V_HIS_RECEPTION_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_RECEPTION_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisReceptionRoomGet(param).GetViewById(data);
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

        
        public V_HIS_RECEPTION_ROOM GetViewById(long data, HisReceptionRoomViewFilterQuery filter)
        {
            V_HIS_RECEPTION_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_RECEPTION_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisReceptionRoomGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_RECEPTION_ROOM> GetViewActive()
        {
            List<V_HIS_RECEPTION_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<V_HIS_RECEPTION_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisReceptionRoomGet(param).GetViewActive();
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
