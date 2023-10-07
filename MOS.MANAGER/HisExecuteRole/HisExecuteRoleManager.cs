using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRole
{
    public partial class HisExecuteRoleManager : BusinessBase
    {
        public HisExecuteRoleManager()
            : base()
        {

        }

        public HisExecuteRoleManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EXECUTE_ROLE> Get(HisExecuteRoleFilterQuery filter)
        {
             List<HIS_EXECUTE_ROLE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXECUTE_ROLE> resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoleGet(param).Get(filter);
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

        
        public  HIS_EXECUTE_ROLE GetById(long data)
        {
             HIS_EXECUTE_ROLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXECUTE_ROLE resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoleGet(param).GetById(data);
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

        
        public  HIS_EXECUTE_ROLE GetById(long data, HisExecuteRoleFilterQuery filter)
        {
             HIS_EXECUTE_ROLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXECUTE_ROLE resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoleGet(param).GetById(data, filter);
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

        
        public  HIS_EXECUTE_ROLE GetByCode(string data)
        {
             HIS_EXECUTE_ROLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXECUTE_ROLE resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoleGet(param).GetByCode(data);
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

        
        public  HIS_EXECUTE_ROLE GetByCode(string data, HisExecuteRoleFilterQuery filter)
        {
             HIS_EXECUTE_ROLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXECUTE_ROLE resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoleGet(param).GetByCode(data, filter);
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
