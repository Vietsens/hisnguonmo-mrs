using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicinePaty
{
    public partial class HisMedicinePatyDAO : EntityBase
    {
        public List<V_HIS_MEDICINE_PATY> GetView(HisMedicinePatySO search, CommonParam param)
        {
            List<V_HIS_MEDICINE_PATY> result = new List<V_HIS_MEDICINE_PATY>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_MEDICINE_PATY GetViewById(long id, HisMedicinePatySO search)
        {
            V_HIS_MEDICINE_PATY result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
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
