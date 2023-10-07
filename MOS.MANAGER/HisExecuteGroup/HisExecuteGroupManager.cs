using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteGroup
{
    public partial class HisExecuteGroupManager : BusinessBase
    {
        public HisExecuteGroupManager()
            : base()
        {

        }

        public HisExecuteGroupManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EXECUTE_GROUP> Get(HisExecuteGroupFilterQuery filter)
        {
             List<HIS_EXECUTE_GROUP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXECUTE_GROUP> resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteGroupGet(param).Get(filter);
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

        
        public  HIS_EXECUTE_GROUP GetById(long data)
        {
             HIS_EXECUTE_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXECUTE_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteGroupGet(param).GetById(data);
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

        
        public  HIS_EXECUTE_GROUP GetById(long data, HisExecuteGroupFilterQuery filter)
        {
             HIS_EXECUTE_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXECUTE_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteGroupGet(param).GetById(data, filter);
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

        
        public  HIS_EXECUTE_GROUP GetByCode(string data)
        {
             HIS_EXECUTE_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXECUTE_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteGroupGet(param).GetByCode(data);
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

        
        public  HIS_EXECUTE_GROUP GetByCode(string data, HisExecuteGroupFilterQuery filter)
        {
             HIS_EXECUTE_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXECUTE_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteGroupGet(param).GetByCode(data, filter);
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
