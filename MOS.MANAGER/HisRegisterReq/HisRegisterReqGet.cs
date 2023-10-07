using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRegisterReq
{
    partial class HisRegisterReqGet : BusinessBase
    {
        internal HisRegisterReqGet()
            : base()
        {

        }

        internal HisRegisterReqGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_REGISTER_REQ> Get(HisRegisterReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRegisterReqDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REGISTER_REQ GetById(long id)
        {
            try
            {
                return GetById(id, new HisRegisterReqFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REGISTER_REQ GetById(long id, HisRegisterReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRegisterReqDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_REGISTER_REQ> GetByRegisterGateId(long id)
        {
            HisRegisterReqFilterQuery filter = new HisRegisterReqFilterQuery();
            filter.REGISTER_GATE_ID = id;
            return this.Get(filter);
        }
    }
}
