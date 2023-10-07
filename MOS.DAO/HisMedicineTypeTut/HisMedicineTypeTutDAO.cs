using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineTypeTut
{
    public partial class HisMedicineTypeTutDAO : EntityBase
    {
        private HisMedicineTypeTutGet GetWorker
        {
            get
            {
                return (HisMedicineTypeTutGet)Worker.Get<HisMedicineTypeTutGet>();
            }
        }
        public List<HIS_MEDICINE_TYPE_TUT> Get(HisMedicineTypeTutSO search, CommonParam param)
        {
            List<HIS_MEDICINE_TYPE_TUT> result = new List<HIS_MEDICINE_TYPE_TUT>();
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

        public HIS_MEDICINE_TYPE_TUT GetById(long id, HisMedicineTypeTutSO search)
        {
            HIS_MEDICINE_TYPE_TUT result = null;
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
