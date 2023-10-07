using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExecuteRoleUser
{
    public partial class HisExecuteRoleUserDAO : EntityBase
    {
        private HisExecuteRoleUserGet GetWorker
        {
            get
            {
                return (HisExecuteRoleUserGet)Worker.Get<HisExecuteRoleUserGet>();
            }
        }
        public List<HIS_EXECUTE_ROLE_USER> Get(HisExecuteRoleUserSO search, CommonParam param)
        {
            List<HIS_EXECUTE_ROLE_USER> result = new List<HIS_EXECUTE_ROLE_USER>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_EXECUTE_ROLE_USER GetById(long id, HisExecuteRoleUserSO search)
        {
            HIS_EXECUTE_ROLE_USER result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
