using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatment
{
    public partial class HisTreatmentDAO : EntityBase
    {
        private HisTreatmentGet GetWorker
        {
            get
            {
                return (HisTreatmentGet)Worker.Get<HisTreatmentGet>();
            }
        }
        public List<HIS_TREATMENT> Get(HisTreatmentSO search, CommonParam param)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
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

        public HIS_TREATMENT GetById(long id, HisTreatmentSO search)
        {
            HIS_TREATMENT result = null;
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
