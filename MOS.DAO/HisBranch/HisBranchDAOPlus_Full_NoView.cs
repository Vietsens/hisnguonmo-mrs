using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBranch
{
    public partial class HisBranchDAO : EntityBase
    {
        public HIS_BRANCH GetByCode(string code, HisBranchSO search)
        {
            HIS_BRANCH result = null;

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

        public Dictionary<string, HIS_BRANCH> GetDicByCode(HisBranchSO search, CommonParam param)
        {
            Dictionary<string, HIS_BRANCH> result = new Dictionary<string, HIS_BRANCH>();
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
