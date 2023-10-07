using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisManufacturer
{
    public partial class HisManufacturerDAO : EntityBase
    {
        private HisManufacturerGet GetWorker
        {
            get
            {
                return (HisManufacturerGet)Worker.Get<HisManufacturerGet>();
            }
        }
        public List<HIS_MANUFACTURER> Get(HisManufacturerSO search, CommonParam param)
        {
            List<HIS_MANUFACTURER> result = new List<HIS_MANUFACTURER>();
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

        public HIS_MANUFACTURER GetById(long id, HisManufacturerSO search)
        {
            HIS_MANUFACTURER result = null;
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
