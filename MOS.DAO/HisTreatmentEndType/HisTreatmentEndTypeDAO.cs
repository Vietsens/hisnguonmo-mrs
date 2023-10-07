using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentEndType
{
    public partial class HisTreatmentEndTypeDAO : EntityBase
    {
        private HisTreatmentEndTypeGet GetWorker
        {
            get
            {
                return (HisTreatmentEndTypeGet)Worker.Get<HisTreatmentEndTypeGet>();
            }
        }
        public List<HIS_TREATMENT_END_TYPE> Get(HisTreatmentEndTypeSO search, CommonParam param)
        {
            List<HIS_TREATMENT_END_TYPE> result = new List<HIS_TREATMENT_END_TYPE>();
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

        public HIS_TREATMENT_END_TYPE GetById(long id, HisTreatmentEndTypeSO search)
        {
            HIS_TREATMENT_END_TYPE result = null;
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
