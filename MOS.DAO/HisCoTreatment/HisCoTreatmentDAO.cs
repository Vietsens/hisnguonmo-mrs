using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCoTreatment
{
    public partial class HisCoTreatmentDAO : EntityBase
    {
        private HisCoTreatmentGet GetWorker
        {
            get
            {
                return (HisCoTreatmentGet)Worker.Get<HisCoTreatmentGet>();
            }
        }

        public List<HIS_CO_TREATMENT> Get(HisCoTreatmentSO search, CommonParam param)
        {
            List<HIS_CO_TREATMENT> result = new List<HIS_CO_TREATMENT>();
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

        public HIS_CO_TREATMENT GetById(long id, HisCoTreatmentSO search)
        {
            HIS_CO_TREATMENT result = null;
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
