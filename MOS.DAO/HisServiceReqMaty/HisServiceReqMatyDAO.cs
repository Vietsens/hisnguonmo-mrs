using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceReqMaty
{
    public partial class HisServiceReqMatyDAO : EntityBase
    {
        private HisServiceReqMatyGet GetWorker
        {
            get
            {
                return (HisServiceReqMatyGet)Worker.Get<HisServiceReqMatyGet>();
            }
        }
        public List<HIS_SERVICE_REQ_MATY> Get(HisServiceReqMatySO search, CommonParam param)
        {
            List<HIS_SERVICE_REQ_MATY> result = new List<HIS_SERVICE_REQ_MATY>();
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

        public HIS_SERVICE_REQ_MATY GetById(long id, HisServiceReqMatySO search)
        {
            HIS_SERVICE_REQ_MATY result = null;
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
