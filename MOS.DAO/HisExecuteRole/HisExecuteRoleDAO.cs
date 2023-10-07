using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExecuteRole
{
    public partial class HisExecuteRoleDAO : EntityBase
    {
        private HisExecuteRoleGet GetWorker
        {
            get
            {
                return (HisExecuteRoleGet)Worker.Get<HisExecuteRoleGet>();
            }
        }
        public List<HIS_EXECUTE_ROLE> Get(HisExecuteRoleSO search, CommonParam param)
        {
            List<HIS_EXECUTE_ROLE> result = new List<HIS_EXECUTE_ROLE>();
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

        public HIS_EXECUTE_ROLE GetById(long id, HisExecuteRoleSO search)
        {
            HIS_EXECUTE_ROLE result = null;
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
