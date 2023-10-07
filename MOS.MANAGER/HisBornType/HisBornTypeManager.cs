using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBornType
{
    public partial class HisBornTypeManager : BusinessBase
    {
        public HisBornTypeManager()
            : base()
        {

        }

        public HisBornTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BORN_TYPE> Get(HisBornTypeFilterQuery filter)
        {
             List<HIS_BORN_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BORN_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisBornTypeGet(param).Get(filter);
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

        
        public  HIS_BORN_TYPE GetById(long data)
        {
             HIS_BORN_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BORN_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBornTypeGet(param).GetById(data);
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

        
        public  HIS_BORN_TYPE GetById(long data, HisBornTypeFilterQuery filter)
        {
             HIS_BORN_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BORN_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBornTypeGet(param).GetById(data, filter);
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

        
        public  HIS_BORN_TYPE GetByCode(string data)
        {
             HIS_BORN_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BORN_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBornTypeGet(param).GetByCode(data);
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

        
        public  HIS_BORN_TYPE GetByCode(string data, HisBornTypeFilterQuery filter)
        {
             HIS_BORN_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BORN_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBornTypeGet(param).GetByCode(data, filter);
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
