using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRoleUser
{
    partial class HisExecuteRoleUserGet : BusinessBase
    {
        internal HisExecuteRoleUserGet()
            : base()
        {

        }

        internal HisExecuteRoleUserGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXECUTE_ROLE_USER> Get(HisExecuteRoleUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoleUserDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_ROLE_USER GetById(long id)
        {
            try
            {
                return GetById(id, new HisExecuteRoleUserFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXECUTE_ROLE_USER GetById(long id, HisExecuteRoleUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExecuteRoleUserDAO.GetById(id, filter.Query());
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
