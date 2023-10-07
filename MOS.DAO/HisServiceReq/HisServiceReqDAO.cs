using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceReq
{
    public partial class HisServiceReqDAO : EntityBase
    {
        private HisServiceReqGet GetWorker
        {
            get
            {
                return (HisServiceReqGet)Worker.Get<HisServiceReqGet>();
            }
        }
        public List<HIS_SERVICE_REQ> Get(HisServiceReqSO search, CommonParam param)
        {
            List<HIS_SERVICE_REQ> result = new List<HIS_SERVICE_REQ>();
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

        public HIS_SERVICE_REQ GetById(long id, HisServiceReqSO search)
        {
            HIS_SERVICE_REQ result = null;
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
