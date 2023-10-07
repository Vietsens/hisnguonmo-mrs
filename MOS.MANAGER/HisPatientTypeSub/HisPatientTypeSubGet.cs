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
        internal HisPatientTypeSubGet()
            : base()
        {

        }

        internal HisPatientTypeSubGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PATIENT_TYPE_SUB> Get(HisPatientTypeSubFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeSubDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE_SUB GetById(long id)
        {
            try
            {
                return GetById(id, new HisPatientTypeSubFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE_SUB GetById(long id, HisPatientTypeSubFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeSubDAO.GetById(id, filter.Query());
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
