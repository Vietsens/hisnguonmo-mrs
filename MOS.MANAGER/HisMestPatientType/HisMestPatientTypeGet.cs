using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPatientType
{
    class HisMestPatientTypeGet : GetBase
    {
        internal HisMestPatientTypeGet()
            : base()
        {

        }

        internal HisMestPatientTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_PATIENT_TYPE> Get(HisMestPatientTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPatientTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEST_PATIENT_TYPE> GetView(HisMestPatientTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPatientTypeDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PATIENT_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestPatientTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PATIENT_TYPE GetById(long id, HisMestPatientTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPatientTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_MEST_PATIENT_TYPE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMestPatientTypeViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEST_PATIENT_TYPE GetViewById(long id, HisMestPatientTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPatientTypeDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_PATIENT_TYPE> GetByMediStockId(long id)
        {
            try
            {
                HisMestPatientTypeFilterQuery filter = new HisMestPatientTypeFilterQuery();
                filter.MEDI_STOCK_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_PATIENT_TYPE> GetByPatientTypeId(long patientTypeId)
        {
            try
            {
                HisMestPatientTypeFilterQuery filter = new HisMestPatientTypeFilterQuery();
                filter.PATIENT_TYPE_ID = patientTypeId;
                return this.Get(filter);
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
