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
        internal HIS_PATIENT_PROGRAM GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPatientProgramFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_PROGRAM GetByCode(string code, HisPatientProgramFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientProgramDAO.GetByCode(code, filter.Query());
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
