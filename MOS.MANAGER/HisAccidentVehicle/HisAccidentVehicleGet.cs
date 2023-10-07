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
        internal HisAccidentVehicleGet()
            : base()
        {

        }

        internal HisAccidentVehicleGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACCIDENT_VEHICLE> Get(HisAccidentVehicleFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentVehicleDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_VEHICLE GetById(long id)
        {
            try
            {
                return GetById(id, new HisAccidentVehicleFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_VEHICLE GetById(long id, HisAccidentVehicleFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentVehicleDAO.GetById(id, filter.Query());
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
