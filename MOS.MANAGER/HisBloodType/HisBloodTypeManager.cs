using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodType
{
    public partial class HisBloodTypeManager : BusinessBase
    {
        public HisBloodTypeManager()
            : base()
        {

        }

        public HisBloodTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BLOOD_TYPE> Get(HisBloodTypeFilterQuery filter)
        {
             List<HIS_BLOOD_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BLOOD_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisBloodTypeGet(param).Get(filter);
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

        
        public  HIS_BLOOD_TYPE GetById(long data)
        {
             HIS_BLOOD_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BLOOD_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBloodTypeGet(param).GetById(data);
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

        
        public  HIS_BLOOD_TYPE GetById(long data, HisBloodTypeFilterQuery filter)
        {
             HIS_BLOOD_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BLOOD_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBloodTypeGet(param).GetById(data, filter);
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

        
        public  HIS_BLOOD_TYPE GetByCode(string data)
        {
             HIS_BLOOD_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BLOOD_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBloodTypeGet(param).GetByCode(data);
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

        
        public  HIS_BLOOD_TYPE GetByCode(string data, HisBloodTypeFilterQuery filter)
        {
             HIS_BLOOD_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BLOOD_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBloodTypeGet(param).GetByCode(data, filter);
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
