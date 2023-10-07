using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRoom
{
    public partial class HisRoomManager : BusinessBase
    {
        public HisRoomManager()
            : base()
        {

        }

        public HisRoomManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_ROOM> Get(HisRoomFilterQuery filter)
        {
            List<HIS_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisRoomGet(param).Get(filter);
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

        
        public List<V_HIS_ROOM> GetView(HisRoomViewFilterQuery filter)
        {
            List<V_HIS_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisRoomGet(param).GetView(filter);
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

        
        public List<V_HIS_ROOM_COUNTER> GetCounterView(HisRoomCounterViewFilterQuery filter)
        {
            List<V_HIS_ROOM_COUNTER> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_ROOM_COUNTER> resultData = null;
                if (valid)
                {
                    resultData = new HisRoomGet(param).GetCounterView(filter);
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

        
        public List<V_HIS_ROOM_COUNTER_1> GetCounter1View(HisRoomCounter1ViewFilterQuery filter)
        {
            List<V_HIS_ROOM_COUNTER_1> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_ROOM_COUNTER_1> resultData = null;
                if (valid)
                {
                    resultData = new HisRoomGet(param).GetCounter1View(filter);
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

        
        public List<V_HIS_ROOM> GetViewByIds(List<long> ids)
        {
            List<V_HIS_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(ids);
                List<V_HIS_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_ROOM>();
                    var skip = 0;
                    while (ids.Count - skip > 0)
                    {
                        var Ids = ids.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisRoomGet(param).GetViewByIds(Ids));
                    }
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

        
        public List<V_HIS_ROOM_COUNTER_1> GetCounter1ViewByIds(List<long> ids)
        {
            List<V_HIS_ROOM_COUNTER_1> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(ids);
                List<V_HIS_ROOM_COUNTER_1> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_ROOM_COUNTER_1>();
                    var skip = 0;
                    while (ids.Count - skip > 0)
                    {
                        var Ids = ids.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisRoomGet(param).GetCounter1ViewByIds(Ids));
                    }
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
    }
}
