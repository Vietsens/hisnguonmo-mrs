using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMachine
{
    partial class HisMachineGet : BusinessBase
    {
        internal List<V_HIS_MACHINE> GetView(HisMachineViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMachineDAO.GetView(filter.Query(), param);
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
