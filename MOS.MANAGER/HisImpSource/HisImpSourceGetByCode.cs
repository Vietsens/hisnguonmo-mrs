using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpSource
{
    partial class HisImpSourceGet : BusinessBase
    {
        internal HIS_IMP_SOURCE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisImpSourceFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_SOURCE GetByCode(string code, HisImpSourceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpSourceDAO.GetByCode(code, filter.Query());
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
