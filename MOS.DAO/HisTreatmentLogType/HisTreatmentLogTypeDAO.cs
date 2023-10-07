using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentLogType
{
    public partial class HisTreatmentLogTypeDAO : EntityBase
    {
        private HisTreatmentLogTypeGet GetWorker
        {
            get
            {
                return (HisTreatmentLogTypeGet)Worker.Get<HisTreatmentLogTypeGet>();
            }
        }
        public List<HIS_TREATMENT_LOG_TYPE> Get(HisTreatmentLogTypeSO search, CommonParam param)
        {
            List<HIS_TREATMENT_LOG_TYPE> result = new List<HIS_TREATMENT_LOG_TYPE>();
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

        public HIS_TREATMENT_LOG_TYPE GetById(long id, HisTreatmentLogTypeSO search)
        {
            HIS_TREATMENT_LOG_TYPE result = null;
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
