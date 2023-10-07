using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedType
{
    public partial class HisBedTypeManager : BusinessBase
    {
        public HisBedTypeManager()
            : base()
        {

        }

        public HisBedTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_BED_TYPE> Get(HisBedTypeFilterQuery filter)
        {
            List<HIS_BED_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BED_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisBedTypeGet(param).Get(filter);
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

        
        public HIS_BED_TYPE GetById(long data)
        {
            HIS_BED_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BED_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBedTypeGet(param).GetById(data);
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

        
        public HIS_BED_TYPE GetById(long data, HisBedTypeFilterQuery filter)
        {
            HIS_BED_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BED_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBedTypeGet(param).GetById(data, filter);
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

        
        public HIS_BED_TYPE GetByCode(string data)
        {
            HIS_BED_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BED_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBedTypeGet(param).GetByCode(data);
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

        
        public HIS_BED_TYPE GetByCode(string data, HisBedTypeFilterQuery filter)
        {
            HIS_BED_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BED_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBedTypeGet(param).GetByCode(data, filter);
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
