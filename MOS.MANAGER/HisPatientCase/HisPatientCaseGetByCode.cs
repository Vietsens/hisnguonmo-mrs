using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientCase
{
    partial class HisPatientCaseGet : BusinessBase
    {
        internal HIS_PATIENT_CASE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPatientCaseFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_CASE GetByCode(string code, HisPatientCaseFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientCaseDAO.GetByCode(code, filter.Query());
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
