using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisIcdGroup
{
    public partial class HisIcdGroupManager : BusinessBase
    {
        public HisIcdGroupManager()
            : base()
        {

        }

        public HisIcdGroupManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ICD_GROUP> Get(HisIcdGroupFilterQuery filter)
        {
             List<HIS_ICD_GROUP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ICD_GROUP> resultData = null;
                if (valid)
                {
                    resultData = new HisIcdGroupGet(param).Get(filter);
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

        
        public  HIS_ICD_GROUP GetById(long data)
        {
             HIS_ICD_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ICD_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisIcdGroupGet(param).GetById(data);
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

        
        public  HIS_ICD_GROUP GetById(long data, HisIcdGroupFilterQuery filter)
        {
             HIS_ICD_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ICD_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisIcdGroupGet(param).GetById(data, filter);
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

        
        public  HIS_ICD_GROUP GetByCode(string data)
        {
             HIS_ICD_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ICD_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisIcdGroupGet(param).GetByCode(data);
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

        
        public  HIS_ICD_GROUP GetByCode(string data, HisIcdGroupFilterQuery filter)
        {
             HIS_ICD_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ICD_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisIcdGroupGet(param).GetByCode(data, filter);
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
