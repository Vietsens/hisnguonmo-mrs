using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceReqStt
{
    public partial class HisServiceReqSttDAO : EntityBase
    {
        private HisServiceReqSttGet GetWorker
        {
            get
            {
                return (HisServiceReqSttGet)Worker.Get<HisServiceReqSttGet>();
            }
        }
        public List<HIS_SERVICE_REQ_STT> Get(HisServiceReqSttSO search, CommonParam param)
        {
            List<HIS_SERVICE_REQ_STT> result = new List<HIS_SERVICE_REQ_STT>();
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

        public HIS_SERVICE_REQ_STT GetById(long id, HisServiceReqSttSO search)
        {
            HIS_SERVICE_REQ_STT result = null;
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
