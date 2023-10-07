using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExecuteRole
{
    public partial class HisExecuteRoleDAO : EntityBase
    {
        public HIS_EXECUTE_ROLE GetByCode(string code, HisExecuteRoleSO search)
        {
            HIS_EXECUTE_ROLE result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_EXECUTE_ROLE> GetDicByCode(HisExecuteRoleSO search, CommonParam param)
        {
            Dictionary<string, HIS_EXECUTE_ROLE> result = new Dictionary<string, HIS_EXECUTE_ROLE>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
