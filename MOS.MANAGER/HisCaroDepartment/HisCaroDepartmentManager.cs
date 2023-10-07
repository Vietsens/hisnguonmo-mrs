using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCaroDepartment
{
    public partial class HisCaroDepartmentManager : BusinessBase
    {
        public HisCaroDepartmentManager()
            : base()
        {

        }

        public HisCaroDepartmentManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_CARO_DEPARTMENT> Get(HisCaroDepartmentFilterQuery filter)
        {
             List<HIS_CARO_DEPARTMENT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_CARO_DEPARTMENT> resultData = null;
                if (valid)
                {
                    resultData = new HisCaroDepartmentGet(param).Get(filter);
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

        
        public  HIS_CARO_DEPARTMENT GetById(long data)
        {
             HIS_CARO_DEPARTMENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_CARO_DEPARTMENT resultData = null;
                if (valid)
                {
                    resultData = new HisCaroDepartmentGet(param).GetById(data);
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

        
        public  HIS_CARO_DEPARTMENT GetById(long data, HisCaroDepartmentFilterQuery filter)
        {
             HIS_CARO_DEPARTMENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_CARO_DEPARTMENT resultData = null;
                if (valid)
                {
                    resultData = new HisCaroDepartmentGet(param).GetById(data, filter);
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
