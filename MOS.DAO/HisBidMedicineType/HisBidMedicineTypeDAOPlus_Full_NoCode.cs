using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBidMedicineType
{
    public partial class HisBidMedicineTypeDAO : EntityBase
    {
        public List<V_HIS_BID_MEDICINE_TYPE> GetView(HisBidMedicineTypeSO search, CommonParam param)
        {
            List<V_HIS_BID_MEDICINE_TYPE> result = new List<V_HIS_BID_MEDICINE_TYPE>();
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

        public V_HIS_BID_MEDICINE_TYPE GetViewById(long id, HisBidMedicineTypeSO search)
        {
            V_HIS_BID_MEDICINE_TYPE result = null;

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
