using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceReqType
{
    public partial class HisServiceReqTypeDAO : EntityBase
    {
        private HisServiceReqTypeGet GetWorker
        {
            get
            {
                return (HisServiceReqTypeGet)Worker.Get<HisServiceReqTypeGet>();
            }
        }
        public List<HIS_SERVICE_REQ_TYPE> Get(HisServiceReqTypeSO search, CommonParam param)
        {
            List<HIS_SERVICE_REQ_TYPE> result = new List<HIS_SERVICE_REQ_TYPE>();
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

        public HIS_SERVICE_REQ_TYPE GetById(long id, HisServiceReqTypeSO search)
        {
            HIS_SERVICE_REQ_TYPE result = null;
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
