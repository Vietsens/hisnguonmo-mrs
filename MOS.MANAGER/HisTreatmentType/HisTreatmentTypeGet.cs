using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentType
{
    partial class HisTreatmentTypeGet : BusinessBase
    {
        internal HisTreatmentTypeGet()
            : base()
        {

        }

        internal HisTreatmentTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TREATMENT_TYPE> Get(HisTreatmentTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisTreatmentTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_TYPE GetById(long id, HisTreatmentTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisTreatmentTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_TYPE GetByCode(string code, HisTreatmentTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentTypeDAO.GetByCode(code, filter.Query());
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
