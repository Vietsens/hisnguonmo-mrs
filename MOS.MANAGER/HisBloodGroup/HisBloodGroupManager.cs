using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodGroup
{
    public partial class HisBloodGroupManager : BusinessBase
    {
        public HisBloodGroupManager()
            : base()
        {

        }

        public HisBloodGroupManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BLOOD_GROUP> Get(HisBloodGroupFilterQuery filter)
        {
             List<HIS_BLOOD_GROUP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BLOOD_GROUP> resultData = null;
                if (valid)
                {
                    resultData = new HisBloodGroupGet(param).Get(filter);
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

        
        public  HIS_BLOOD_GROUP GetById(long data)
        {
             HIS_BLOOD_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BLOOD_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisBloodGroupGet(param).GetById(data);
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

        
        public  HIS_BLOOD_GROUP GetById(long data, HisBloodGroupFilterQuery filter)
        {
             HIS_BLOOD_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BLOOD_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisBloodGroupGet(param).GetById(data, filter);
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

        
        public  HIS_BLOOD_GROUP GetByCode(string data)
        {
             HIS_BLOOD_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BLOOD_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisBloodGroupGet(param).GetByCode(data);
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

        
        public  HIS_BLOOD_GROUP GetByCode(string data, HisBloodGroupFilterQuery filter)
        {
             HIS_BLOOD_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BLOOD_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisBloodGroupGet(param).GetByCode(data, filter);
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
