using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepartmentTran
{
    public partial class HisDepartmentTranManager : BusinessBase
    {
        
        public List<V_HIS_DEPARTMENT_TRAN> GetView(HisDepartmentTranViewFilterQuery filter)
        {
            List<V_HIS_DEPARTMENT_TRAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_DEPARTMENT_TRAN> resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetView(filter);
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

        
        public V_HIS_DEPARTMENT_TRAN GetViewById(long data)
        {
            V_HIS_DEPARTMENT_TRAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_DEPARTMENT_TRAN resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetViewById(data);
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

        
        public V_HIS_DEPARTMENT_TRAN GetViewById(long data, HisDepartmentTranViewFilterQuery filter)
        {
            V_HIS_DEPARTMENT_TRAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_DEPARTMENT_TRAN resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_DEPARTMENT_TRAN> GetViewByDepartmentId(long data)
        {
            List<V_HIS_DEPARTMENT_TRAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_DEPARTMENT_TRAN> resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetViewByDepartmentId(data);
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

        
        public List<V_HIS_DEPARTMENT_TRAN> GetViewByTreatmentId(long data)
        {
            List<V_HIS_DEPARTMENT_TRAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_DEPARTMENT_TRAN> resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetViewByTreatmentId(data);
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

        
        public List<V_HIS_DEPARTMENT_TRAN> GetViewByTreatmentIds(List<long> data)
        {
            List<V_HIS_DEPARTMENT_TRAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_DEPARTMENT_TRAN> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_DEPARTMENT_TRAN>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisDepartmentTranGet(param).GetViewByTreatmentIds(Ids));
                    }
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

        
        public V_HIS_DEPARTMENT_TRAN GetViewLastByTreatmentId(long data)
        {
            V_HIS_DEPARTMENT_TRAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_DEPARTMENT_TRAN resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetViewLastByTreatmentId(data);
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

        
        public V_HIS_DEPARTMENT_TRAN GetViewLastByTreatmentId(long treatmentId, long? beforeLogTime)
        {
            V_HIS_DEPARTMENT_TRAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(treatmentId);
                valid = valid && IsNotNull(beforeLogTime);
                V_HIS_DEPARTMENT_TRAN resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetViewLastByTreatmentId(treatmentId, beforeLogTime);
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

        
        public V_HIS_DEPARTMENT_TRAN GetViewFirstByTreatmentId(long data)
        {
            V_HIS_DEPARTMENT_TRAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_DEPARTMENT_TRAN resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetViewFirstByTreatmentId(data);
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

        
        public V_HIS_DEPARTMENT_TRAN GetViewLastByTreatmentId(List<V_HIS_DEPARTMENT_TRAN> departmentTrans, long treatmentId)
        {
            V_HIS_DEPARTMENT_TRAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(departmentTrans);
                valid = valid && IsNotNull(treatmentId);
                V_HIS_DEPARTMENT_TRAN resultData = null;
                if (valid)
                {
                    resultData = new V_HIS_DEPARTMENT_TRAN();
                    var skip = 0;
                    while (departmentTrans.Count - skip > 0)
                    {
                        var Ids = departmentTrans.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData = new HisDepartmentTranGet(param).GetViewLastByTreatmentId(Ids, treatmentId);
                    }
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
