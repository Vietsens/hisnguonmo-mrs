using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedRoom
{
    public partial class HisBedRoomManager : BusinessBase
    {
        
        public List<V_HIS_BED_ROOM> GetView(HisBedRoomViewFilterQuery filter)
        {
            List<V_HIS_BED_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_BED_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisBedRoomGet(param).GetView(filter);
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

        
        public V_HIS_BED_ROOM GetViewByCode(string data)
        {
            V_HIS_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_BED_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisBedRoomGet(param).GetViewByCode(data);
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

        
        public V_HIS_BED_ROOM GetViewByCode(string data, HisBedRoomViewFilterQuery filter)
        {
            V_HIS_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_BED_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisBedRoomGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_BED_ROOM GetViewById(long data)
        {
            V_HIS_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_BED_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisBedRoomGet(param).GetViewById(data);
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

        
        public V_HIS_BED_ROOM GetViewById(long data, HisBedRoomViewFilterQuery filter)
        {
            V_HIS_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_BED_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisBedRoomGet(param).GetViewById(data, filter);
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
