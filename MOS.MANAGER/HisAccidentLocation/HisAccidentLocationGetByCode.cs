using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentLocation
{
    partial class HisAccidentLocationGet : BusinessBase
    {
        internal HIS_ACCIDENT_LOCATION GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisAccidentLocationFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_LOCATION GetByCode(string code, HisAccidentLocationFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentLocationDAO.GetByCode(code, filter.Query());
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
