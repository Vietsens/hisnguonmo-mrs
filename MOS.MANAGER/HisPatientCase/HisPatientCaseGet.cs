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
        internal HisPatientCaseGet()
            : base()
        {

        }

        internal HisPatientCaseGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PATIENT_CASE> Get(HisPatientCaseFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientCaseDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_CASE GetById(long id)
        {
            try
            {
                return GetById(id, new HisPatientCaseFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_CASE GetById(long id, HisPatientCaseFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientCaseDAO.GetById(id, filter.Query());
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
