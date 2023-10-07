using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentHurtType
{
    public partial class HisAccidentHurtTypeManager : BusinessBase
    {
        public HisAccidentHurtTypeManager()
            : base()
        {

        }

        public HisAccidentHurtTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ACCIDENT_HURT_TYPE> Get(HisAccidentHurtTypeFilterQuery filter)
        {
             List<HIS_ACCIDENT_HURT_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACCIDENT_HURT_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHurtTypeGet(param).Get(filter);
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

        
        public  HIS_ACCIDENT_HURT_TYPE GetById(long data)
        {
             HIS_ACCIDENT_HURT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_HURT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHurtTypeGet(param).GetById(data);
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

        
        public  HIS_ACCIDENT_HURT_TYPE GetById(long data, HisAccidentHurtTypeFilterQuery filter)
        {
             HIS_ACCIDENT_HURT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_HURT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHurtTypeGet(param).GetById(data, filter);
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

        
        public  HIS_ACCIDENT_HURT_TYPE GetByCode(string data)
        {
             HIS_ACCIDENT_HURT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_HURT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHurtTypeGet(param).GetByCode(data);
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

        
        public  HIS_ACCIDENT_HURT_TYPE GetByCode(string data, HisAccidentHurtTypeFilterQuery filter)
        {
             HIS_ACCIDENT_HURT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_HURT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHurtTypeGet(param).GetByCode(data, filter);
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
