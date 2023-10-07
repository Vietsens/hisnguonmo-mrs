using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRegisterReq
{
    public partial class HisRegisterReqDAO : EntityBase
    {
        private HisRegisterReqGet GetWorker
        {
            get
            {
                return (HisRegisterReqGet)Worker.Get<HisRegisterReqGet>();
            }
        }
        public List<HIS_REGISTER_REQ> Get(HisRegisterReqSO search, CommonParam param)
        {
            List<HIS_REGISTER_REQ> result = new List<HIS_REGISTER_REQ>();
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

        public HIS_REGISTER_REQ GetById(long id, HisRegisterReqSO search)
        {
            HIS_REGISTER_REQ result = null;
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
