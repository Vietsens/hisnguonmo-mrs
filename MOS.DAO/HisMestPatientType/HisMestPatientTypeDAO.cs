using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestPatientType
{
    public partial class HisMestPatientTypeDAO : EntityBase
    {
        private HisMestPatientTypeGet GetWorker
        {
            get
            {
                return (HisMestPatientTypeGet)Worker.Get<HisMestPatientTypeGet>();
            }
        }
        public List<HIS_MEST_PATIENT_TYPE> Get(HisMestPatientTypeSO search, CommonParam param)
        {
            List<HIS_MEST_PATIENT_TYPE> result = new List<HIS_MEST_PATIENT_TYPE>();
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

        public HIS_MEST_PATIENT_TYPE GetById(long id, HisMestPatientTypeSO search)
        {
            HIS_MEST_PATIENT_TYPE result = null;
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
