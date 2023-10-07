using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineBean
{
    public partial class HisMedicineBeanDAO : EntityBase
    {
        public List<V_HIS_MEDICINE_BEAN_2> GetView2(HisMedicineBeanSO search, CommonParam param)
        {
            List<V_HIS_MEDICINE_BEAN_2> result = new List<V_HIS_MEDICINE_BEAN_2>();
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

        public V_HIS_MEDICINE_BEAN_2 GetView2ById(long id, HisMedicineBeanSO search)
        {
            V_HIS_MEDICINE_BEAN_2 result = null;

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
