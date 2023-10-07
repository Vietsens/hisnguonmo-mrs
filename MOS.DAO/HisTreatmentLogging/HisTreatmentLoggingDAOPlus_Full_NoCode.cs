using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentLogging
{
    public partial class HisTreatmentLoggingDAO : EntityBase
    {
        public List<V_HIS_TREATMENT_LOGGING> GetView(HisTreatmentLoggingSO search, CommonParam param)
        {
            List<V_HIS_TREATMENT_LOGGING> result = new List<V_HIS_TREATMENT_LOGGING>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_TREATMENT_LOGGING GetViewById(long id, HisTreatmentLoggingSO search)
        {
            V_HIS_TREATMENT_LOGGING result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
