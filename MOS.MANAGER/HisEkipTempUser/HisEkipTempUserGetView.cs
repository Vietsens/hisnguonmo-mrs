using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkipTempUser
{
    partial class HisEkipTempUserGet : BusinessBase
    {
        internal List<V_HIS_EKIP_TEMP_USER> GetView(HisEkipTempUserViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEkipTempUserDAO.GetView(filter.Query(), param);
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
