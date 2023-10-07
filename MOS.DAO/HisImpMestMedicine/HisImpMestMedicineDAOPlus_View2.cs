using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestMedicine
{
    public partial class HisImpMestMedicineDAO : EntityBase
    {
        public List<V_HIS_IMP_MEST_MEDICINE_2> GetView2(HisImpMestMedicineSO search, CommonParam param)
        {
            List<V_HIS_IMP_MEST_MEDICINE_2> result = new List<V_HIS_IMP_MEST_MEDICINE_2>();
            try
            {
                result = GetWorker.GetView2(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_IMP_MEST_MEDICINE_2 GetView2ById(long id, HisImpMestMedicineSO search)
        {
            V_HIS_IMP_MEST_MEDICINE_2 result = null;

            try
            {
                result = GetWorker.GetView2ById(id, search);
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
