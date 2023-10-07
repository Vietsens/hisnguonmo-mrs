using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisIcdService
{
    partial class HisIcdServiceGet : BusinessBase
    {
        internal List<V_HIS_ICD_SERVICE> GetView(HisIcdServiceViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisIcdServiceDAO.GetView(filter.Query(), param);
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
