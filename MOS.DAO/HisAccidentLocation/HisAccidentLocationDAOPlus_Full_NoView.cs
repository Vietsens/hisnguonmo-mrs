using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentLocation
{
    public partial class HisAccidentLocationDAO : EntityBase
    {
        public HIS_ACCIDENT_LOCATION GetByCode(string code, HisAccidentLocationSO search)
        {
            HIS_ACCIDENT_LOCATION result = null;

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

        public Dictionary<string, HIS_ACCIDENT_LOCATION> GetDicByCode(HisAccidentLocationSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCIDENT_LOCATION> result = new Dictionary<string, HIS_ACCIDENT_LOCATION>();
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
