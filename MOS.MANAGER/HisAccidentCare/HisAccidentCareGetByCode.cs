using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentCare
{
    partial class HisAccidentCareGet : BusinessBase
    {
        internal HIS_ACCIDENT_CARE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisAccidentCareFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_CARE GetByCode(string code, HisAccidentCareFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentCareDAO.GetByCode(code, filter.Query());
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
