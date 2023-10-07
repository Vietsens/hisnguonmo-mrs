using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodRh
{
    public partial class HisBloodRhManager : BusinessBase
    {
        public HisBloodRhManager()
            : base()
        {

        }

        public HisBloodRhManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_BLOOD_RH> Get(HisBloodRhFilterQuery filter)
        {
            List<HIS_BLOOD_RH> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BLOOD_RH> resultData = null;
                if (valid)
                {
                    resultData = new HisBloodRhGet(param).Get(filter);
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

        
        public HIS_BLOOD_RH GetById(long data)
        {
            HIS_BLOOD_RH result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BLOOD_RH resultData = null;
                if (valid)
                {
                    resultData = new HisBloodRhGet(param).GetById(data);
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

        
        public HIS_BLOOD_RH GetById(long data, HisBloodRhFilterQuery filter)
        {
            HIS_BLOOD_RH result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BLOOD_RH resultData = null;
                if (valid)
                {
                    resultData = new HisBloodRhGet(param).GetById(data, filter);
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

        
        public HIS_BLOOD_RH GetByCode(string data)
        {
            HIS_BLOOD_RH result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BLOOD_RH resultData = null;
                if (valid)
                {
                    resultData = new HisBloodRhGet(param).GetByCode(data);
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

        
        public HIS_BLOOD_RH GetByCode(string data, HisBloodRhFilterQuery filter)
        {
            HIS_BLOOD_RH result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BLOOD_RH resultData = null;
                if (valid)
                {
                    resultData = new HisBloodRhGet(param).GetByCode(data, filter);
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
