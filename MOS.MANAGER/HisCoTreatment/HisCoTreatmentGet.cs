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
        internal HisCoTreatmentGet()
            : base()
        {

        }

        internal HisCoTreatmentGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CO_TREATMENT> Get(HisCoTreatmentFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCoTreatmentDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CO_TREATMENT GetById(long id)
        {
            try
            {
                return GetById(id, new HisCoTreatmentFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CO_TREATMENT GetById(long id, HisCoTreatmentFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCoTreatmentDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_CO_TREATMENT> GetByDepartmentTranId(long departmentTranId)
        {
            try
            {
                HisCoTreatmentFilterQuery filter = new HisCoTreatmentFilterQuery();
                filter.DEPARTMENT_TRAN_ID = departmentTranId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        internal List<HIS_CO_TREATMENT> GetByDepartmentId(long departmentId)
        {
            try
            {
                HisCoTreatmentFilterQuery filter = new HisCoTreatmentFilterQuery();
                filter.DEPARTMENT_ID = departmentId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }
    }
}
