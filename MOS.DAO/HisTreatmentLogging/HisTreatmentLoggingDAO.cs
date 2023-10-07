using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentLogging
{
    public partial class HisTreatmentLoggingDAO : EntityBase
    {
        private HisTreatmentLoggingGet GetWorker
        {
            get
            {
                return (HisTreatmentLoggingGet)Worker.Get<HisTreatmentLoggingGet>();
            }
        }
        public List<HIS_TREATMENT_LOGGING> Get(HisTreatmentLoggingSO search, CommonParam param)
        {
            List<HIS_TREATMENT_LOGGING> result = new List<HIS_TREATMENT_LOGGING>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_TREATMENT_LOGGING GetById(long id, HisTreatmentLoggingSO search)
        {
            HIS_TREATMENT_LOGGING result = null;
            try
            {
                result = GetWorker.GetById(id, search);
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
