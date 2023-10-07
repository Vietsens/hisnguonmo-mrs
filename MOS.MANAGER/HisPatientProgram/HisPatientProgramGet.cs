using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientProgram
{
    partial class HisPatientProgramGet : BusinessBase
    {
        internal HisPatientProgramGet()
            : base()
        {

        }

        internal HisPatientProgramGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PATIENT_PROGRAM> Get(HisPatientProgramFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientProgramDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_PROGRAM GetById(long id)
        {
            try
            {
                return GetById(id, new HisPatientProgramFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_PROGRAM GetById(long id, HisPatientProgramFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientProgramDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_PROGRAM> GetByProgramId(long id)
        {
            try
            {
                HisPatientProgramFilterQuery filter = new HisPatientProgramFilterQuery();
                filter.PROGRAM_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_PROGRAM> GetByPatientId(long id)
        {
            try
            {
                HisPatientProgramFilterQuery filter = new HisPatientProgramFilterQuery();
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

        internal List<HIS_PATIENT_PROGRAM> GetByPatientIdAndProgramId(long patientId, long programId)
        {
            try
            {
                HisPatientProgramFilterQuery filter = new HisPatientProgramFilterQuery();
                filter.PATIENT_ID = patientId;
                filter.PROGRAM_ID = programId;
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
