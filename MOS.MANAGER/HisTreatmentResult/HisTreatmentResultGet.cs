using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentResult
{
    class HisTreatmentResultGet : BusinessBase
    {
        internal HisTreatmentResultGet()
            : base()
        {

        }

        internal HisTreatmentResultGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TREATMENT_RESULT> Get(HisTreatmentResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentResultDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_RESULT GetById(long id)
        {
            try
            {
                return GetById(id, new HisTreatmentResultFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_RESULT GetById(long id, HisTreatmentResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentResultDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_RESULT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisTreatmentResultFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_RESULT GetByCode(string code, HisTreatmentResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentResultDAO.GetByCode(code, filter.Query());
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
