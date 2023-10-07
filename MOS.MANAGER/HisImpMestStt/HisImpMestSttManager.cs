using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestStt
{
    public partial class HisImpMestSttManager : BusinessBase
    {
        public HisImpMestSttManager()
            : base()
        {

        }

        public HisImpMestSttManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_IMP_MEST_STT> Get(HisImpMestSttFilterQuery filter)
        {
             List<HIS_IMP_MEST_STT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_IMP_MEST_STT> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestSttGet(param).Get(filter);
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

        
        public  HIS_IMP_MEST_STT GetById(long data)
        {
             HIS_IMP_MEST_STT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_IMP_MEST_STT resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestSttGet(param).GetById(data);
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

        
        public  HIS_IMP_MEST_STT GetById(long data, HisImpMestSttFilterQuery filter)
        {
             HIS_IMP_MEST_STT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_IMP_MEST_STT resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestSttGet(param).GetById(data, filter);
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

        
        public  HIS_IMP_MEST_STT GetByCode(string data)
        {
             HIS_IMP_MEST_STT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_IMP_MEST_STT resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestSttGet(param).GetByCode(data);
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

        
        public  HIS_IMP_MEST_STT GetByCode(string data, HisImpMestSttFilterQuery filter)
        {
             HIS_IMP_MEST_STT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_IMP_MEST_STT resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestSttGet(param).GetByCode(data, filter);
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
