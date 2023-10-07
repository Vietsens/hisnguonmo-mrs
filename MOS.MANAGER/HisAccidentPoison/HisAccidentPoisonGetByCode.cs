using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentPoison
{
    partial class HisAccidentPoisonGet : BusinessBase
    {
        internal HIS_ACCIDENT_POISON GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisAccidentPoisonFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_POISON GetByCode(string code, HisAccidentPoisonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentPoisonDAO.GetByCode(code, filter.Query());
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
