using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisIcdCm
{
    partial class HisIcdCmGet : BusinessBase
    {
        internal HIS_ICD_CM GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisIcdCmFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ICD_CM GetByCode(string code, HisIcdCmFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisIcdCmDAO.GetByCode(code, filter.Query());
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
