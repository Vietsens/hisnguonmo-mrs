using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMemaGroup
{
    public partial class HisMemaGroupManager : BusinessBase
    {
        public HisMemaGroupManager()
            : base()
        {

        }

        public HisMemaGroupManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEMA_GROUP> Get(HisMemaGroupFilterQuery filter)
        {
             List<HIS_MEMA_GROUP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEMA_GROUP> resultData = null;
                if (valid)
                {
                    resultData = new HisMemaGroupGet(param).Get(filter);
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

        
        public  HIS_MEMA_GROUP GetById(long data)
        {
             HIS_MEMA_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEMA_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisMemaGroupGet(param).GetById(data);
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

        
        public  HIS_MEMA_GROUP GetById(long data, HisMemaGroupFilterQuery filter)
        {
             HIS_MEMA_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEMA_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisMemaGroupGet(param).GetById(data, filter);
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

        
        public  HIS_MEMA_GROUP GetByCode(string data)
        {
             HIS_MEMA_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEMA_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisMemaGroupGet(param).GetByCode(data);
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

        
        public  HIS_MEMA_GROUP GetByCode(string data, HisMemaGroupFilterQuery filter)
        {
             HIS_MEMA_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEMA_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisMemaGroupGet(param).GetByCode(data, filter);
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
