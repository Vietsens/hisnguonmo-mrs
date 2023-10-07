using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatient
{
    public partial class HisPatientDAO : EntityBase
    {
        private HisPatientGet GetWorker
        {
            get
            {
                return (HisPatientGet)Worker.Get<HisPatientGet>();
            }
        }
        public List<HIS_PATIENT> Get(HisPatientSO search, CommonParam param)
        {
            List<HIS_PATIENT> result = new List<HIS_PATIENT>();
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

        public HIS_PATIENT GetById(long id, HisPatientSO search)
        {
            HIS_PATIENT result = null;
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
