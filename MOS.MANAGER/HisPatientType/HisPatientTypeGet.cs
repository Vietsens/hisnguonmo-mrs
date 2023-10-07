using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientType
{
    partial class HisPatientTypeGet : BusinessBase
    {
        internal HisPatientTypeGet()
            : base()
        {

        }

        internal HisPatientTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PATIENT_TYPE> Get(HisPatientTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE> GetActive()
        {
            try
            {
                HisPatientTypeFilterQuery filter = new HisPatientTypeFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE> GetIsCoPayment()
        {
            try
            {
                HisPatientTypeFilterQuery filter = new HisPatientTypeFilterQuery();
                filter.IS_COPAYMENT = true;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisPatientTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE> GetByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisPatientTypeFilterQuery filter = new HisPatientTypeFilterQuery();
                filter.IDs = ids;
                return this.Get(filter);
            }
            return null;
        }

        internal HIS_PATIENT_TYPE GetById(long id, HisPatientTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPatientTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE GetByCode(string code, HisPatientTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeDAO.GetByCode(code, filter.Query());
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
