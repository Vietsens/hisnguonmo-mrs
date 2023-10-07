using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRole
{
    partial class HisExecuteRoleGet : BusinessBase
    {
        internal HIS_EXECUTE_ROLE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisExecuteRoleFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_ROLE GetByCode(string code, HisExecuteRoleFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoleDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
