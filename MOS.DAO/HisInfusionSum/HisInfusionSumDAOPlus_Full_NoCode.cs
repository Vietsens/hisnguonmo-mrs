using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisInfusionSum
{
    public partial class HisInfusionSumDAO : EntityBase
    {
        public List<V_HIS_INFUSION_SUM> GetView(HisInfusionSumSO search, CommonParam param)
        {
            List<V_HIS_INFUSION_SUM> result = new List<V_HIS_INFUSION_SUM>();
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

        public V_HIS_INFUSION_SUM GetViewById(long id, HisInfusionSumSO search)
        {
            V_HIS_INFUSION_SUM result = null;

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
