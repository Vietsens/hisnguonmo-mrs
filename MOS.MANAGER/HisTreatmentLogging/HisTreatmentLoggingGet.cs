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
        internal HisTreatmentLoggingGet()
            : base()
        {

        }

        internal HisTreatmentLoggingGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TREATMENT_LOGGING> Get(HisTreatmentLoggingFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentLoggingDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_LOGGING GetById(long id)
        {
            try
            {
                return GetById(id, new HisTreatmentLoggingFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_LOGGING GetById(long id, HisTreatmentLoggingFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentLoggingDAO.GetById(id, filter.Query());
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
