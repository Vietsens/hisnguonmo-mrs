using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentLocation
{
    public partial class HisAccidentLocationDAO : EntityBase
    {
        private HisAccidentLocationGet GetWorker
        {
            get
            {
                return (HisAccidentLocationGet)Worker.Get<HisAccidentLocationGet>();
            }
        }
        public List<HIS_ACCIDENT_LOCATION> Get(HisAccidentLocationSO search, CommonParam param)
        {
            List<HIS_ACCIDENT_LOCATION> result = new List<HIS_ACCIDENT_LOCATION>();
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

        public HIS_ACCIDENT_LOCATION GetById(long id, HisAccidentLocationSO search)
        {
            HIS_ACCIDENT_LOCATION result = null;
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
