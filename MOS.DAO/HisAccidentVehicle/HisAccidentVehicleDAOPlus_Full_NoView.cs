using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentVehicle
{
    public partial class HisAccidentVehicleDAO : EntityBase
    {
        public HIS_ACCIDENT_VEHICLE GetByCode(string code, HisAccidentVehicleSO search)
        {
            HIS_ACCIDENT_VEHICLE result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_ACCIDENT_VEHICLE> GetDicByCode(HisAccidentVehicleSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCIDENT_VEHICLE> result = new Dictionary<string, HIS_ACCIDENT_VEHICLE>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
