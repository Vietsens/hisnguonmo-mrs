using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCareType
{
    public partial class HisCareTypeManager : BusinessBase
    {
        public HisCareTypeManager()
            : base()
        {

        }

        public HisCareTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_CARE_TYPE> Get(HisCareTypeFilterQuery filter)
        {
             List<HIS_CARE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_CARE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisCareTypeGet(param).Get(filter);
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

        
        public  HIS_CARE_TYPE GetById(long data)
        {
             HIS_CARE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_CARE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisCareTypeGet(param).GetById(data);
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

        
        public  HIS_CARE_TYPE GetById(long data, HisCareTypeFilterQuery filter)
        {
             HIS_CARE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_CARE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisCareTypeGet(param).GetById(data, filter);
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

        
        public  HIS_CARE_TYPE GetByCode(string data, HisCareTypeFilterQuery filter)
        {
             HIS_CARE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_CARE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisCareTypeGet(param).GetByCode(data, filter);
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

        
        public  HIS_CARE_TYPE GetByCode(string data)
        {
             HIS_CARE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_CARE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisCareTypeGet(param).GetByCode(data);
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
