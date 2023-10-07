using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisOweType
{
    partial class HisOweTypeGet : BusinessBase
    {
        internal HIS_OWE_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisOweTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_OWE_TYPE GetByCode(string code, HisOweTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisOweTypeDAO.GetByCode(code, filter.Query());
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
