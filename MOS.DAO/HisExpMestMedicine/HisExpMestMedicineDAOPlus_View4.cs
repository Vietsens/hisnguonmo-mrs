using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestMedicine
{
    public partial class HisExpMestMedicineDAO : EntityBase
    {
        public List<V_HIS_EXP_MEST_MEDICINE_4> GetView4(HisExpMestMedicineSO search, CommonParam param)
        {
            List<V_HIS_EXP_MEST_MEDICINE_4> result = new List<V_HIS_EXP_MEST_MEDICINE_4>();
            try
            {
                result = GetWorker.GetView4(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_EXP_MEST_MEDICINE_4 GetView4ById(long id, HisExpMestMedicineSO search)
        {
            V_HIS_EXP_MEST_MEDICINE_4 result = null;

            try
            {
                result = GetWorker.GetView4ById(id, search);
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
