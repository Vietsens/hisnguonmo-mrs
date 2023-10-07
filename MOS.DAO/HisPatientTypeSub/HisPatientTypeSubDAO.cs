using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientTypeSub
{
    public partial class HisPatientTypeSubDAO : EntityBase
    {
        private HisPatientTypeSubGet GetWorker
        {
            get
            {
                return (HisPatientTypeSubGet)Worker.Get<HisPatientTypeSubGet>();
            }
        }
        public List<HIS_PATIENT_TYPE_SUB> Get(HisPatientTypeSubSO search, CommonParam param)
        {
            List<HIS_PATIENT_TYPE_SUB> result = new List<HIS_PATIENT_TYPE_SUB>();
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

        public HIS_PATIENT_TYPE_SUB GetById(long id, HisPatientTypeSubSO search)
        {
            HIS_PATIENT_TYPE_SUB result = null;
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
