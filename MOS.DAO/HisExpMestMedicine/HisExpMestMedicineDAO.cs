using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestMedicine
{
    public partial class HisExpMestMedicineDAO : EntityBase
    {
        private HisExpMestMedicineGet GetWorker
        {
            get
            {
                return (HisExpMestMedicineGet)Worker.Get<HisExpMestMedicineGet>();
            }
        }
        public List<HIS_EXP_MEST_MEDICINE> Get(HisExpMestMedicineSO search, CommonParam param)
        {
            List<HIS_EXP_MEST_MEDICINE> result = new List<HIS_EXP_MEST_MEDICINE>();
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

        public HIS_EXP_MEST_MEDICINE GetById(long id, HisExpMestMedicineSO search)
        {
            HIS_EXP_MEST_MEDICINE result = null;
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
