using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRoleUser
{
    public partial class HisExecuteRoleUserManager : BusinessBase
    {
        public HisExecuteRoleUserManager()
            : base()
        {

        }

        public HisExecuteRoleUserManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EXECUTE_ROLE_USER> Get(HisExecuteRoleUserFilterQuery filter)
        {
             List<HIS_EXECUTE_ROLE_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXECUTE_ROLE_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoleUserGet(param).Get(filter);
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

        
        public  HIS_EXECUTE_ROLE_USER GetById(long data)
        {
             HIS_EXECUTE_ROLE_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXECUTE_ROLE_USER resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoleUserGet(param).GetById(data);
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

        
        public  HIS_EXECUTE_ROLE_USER GetById(long data, HisExecuteRoleUserFilterQuery filter)
        {
             HIS_EXECUTE_ROLE_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXECUTE_ROLE_USER resultData = null;
                if (valid)
                {
                    resultData = new HisExecuteRoleUserGet(param).GetById(data, filter);
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
