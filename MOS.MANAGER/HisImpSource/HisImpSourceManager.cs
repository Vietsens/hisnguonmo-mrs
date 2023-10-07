using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpSource
{
    public partial class HisImpSourceManager : BusinessBase
    {
        public HisImpSourceManager()
            : base()
        {

        }

        public HisImpSourceManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_IMP_SOURCE> Get(HisImpSourceFilterQuery filter)
        {
             List<HIS_IMP_SOURCE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_IMP_SOURCE> resultData = null;
                if (valid)
                {
                    resultData = new HisImpSourceGet(param).Get(filter);
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

        
        public  HIS_IMP_SOURCE GetById(long data)
        {
             HIS_IMP_SOURCE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_IMP_SOURCE resultData = null;
                if (valid)
                {
                    resultData = new HisImpSourceGet(param).GetById(data);
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

        
        public  HIS_IMP_SOURCE GetById(long data, HisImpSourceFilterQuery filter)
        {
             HIS_IMP_SOURCE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_IMP_SOURCE resultData = null;
                if (valid)
                {
                    resultData = new HisImpSourceGet(param).GetById(data, filter);
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

        
        public  HIS_IMP_SOURCE GetByCode(string data)
        {
             HIS_IMP_SOURCE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_IMP_SOURCE resultData = null;
                if (valid)
                {
                    resultData = new HisImpSourceGet(param).GetByCode(data);
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

        
        public  HIS_IMP_SOURCE GetByCode(string data, HisImpSourceFilterQuery filter)
        {
             HIS_IMP_SOURCE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_IMP_SOURCE resultData = null;
                if (valid)
                {
                    resultData = new HisImpSourceGet(param).GetByCode(data, filter);
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
