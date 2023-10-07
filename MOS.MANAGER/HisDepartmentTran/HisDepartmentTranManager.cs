using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepartmentTran
{
    public partial class HisDepartmentTranManager : BusinessBase
    {
        public HisDepartmentTranManager()
            : base()
        {

        }

        public HisDepartmentTranManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_DEPARTMENT_TRAN> Get(HisDepartmentTranFilterQuery filter)
        {
             List<HIS_DEPARTMENT_TRAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DEPARTMENT_TRAN> resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).Get(filter);
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

        
        public  HIS_DEPARTMENT_TRAN GetById(long data)
        {
             HIS_DEPARTMENT_TRAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DEPARTMENT_TRAN resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetById(data);
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

        
        public  HIS_DEPARTMENT_TRAN GetById(long data, HisDepartmentTranFilterQuery filter)
        {
             HIS_DEPARTMENT_TRAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DEPARTMENT_TRAN resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetById(data, filter);
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

        
        public  List<HIS_DEPARTMENT_TRAN> GetByTreatmentId(long data)
        {
             List<HIS_DEPARTMENT_TRAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_DEPARTMENT_TRAN> resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetByTreatmentId(data);
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

        
        public  List<HIS_DEPARTMENT_TRAN> GetByDepartmentId(long data)
        {
             List<HIS_DEPARTMENT_TRAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_DEPARTMENT_TRAN> resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetByDepartmentId(data);
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

        
        public  HIS_DEPARTMENT_TRAN GetLastByTreatmentId(long treatmentId, long? beforeLogTime)
        {
             HIS_DEPARTMENT_TRAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(treatmentId);
                valid = valid && IsNotNull(beforeLogTime);
                HIS_DEPARTMENT_TRAN resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetLastByTreatmentId(treatmentId, beforeLogTime);
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

        
        public  HIS_DEPARTMENT_TRAN GetLastByTreatmentId(long treatmentId)
        {
             HIS_DEPARTMENT_TRAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(treatmentId);
                HIS_DEPARTMENT_TRAN resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetLastByTreatmentId(treatmentId);
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

        
        public  HIS_DEPARTMENT_TRAN GetFirstByTreatmentId(long treatmentId)
        {
             HIS_DEPARTMENT_TRAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(treatmentId);
                HIS_DEPARTMENT_TRAN resultData = null;
                if (valid)
                {
                    resultData = new HisDepartmentTranGet(param).GetFirstByTreatmentId(treatmentId);
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
