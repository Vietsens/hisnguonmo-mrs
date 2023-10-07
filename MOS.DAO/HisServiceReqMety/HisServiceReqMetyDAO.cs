using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceReqMety
{
    public partial class HisServiceReqMetyDAO : EntityBase
    {
        private HisServiceReqMetyGet GetWorker
        {
            get
            {
                return (HisServiceReqMetyGet)Worker.Get<HisServiceReqMetyGet>();
            }
        }
        public List<HIS_SERVICE_REQ_METY> Get(HisServiceReqMetySO search, CommonParam param)
        {
            List<HIS_SERVICE_REQ_METY> result = new List<HIS_SERVICE_REQ_METY>();
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

        public HIS_SERVICE_REQ_METY GetById(long id, HisServiceReqMetySO search)
        {
            HIS_SERVICE_REQ_METY result = null;
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
