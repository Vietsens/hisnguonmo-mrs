using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSampleRoom
{
    public partial class HisSampleRoomManager : BusinessBase
    {
        
        public List<V_HIS_SAMPLE_ROOM> GetView(HisSampleRoomViewFilterQuery filter)
        {
            List<V_HIS_SAMPLE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SAMPLE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisSampleRoomGet(param).GetView(filter);
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

        
        public V_HIS_SAMPLE_ROOM GetViewByCode(string data)
        {
            V_HIS_SAMPLE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SAMPLE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisSampleRoomGet(param).GetViewByCode(data);
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

        
        public V_HIS_SAMPLE_ROOM GetViewByCode(string data, HisSampleRoomViewFilterQuery filter)
        {
            V_HIS_SAMPLE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SAMPLE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisSampleRoomGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_SAMPLE_ROOM GetViewById(long data)
        {
            V_HIS_SAMPLE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SAMPLE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisSampleRoomGet(param).GetViewById(data);
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

        
        public V_HIS_SAMPLE_ROOM GetViewById(long data, HisSampleRoomViewFilterQuery filter)
        {
            V_HIS_SAMPLE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SAMPLE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisSampleRoomGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_SAMPLE_ROOM> GetViewActive()
        {
            List<V_HIS_SAMPLE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<V_HIS_SAMPLE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisSampleRoomGet(param).GetViewActive();
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
