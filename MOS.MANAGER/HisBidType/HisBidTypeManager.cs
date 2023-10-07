using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidType
{
    public partial class HisBidTypeManager : BusinessBase
    {
        public HisBidTypeManager()
            : base()
        {

        }

        public HisBidTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_BID_TYPE> Get(HisBidTypeFilterQuery filter)
        {
            List<HIS_BID_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BID_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisBidTypeGet(param).Get(filter);
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

        
        public HIS_BID_TYPE GetById(long data)
        {
            HIS_BID_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BID_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidTypeGet(param).GetById(data);
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

        
        public HIS_BID_TYPE GetById(long data, HisBidTypeFilterQuery filter)
        {
            HIS_BID_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BID_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidTypeGet(param).GetById(data, filter);
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

        
        public HIS_BID_TYPE GetByCode(string data)
        {
            HIS_BID_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BID_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidTypeGet(param).GetByCode(data);
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

        
        public HIS_BID_TYPE GetByCode(string data, HisBidTypeFilterQuery filter)
        {
            HIS_BID_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BID_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidTypeGet(param).GetByCode(data, filter);
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
