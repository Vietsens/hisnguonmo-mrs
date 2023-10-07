using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentLogging
{
    partial class HisTreatmentLoggingGet : BusinessBase
    {
        internal List<V_HIS_TREATMENT_LOGGING> GetView(HisTreatmentLoggingViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentLoggingDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT_LOGGING GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisTreatmentLoggingViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT_LOGGING GetViewById(long id, HisTreatmentLoggingViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentLoggingDAO.GetViewById(id, filter.Query());
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
