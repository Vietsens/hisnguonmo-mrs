using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentEndType
{
    partial class HisTreatmentEndTypeGet : BusinessBase
    {
        internal HisTreatmentEndTypeGet()
            : base()
        {

        }

        internal HisTreatmentEndTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TREATMENT_END_TYPE> Get(HisTreatmentEndTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentEndTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_END_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisTreatmentEndTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_END_TYPE GetById(long id, HisTreatmentEndTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentEndTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_END_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisTreatmentEndTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_END_TYPE GetByCode(string code, HisTreatmentEndTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentEndTypeDAO.GetByCode(code, filter.Query());
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
