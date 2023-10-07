using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineTypeAcin
{
    public partial class HisMedicineTypeAcinDAO : EntityBase
    {
        public List<V_HIS_MEDICINE_TYPE_ACIN> GetView(HisMedicineTypeAcinSO search, CommonParam param)
        {
            List<V_HIS_MEDICINE_TYPE_ACIN> result = new List<V_HIS_MEDICINE_TYPE_ACIN>();
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

        public V_HIS_MEDICINE_TYPE_ACIN GetViewById(long id, HisMedicineTypeAcinSO search)
        {
            V_HIS_MEDICINE_TYPE_ACIN result = null;

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
