using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBranch
{
    partial class HisBranchGet : BusinessBase
    {
        internal HIS_BRANCH GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBranchFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BRANCH GetByCode(string code, HisBranchFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBranchDAO.GetByCode(code, filter.Query());
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
