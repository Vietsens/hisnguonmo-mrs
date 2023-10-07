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
        internal List<V_HIS_PATIENT_PROGRAM> GetView(HisPatientProgramViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientProgramDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_PATIENT_PROGRAM GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisPatientProgramViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_PATIENT_PROGRAM GetViewById(long id, HisPatientProgramViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientProgramDAO.GetViewById(id, filter.Query());
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
