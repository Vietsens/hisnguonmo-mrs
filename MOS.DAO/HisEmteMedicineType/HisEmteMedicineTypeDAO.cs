using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEmteMedicineType
{
    public partial class HisEmteMedicineTypeDAO : EntityBase
    {
        private HisEmteMedicineTypeGet GetWorker
        {
            get
            {
                return (HisEmteMedicineTypeGet)Worker.Get<HisEmteMedicineTypeGet>();
            }
        }
        public List<HIS_EMTE_MEDICINE_TYPE> Get(HisEmteMedicineTypeSO search, CommonParam param)
        {
            List<HIS_EMTE_MEDICINE_TYPE> result = new List<HIS_EMTE_MEDICINE_TYPE>();
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

        public HIS_EMTE_MEDICINE_TYPE GetById(long id, HisEmteMedicineTypeSO search)
        {
            HIS_EMTE_MEDICINE_TYPE result = null;
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
