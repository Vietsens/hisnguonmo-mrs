using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepositReq
{
    partial class HisDepositReqGet : BusinessBase
    {
        internal HisDepositReqGet()
            : base()
        {

        }

        internal HisDepositReqGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_DEPOSIT_REQ> Get(HisDepositReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepositReqDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEPOSIT_REQ GetById(long id)
        {
            try
            {
                return GetById(id, new HisDepositReqFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEPOSIT_REQ GetById(long id, HisDepositReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepositReqDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
