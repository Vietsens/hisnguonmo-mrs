using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestType
{
    public partial class HisImpMestTypeManager : BusinessBase
    {
        public HisImpMestTypeManager()
            : base()
        {

        }

        public HisImpMestTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_IMP_MEST_TYPE> Get(HisImpMestTypeFilterQuery filter)
        {
             List<HIS_IMP_MEST_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_IMP_MEST_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestTypeGet(param).Get(filter);
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

        
        public  HIS_IMP_MEST_TYPE GetById(long data)
        {
             HIS_IMP_MEST_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_IMP_MEST_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestTypeGet(param).GetById(data);
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

        
        public  HIS_IMP_MEST_TYPE GetById(long data, HisImpMestTypeFilterQuery filter)
        {
             HIS_IMP_MEST_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_IMP_MEST_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestTypeGet(param).GetById(data, filter);
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

        
        public  HIS_IMP_MEST_TYPE GetByCode(string data)
        {
             HIS_IMP_MEST_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_IMP_MEST_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestTypeGet(param).GetByCode(data);
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

        
        public  HIS_IMP_MEST_TYPE GetByCode(string data, HisImpMestTypeFilterQuery filter)
        {
             HIS_IMP_MEST_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_IMP_MEST_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestTypeGet(param).GetByCode(data, filter);
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
