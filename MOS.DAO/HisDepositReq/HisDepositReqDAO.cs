using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDepositReq
{
    public partial class HisDepositReqDAO : EntityBase
    {
        private HisDepositReqGet GetWorker
        {
            get
            {
                return (HisDepositReqGet)Worker.Get<HisDepositReqGet>();
            }
        }
        public List<HIS_DEPOSIT_REQ> Get(HisDepositReqSO search, CommonParam param)
        {
            List<HIS_DEPOSIT_REQ> result = new List<HIS_DEPOSIT_REQ>();
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

        public HIS_DEPOSIT_REQ GetById(long id, HisDepositReqSO search)
        {
            HIS_DEPOSIT_REQ result = null;
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
