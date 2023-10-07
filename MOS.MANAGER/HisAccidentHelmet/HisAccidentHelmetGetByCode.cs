using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentHelmet
{
    partial class HisAccidentHelmetGet : BusinessBase
    {
        internal HIS_ACCIDENT_HELMET GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisAccidentHelmetFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_HELMET GetByCode(string code, HisAccidentHelmetFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentHelmetDAO.GetByCode(code, filter.Query());
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
