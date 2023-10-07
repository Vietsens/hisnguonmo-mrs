using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentType
{
    public partial class HisTreatmentTypeDAO : EntityBase
    {
        private HisTreatmentTypeGet GetWorker
        {
            get
            {
                return (HisTreatmentTypeGet)Worker.Get<HisTreatmentTypeGet>();
            }
        }
        public List<HIS_TREATMENT_TYPE> Get(HisTreatmentTypeSO search, CommonParam param)
        {
            List<HIS_TREATMENT_TYPE> result = new List<HIS_TREATMENT_TYPE>();
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

        public HIS_TREATMENT_TYPE GetById(long id, HisTreatmentTypeSO search)
        {
            HIS_TREATMENT_TYPE result = null;
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
