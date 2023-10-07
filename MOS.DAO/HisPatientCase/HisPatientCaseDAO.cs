using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientCase
{
    public partial class HisPatientCaseDAO : EntityBase
    {
        private HisPatientCaseGet GetWorker
        {
            get
            {
                return (HisPatientCaseGet)Worker.Get<HisPatientCaseGet>();
            }
        }
        public List<HIS_PATIENT_CASE> Get(HisPatientCaseSO search, CommonParam param)
        {
            List<HIS_PATIENT_CASE> result = new List<HIS_PATIENT_CASE>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_PATIENT_CASE GetById(long id, HisPatientCaseSO search)
        {
            HIS_PATIENT_CASE result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
