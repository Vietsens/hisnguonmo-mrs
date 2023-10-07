using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepartment
{
    public partial class HisDepartmentManager : BusinessBase
    {
        
        public List<V_HIS_DEPARTMENT> GetView(HisDepartmentViewFilterQuery filter)
        {
            List<V_HIS_DEPARTMENT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_DEPARTMENT> resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).GetView(filter);
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

        
        public V_HIS_DEPARTMENT GetViewByCode(string data)
        {
            V_HIS_DEPARTMENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_DEPARTMENT resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).GetViewByCode(data);
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

        
        public V_HIS_DEPARTMENT GetViewByCode(string data, HisDepartmentViewFilterQuery filter)
        {
            V_HIS_DEPARTMENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_DEPARTMENT resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_DEPARTMENT GetViewById(long data)
        {
            V_HIS_DEPARTMENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_DEPARTMENT resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).GetViewById(data);
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

        
        public V_HIS_DEPARTMENT GetViewById(long data, HisDepartmentViewFilterQuery filter)
        {
            V_HIS_DEPARTMENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_DEPARTMENT resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_DEPARTMENT> GetViewByBranchId(long filter)
        {
            List<V_HIS_DEPARTMENT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_DEPARTMENT> resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentGet(param).GetViewByBranchId(filter);
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
