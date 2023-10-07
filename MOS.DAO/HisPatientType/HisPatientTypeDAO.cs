using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientType
{
    public partial class HisPatientTypeDAO : EntityBase
    {
        private HisPatientTypeGet GetWorker
        {
            get
            {
                return (HisPatientTypeGet)Worker.Get<HisPatientTypeGet>();
            }
        }
        public List<HIS_PATIENT_TYPE> Get(HisPatientTypeSO search, CommonParam param)
        {
            List<HIS_PATIENT_TYPE> result = new List<HIS_PATIENT_TYPE>();
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

        public HIS_PATIENT_TYPE GetById(long id, HisPatientTypeSO search)
        {
            HIS_PATIENT_TYPE result = null;
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
