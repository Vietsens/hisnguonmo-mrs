using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentVehicle
{
    public partial class HisAccidentVehicleDAO : EntityBase
    {
        private HisAccidentVehicleGet GetWorker
        {
            get
            {
                return (HisAccidentVehicleGet)Worker.Get<HisAccidentVehicleGet>();
            }
        }
        public List<HIS_ACCIDENT_VEHICLE> Get(HisAccidentVehicleSO search, CommonParam param)
        {
            List<HIS_ACCIDENT_VEHICLE> result = new List<HIS_ACCIDENT_VEHICLE>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_ACCIDENT_VEHICLE GetById(long id, HisAccidentVehicleSO search)
        {
            HIS_ACCIDENT_VEHICLE result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
