using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkipUser
{
    partial class HisEkipUserGet : BusinessBase
    {
        internal List<V_HIS_EKIP_USER> GetView(HisEkipUserViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEkipUserDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EKIP_USER GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisEkipUserViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EKIP_USER GetViewById(long id, HisEkipUserViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEkipUserDAO.GetViewById(id, filter.Query());
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
