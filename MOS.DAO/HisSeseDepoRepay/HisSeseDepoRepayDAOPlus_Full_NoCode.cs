using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSeseDepoRepay
{
    public partial class HisSeseDepoRepayDAO : EntityBase
    {
        public List<V_HIS_SESE_DEPO_REPAY> GetView(HisSeseDepoRepaySO search, CommonParam param)
        {
            List<V_HIS_SESE_DEPO_REPAY> result = new List<V_HIS_SESE_DEPO_REPAY>();
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

        public V_HIS_SESE_DEPO_REPAY GetViewById(long id, HisSeseDepoRepaySO search)
        {
            V_HIS_SESE_DEPO_REPAY result = null;

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
