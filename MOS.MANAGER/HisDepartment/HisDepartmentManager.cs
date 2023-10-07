using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepartment
{
    public partial class HisDepartmentManager : BusinessBase
    {
        public HisDepartmentManager()
            : base()
        {

        }

        public HisDepartmentManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_DEPARTMENT> Get(HisDepartmentFilterQuery filter)
        {
            List<HIS_DEPARTMENT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DEPARTMENT> resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).Get(filter);
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

        
        public HIS_DEPARTMENT GetById(long data)
        {
            HIS_DEPARTMENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DEPARTMENT resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).GetById(data);
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

        
        public HIS_DEPARTMENT GetById(long data, HisDepartmentFilterQuery filter)
        {
            HIS_DEPARTMENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DEPARTMENT resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).GetById(data, filter);
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

        
        public HIS_DEPARTMENT GetByCode(string data)
        {
            HIS_DEPARTMENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DEPARTMENT resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).GetByCode(data);
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

        
        public HIS_DEPARTMENT GetByCode(string data, HisDepartmentFilterQuery filter)
        {
            HIS_DEPARTMENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DEPARTMENT resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).GetByCode(data, filter);
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

        
        public List<HIS_DEPARTMENT> GetByBranchId(long filter)
        {
            List<HIS_DEPARTMENT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DEPARTMENT> resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).GetByBranchId(filter);
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
