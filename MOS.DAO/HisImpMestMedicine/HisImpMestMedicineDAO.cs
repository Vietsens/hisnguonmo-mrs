using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestMedicine
{
    public partial class HisImpMestMedicineDAO : EntityBase
    {
        private HisImpMestMedicineGet GetWorker
        {
            get
            {
                return (HisImpMestMedicineGet)Worker.Get<HisImpMestMedicineGet>();
            }
        }
        public List<HIS_IMP_MEST_MEDICINE> Get(HisImpMestMedicineSO search, CommonParam param)
        {
            List<HIS_IMP_MEST_MEDICINE> result = new List<HIS_IMP_MEST_MEDICINE>();
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

        public HIS_IMP_MEST_MEDICINE GetById(long id, HisImpMestMedicineSO search)
        {
            HIS_IMP_MEST_MEDICINE result = null;
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
