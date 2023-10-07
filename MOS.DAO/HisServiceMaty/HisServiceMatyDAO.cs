using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceMaty
{
    public partial class HisServiceMatyDAO : EntityBase
    {
        private HisServiceMatyGet GetWorker
        {
            get
            {
                return (HisServiceMatyGet)Worker.Get<HisServiceMatyGet>();
            }
        }
        public List<HIS_SERVICE_MATY> Get(HisServiceMatySO search, CommonParam param)
        {
            List<HIS_SERVICE_MATY> result = new List<HIS_SERVICE_MATY>();
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

        public HIS_SERVICE_MATY GetById(long id, HisServiceMatySO search)
        {
            HIS_SERVICE_MATY result = null;
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
