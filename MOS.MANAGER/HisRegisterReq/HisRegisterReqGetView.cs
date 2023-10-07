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
        internal List<V_HIS_REGISTER_REQ> GetView(HisRegisterReqViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRegisterReqDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_REGISTER_REQ GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisRegisterReqViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_REGISTER_REQ GetViewById(long id, HisRegisterReqViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRegisterReqDAO.GetViewById(id, filter.Query());
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
