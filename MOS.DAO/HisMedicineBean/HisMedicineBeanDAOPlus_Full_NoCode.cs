using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineBean
{
    public partial class HisMedicineBeanDAO : EntityBase
    {
        public List<V_HIS_MEDICINE_BEAN> GetView(HisMedicineBeanSO search, CommonParam param)
        {
            List<V_HIS_MEDICINE_BEAN> result = new List<V_HIS_MEDICINE_BEAN>();
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

        public V_HIS_MEDICINE_BEAN GetViewById(long id, HisMedicineBeanSO search)
        {
            V_HIS_MEDICINE_BEAN result = null;

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
