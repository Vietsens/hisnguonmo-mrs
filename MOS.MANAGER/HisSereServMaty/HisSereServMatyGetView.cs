using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServMaty
{
    partial class HisSereServMatyGet : BusinessBase
    {
        internal List<V_HIS_SERE_SERV_MATY> GetView(HisSereServMatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServMatyDAO.GetView(filter.Query(), param);
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
