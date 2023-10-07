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
        internal V_HIS_PATIENT_PROGRAM GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisPatientProgramViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_PATIENT_PROGRAM GetViewByCode(string code, HisPatientProgramViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientProgramDAO.GetViewByCode(code, filter.Query());
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
