using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeSub
{
    partial class HisPatientTypeSubGet : BusinessBase
    {
        internal HIS_PATIENT_TYPE_SUB GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPatientTypeSubFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE_SUB GetByCode(string code, HisPatientTypeSubFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeSubDAO.GetByCode(code, filter.Query());
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
