using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentHurt
{
    public partial class HisAccidentHurtDAO : EntityBase
    {
        public List<V_HIS_ACCIDENT_HURT> GetView(HisAccidentHurtSO search, CommonParam param)
        {
            List<V_HIS_ACCIDENT_HURT> result = new List<V_HIS_ACCIDENT_HURT>();
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

        public V_HIS_ACCIDENT_HURT GetViewById(long id, HisAccidentHurtSO search)
        {
            V_HIS_ACCIDENT_HURT result = null;

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
