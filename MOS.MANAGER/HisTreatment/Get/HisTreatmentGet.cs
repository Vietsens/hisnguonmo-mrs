using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.Filter;
using MOS.MANAGER.HisDepartmentTran;
using AutoMapper;
using MOS.MANAGER.HisHeinApproval;

namespace MOS.MANAGER.HisTreatment
{
    partial class HisTreatmentGet : GetBase
    {
        internal HisTreatmentGet()
            : base()
        {

        }

        internal HisTreatmentGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TREATMENT> Get(HisTreatmentFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT GetByCode(string treatmentCode)
        {
            try
            {
                HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
                filter.TREATMENT_CODE__EXACT = treatmentCode;
                List<HIS_TREATMENT> data = this.Get(filter);
                return IsNotNullOrEmpty(data) ? data[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TREATMENT> GetView(HisTreatmentViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT GetById(long id)
        {
            try
            {
                return GetById(id, new HisTreatmentFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TREATMENT> GetByIds(List<long> ids)
        {
            HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
            filter.IDs = ids;
            return this.Get(filter);
        }

        internal List<V_HIS_TREATMENT> GetViewByIds(List<long> ids)
        {
            HisTreatmentViewFilterQuery filter = new HisTreatmentViewFilterQuery();
            filter.IDs = ids;
            return this.GetView(filter);
        }

        internal HIS_TREATMENT GetById(long id, HisTreatmentFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisTreatmentViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT GetViewById(long id, HisTreatmentViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TREATMENT> GetByPatientId(long id)
        {
            try
            {
                HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
                filter.PATIENT_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TREATMENT> GetByTreatmentEndTypeId(long id)
        {
            try
            {
                HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
                filter.TREATMENT_END_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TREATMENT> GetByTreatmentResultId(long id)
        {
            try
            {
                HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
                filter.TREATMENT_RESULT_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        //internal List<HIS_TREATMENT> GetByProgramId(long id)
        //{
        //    HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
        //    filter.PROGRAM_ID = id;
        //    return this.Get(filter);
        //}

        internal List<HIS_TREATMENT> GetByAppointmentCode(string appointmentCode)
        {
            HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
            filter.APPOINTMENT_CODE__EXACT = appointmentCode;
            return this.Get(filter);
        }

        internal List<HIS_TREATMENT> GetByOweTypeId(long id)
        {
            HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
            filter.OWE_TYPE_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_TREATMENT> GetByDeathWithinId(long id)
        {
            HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
            filter.DEATH_WITHIN_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_TREATMENT> GetByDeathCauseId(long id)
        {
            HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
            filter.DEATH_CAUSE_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_TREATMENT> GetByTranPatiFormId(long id)
        {
            HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
            filter.TRAN_PATI_FORM_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_TREATMENT> GetByEmergencyWtimeId(long id)
        {
            HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
            filter.EMERGENCY_WTIME_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_TREATMENT> GetByTranPatiReasonId(long id)
        {
            HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
            filter.TRAN_PATI_REASON_ID = id;
            return this.Get(filter);
        }
    }
}
