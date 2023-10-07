using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSampleRoom
{
    public partial class HisSampleRoomManager : BusinessBase
    {
        public HisSampleRoomManager()
            : base()
        {

        }

        public HisSampleRoomManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_SAMPLE_ROOM> Get(HisSampleRoomFilterQuery filter)
        {
            List<HIS_SAMPLE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SAMPLE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisSampleRoomGet(param).Get(filter);
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

        
        public HIS_SAMPLE_ROOM GetById(long data)
        {
            HIS_SAMPLE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SAMPLE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisSampleRoomGet(param).GetById(data);
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

        
        public HIS_SAMPLE_ROOM GetById(long data, HisSampleRoomFilterQuery filter)
        {
            HIS_SAMPLE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SAMPLE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisSampleRoomGet(param).GetById(data, filter);
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

        
        public HIS_SAMPLE_ROOM GetByCode(string data)
        {
            HIS_SAMPLE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SAMPLE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisSampleRoomGet(param).GetByCode(data);
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

        
        public HIS_SAMPLE_ROOM GetByCode(string data, HisSampleRoomFilterQuery filter)
        {
            HIS_SAMPLE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SAMPLE_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisSampleRoomGet(param).GetByCode(data, filter);
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

        
        public List<HIS_SAMPLE_ROOM> GetByRoomIds(List<long> roomIds)
        {
            List<HIS_SAMPLE_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(roomIds);
                List<HIS_SAMPLE_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SAMPLE_ROOM>();
                    var skip = 0;
                    while (roomIds.Count - skip > 0)
                    {
                        var Ids = roomIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSampleRoomGet(param).GetByRoomIds(Ids));
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
    }
}
