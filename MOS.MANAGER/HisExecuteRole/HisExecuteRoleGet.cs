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
        internal HisExecuteRoleGet()
            : base()
        {

        }

        internal HisExecuteRoleGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXECUTE_ROLE> Get(HisExecuteRoleFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoleDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_ROLE GetById(long id)
        {
            try
            {
                return GetById(id, new HisExecuteRoleFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_ROLE GetById(long id, HisExecuteRoleFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoleDAO.GetById(id, filter.Query());
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
