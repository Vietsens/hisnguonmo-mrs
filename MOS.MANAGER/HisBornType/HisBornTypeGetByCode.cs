using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBornType
{
    partial class HisBornTypeGet : BusinessBase
    {
        internal HIS_BORN_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBornTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BORN_TYPE GetByCode(string code, HisBornTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBornTypeDAO.GetByCode(code, filter.Query());
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
