using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentResult
{
    public partial class HisTreatmentResultDAO : EntityBase
    {
        private HisTreatmentResultGet GetWorker
        {
            get
            {
                return (HisTreatmentResultGet)Worker.Get<HisTreatmentResultGet>();
            }
        }
        public List<HIS_TREATMENT_RESULT> Get(HisTreatmentResultSO search, CommonParam param)
        {
            List<HIS_TREATMENT_RESULT> result = new List<HIS_TREATMENT_RESULT>();
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

        public HIS_TREATMENT_RESULT GetById(long id, HisTreatmentResultSO search)
        {
            HIS_TREATMENT_RESULT result = null;
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
