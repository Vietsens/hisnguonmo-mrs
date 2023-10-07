using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisReceptionRoom
{
    public partial class HisReceptionRoomManager : BusinessBase
    {
        public HisReceptionRoomManager()
            : base()
        {

        }

        public HisReceptionRoomManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_RECEPTION_ROOM> Get(HisReceptionRoomFilterQuery filter)
        {
            List<HIS_RECEPTION_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_RECEPTION_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisReceptionRoomGet(param).Get(filter);
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

        
        public HIS_RECEPTION_ROOM GetById(long data)
        {
            HIS_RECEPTION_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_RECEPTION_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisReceptionRoomGet(param).GetById(data);
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

        
        public HIS_RECEPTION_ROOM GetById(long data, HisReceptionRoomFilterQuery filter)
        {
            HIS_RECEPTION_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_RECEPTION_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisReceptionRoomGet(param).GetById(data, filter);
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

        
        public HIS_RECEPTION_ROOM GetByCode(string data)
        {
            HIS_RECEPTION_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_RECEPTION_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisReceptionRoomGet(param).GetByCode(data);
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

        
        public HIS_RECEPTION_ROOM GetByCode(string data, HisReceptionRoomFilterQuery filter)
        {
            HIS_RECEPTION_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_RECEPTION_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisReceptionRoomGet(param).GetByCode(data, filter);
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

        
        public List<HIS_RECEPTION_ROOM> GetByRoomIds(List<long> data)
        {
            List<HIS_RECEPTION_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_RECEPTION_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_RECEPTION_ROOM>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisReceptionRoomGet(param).GetByRoomIds(Ids));
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
