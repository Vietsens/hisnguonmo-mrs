using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestBlood
{
    public partial class HisImpMestBloodManager : BusinessBase
    {
        public HisImpMestBloodManager()
            : base()
        {

        }

        public HisImpMestBloodManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_IMP_MEST_BLOOD> Get(HisImpMestBloodFilterQuery filter)
        {
            List<HIS_IMP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_IMP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestBloodGet(param).Get(filter);
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

        
        public HIS_IMP_MEST_BLOOD GetById(long data)
        {
            HIS_IMP_MEST_BLOOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_IMP_MEST_BLOOD resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestBloodGet(param).GetById(data);
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

        
        public HIS_IMP_MEST_BLOOD GetById(long data, HisImpMestBloodFilterQuery filter)
        {
            HIS_IMP_MEST_BLOOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_IMP_MEST_BLOOD resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestBloodGet(param).GetById(data, filter);
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

        
        public List<HIS_IMP_MEST_BLOOD> GetByImpMestId(long data)
        {
            List<HIS_IMP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_IMP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestBloodGet(param).GetByImpMestId(data);
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

        
        public List<HIS_IMP_MEST_BLOOD> GetByBloodId(long data)
        {
            List<HIS_IMP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_IMP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestBloodGet(param).GetByBloodId(data);
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
