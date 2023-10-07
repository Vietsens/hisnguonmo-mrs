using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceUnit
{
    public partial class HisServiceUnitDAO : EntityBase
    {
        private HisServiceUnitGet GetWorker
        {
            get
            {
                return (HisServiceUnitGet)Worker.Get<HisServiceUnitGet>();
            }
        }
        public List<HIS_SERVICE_UNIT> Get(HisServiceUnitSO search, CommonParam param)
        {
            List<HIS_SERVICE_UNIT> result = new List<HIS_SERVICE_UNIT>();
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

        public HIS_SERVICE_UNIT GetById(long id, HisServiceUnitSO search)
        {
            HIS_SERVICE_UNIT result = null;
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
