using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentResult
{
    partial class HisAccidentResultGet : BusinessBase
    {
        internal HIS_ACCIDENT_RESULT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisAccidentResultFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_RESULT GetByCode(string code, HisAccidentResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentResultDAO.GetByCode(code, filter.Query());
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
