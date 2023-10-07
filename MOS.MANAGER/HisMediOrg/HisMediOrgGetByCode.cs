using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediOrg
{
    partial class HisMediOrgGet : BusinessBase
    {
        internal HIS_MEDI_ORG GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisMediOrgFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_ORG GetByCode(string code, HisMediOrgFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediOrgDAO.GetByCode(code, filter.Query());
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
