using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCoTreatment
{
    partial class HisCoTreatmentGet : BusinessBase
    {
        internal List<V_HIS_CO_TREATMENT> GetView(HisCoTreatmentViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCoTreatmentDAO.GetView(filter.Query(), param);
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
