using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServicePaty
{
    public partial class HisServicePatyDAO : EntityBase
    {
        private HisServicePatyGet GetWorker
        {
            get
            {
                return (HisServicePatyGet)Worker.Get<HisServicePatyGet>();
            }
        }
        public List<HIS_SERVICE_PATY> Get(HisServicePatySO search, CommonParam param)
        {
            List<HIS_SERVICE_PATY> result = new List<HIS_SERVICE_PATY>();
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

        public HIS_SERVICE_PATY GetById(long id, HisServicePatySO search)
        {
            HIS_SERVICE_PATY result = null;
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
