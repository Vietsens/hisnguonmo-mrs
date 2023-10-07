using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentVehicle
{
    partial class HisAccidentVehicleGet : BusinessBase
    {
        internal HIS_ACCIDENT_VEHICLE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisAccidentVehicleFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_VEHICLE GetByCode(string code, HisAccidentVehicleFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentVehicleDAO.GetByCode(code, filter.Query());
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
