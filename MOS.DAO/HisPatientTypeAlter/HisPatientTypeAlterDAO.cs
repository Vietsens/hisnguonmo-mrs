using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientTypeAlter
{
    public partial class HisPatientTypeAlterDAO : EntityBase
    {
        private HisPatientTypeAlterGet GetWorker
        {
            get
            {
                return (HisPatientTypeAlterGet)Worker.Get<HisPatientTypeAlterGet>();
            }
        }
        public List<HIS_PATIENT_TYPE_ALTER> Get(HisPatientTypeAlterSO search, CommonParam param)
        {
            List<HIS_PATIENT_TYPE_ALTER> result = new List<HIS_PATIENT_TYPE_ALTER>();
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

        public HIS_PATIENT_TYPE_ALTER GetById(long id, HisPatientTypeAlterSO search)
        {
            HIS_PATIENT_TYPE_ALTER result = null;
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
